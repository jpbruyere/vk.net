#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (set = 1, binding = 0) uniform sampler2D samplerColor;
layout (set = 1, binding = 1) uniform sampler2D samplerNormal;
layout (set = 1, binding = 2) uniform sampler2D samplerOcclusion;
layout (set = 1, binding = 3) uniform sampler2D samplerMetalRoughness;
layout (set = 1, binding = 4) uniform sampler2D samplerEmissive;

layout (location = 0) in vec2 inUV;
layout (location = 1) in vec3 inN;
layout (location = 2) in vec3 inV;
layout (location = 3) in vec3 inWorldPos;

layout (location = 0) out vec4 outColorRough;
layout (location = 1) out vec4 outEmitMetal;
layout (location = 2) out vec4 outN;
layout (location = 3) out vec4 outPos;

layout(push_constant) uniform PushConsts {
    layout(offset = 64)
    vec4 baseColorFactor;
    vec4 emissiveFactor;
    uint availableAttachments;
    uint alphaMode;
    float alphaCutoff;
    float metallicFactor;
    float roughnessFactor;
} pc;

const uint MAP_COLOR = 0x1;
const uint MAP_NORMAL = 0x2;
const uint MAP_AO = 0x4;
const uint MAP_METAL = 0x8;
const uint MAP_ROUGHNESS = 0x16;
const uint MAP_METALROUGHNESS = 0x32;
const uint MAP_EMISSIVE = 0x64;

// See http://www.thetenthplanet.de/archives/1180
vec3 perturbNormal(vec3 inNormal, vec3 tangentNormal)
{
    

    vec3 q1 = dFdx(inWorldPos);
    vec3 q2 = dFdy(inWorldPos);
    vec2 st1 = dFdx(inUV);
    vec2 st2 = dFdy(inUV);

    vec3 N = normalize(inNormal);
    vec3 T = normalize(q1 * st2.t - q2 * st1.t);
    vec3 B = -normalize(cross(N, T));
    mat3 TBN = mat3(T, B, N);

    return normalize(TBN * tangentNormal);
}

layout (constant_id = 0) const float NEAR_PLANE = 0.1f;
layout (constant_id = 1) const float FAR_PLANE = 256.0f;

float linearDepth(float depth)
{
    float z = depth * 2.0f - 1.0f; 
    return (2.0f * NEAR_PLANE * FAR_PLANE) / (FAR_PLANE + NEAR_PLANE - z * (FAR_PLANE - NEAR_PLANE));   
}

void main() 
{
    vec3 emit = vec3(0);
    vec4 base_color = pc.baseColorFactor;
    float rough = pc.roughnessFactor;
    float metallic = pc.metallicFactor;
    
    if ((pc.availableAttachments & MAP_COLOR) == MAP_COLOR)
        base_color *= texture(samplerColor, inUV);    
    if ((pc.availableAttachments & MAP_METALROUGHNESS) == MAP_METALROUGHNESS) {
        rough *= clamp(texture(samplerMetalRoughness, inUV).g, 0.04, 1.0);
        metallic *= texture(samplerMetalRoughness, inUV).b;
    }    
    if ((pc.availableAttachments & MAP_EMISSIVE) == MAP_EMISSIVE)   
        emit = pc.emissiveFactor.rgb * texture(samplerEmissive, inUV).rgb;
        
    vec3 N = ((pc.availableAttachments & MAP_NORMAL) == MAP_NORMAL) ?
        perturbNormal(inN, texture(samplerNormal, inUV).xyz * 2.0 - 1.0) :
        normalize(inN);    
    vec3 V = normalize(inV);
    
    outColorRough = vec4 (base_color.rgb, rough);
    outEmitMetal = vec4 (emit,metallic);
    outN = vec4 (N,1);
    outPos = vec4 (V, linearDepth(gl_FragCoord.z));
}