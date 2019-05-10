#version 450
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (input_attachment_index = 0, set = 1, binding = 0) uniform subpassInputMS samplerColorRough;
layout (input_attachment_index = 1, set = 1, binding = 1) uniform subpassInputMS samplerEmitMetal;
layout (input_attachment_index = 2, set = 1, binding = 2) uniform subpassInputMS samplerN_AO;
layout (input_attachment_index = 3, set = 1, binding = 3) uniform subpassInputMS samplerPos;

layout (set = 0, binding = 1) uniform samplerCube samplerIrradiance;
layout (set = 0, binding = 2) uniform samplerCube prefilteredMap;
layout (set = 0, binding = 3) uniform sampler2D samplerBRDFLUT;
layout (set = 0, binding = 6) uniform sampler2DArray samplerShadowMap;

layout (push_constant) uniform PushCsts {
    layout(offset = 64)
    int imgIdx;
};

layout (location = 0) in vec2 inUV;
layout (location = 0) out vec4 outColor;

const uint color        = 0;
const uint normal       = 1;
const uint pos          = 2;
const uint occlusion    = 3;
const uint emissive     = 4;
const uint metallic     = 5;
const uint roughness    = 6;
const uint depth        = 7;
const uint prefill      = 8;
const uint irradiance   = 9;
const uint shadowMap    = 10;

vec4 sampleCubeMap (samplerCube sc, uint face, uint lod) {
    vec2 uv = 2.0 * inUV - vec2(1.0);
    switch (face) {
        case 0:
            return vec4 (textureLod (sc, vec3(1, uv.t, uv.s), lod).rgb, 1);
        case 1:
            return vec4 (textureLod (sc, vec3(-1, uv.t, uv.s), lod).rgb, 1);
        case 2:
            return vec4 (textureLod (sc, vec3(uv.s, 1, -uv.t), lod).rgb, 1);
        case 3:
            return vec4 (textureLod (sc, vec3(uv.s, -1, uv.t), lod).rgb, 1);
        case 4:
            return vec4 (textureLod (sc, vec3(uv, 1), lod).rgb, 1);
        case 5:
            return vec4 (textureLod (sc, vec3(-uv.s, uv.t, -1), lod).rgb, 1);
   }
}

void main() 
{
    uint imgNum = bitfieldExtract (imgIdx, 0, 8);
    switch (imgNum) {
        case color:
            outColor = vec4(subpassLoad(samplerColorRough, gl_SampleID).rgb, 1);
            break;
        case normal:
            outColor = vec4(subpassLoad(samplerN_AO, gl_SampleID).rgb, 1);
            break;
        case pos:
            outColor = vec4(subpassLoad(samplerPos, gl_SampleID).rgb, 1);
            break;
        case occlusion:
            outColor = vec4(subpassLoad(samplerN_AO, gl_SampleID).aaa, 1);
            break;
        case emissive:
            outColor = vec4(subpassLoad(samplerEmitMetal, gl_SampleID).rgb, 1);
            break;
        case metallic:
            outColor = vec4(subpassLoad(samplerEmitMetal, gl_SampleID).aaa, 1);
            break;
        case roughness:
            outColor = vec4(subpassLoad(samplerColorRough, gl_SampleID).aaa, 1);
            break;
        case depth:
            outColor = vec4(subpassLoad(samplerPos, gl_SampleID).aaa, 1);
            break;
        case shadowMap:            
            vec3 d = texture(samplerShadowMap, vec3(inUV, bitfieldExtract (imgIdx, 8, 8))).rrr;
            outColor = vec4(d*d*d, 1);
            break;
        default:
            if (imgNum == prefill)
                outColor = sampleCubeMap (prefilteredMap, bitfieldExtract (imgIdx, 8, 8), bitfieldExtract (imgIdx, 16, 8));            
            else if (imgNum == irradiance)
                outColor = sampleCubeMap (samplerIrradiance, bitfieldExtract (imgIdx, 8, 8), bitfieldExtract (imgIdx, 16, 8));            
            break;
    }
}