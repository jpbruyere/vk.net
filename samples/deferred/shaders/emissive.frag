#version 450
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (input_attachment_index = 0, set = 2, binding = 0) uniform subpassInputMS samplerColorRough;
layout (input_attachment_index = 1, set = 2, binding = 1) uniform subpassInputMS samplerEmitMetal;
layout (input_attachment_index = 2, set = 2, binding = 2) uniform subpassInputMS samplerN_AO;
layout (input_attachment_index = 3, set = 2, binding = 3) uniform subpassInputMS samplerPos;

layout (location = 0) in vec2 inUV;

layout (location = 0) out vec4 outColor;

const float offset[5] = float[](0.0, 1.0, 2.0, 3.0, 4.0);
const float weight[5] = float[](0.2270270270, 0.1945945946, 0.1216216216,
                                  0.0540540541, 0.0162162162);
 
void main() 
{
    if (subpassLoad(samplerPos, gl_SampleID).a == 1.0f)
        discard;
    vec3 emissive = subpassLoad (samplerEmitMetal, gl_SampleID).rgb;        
        
    outColor = vec4(emissive, 1);
}