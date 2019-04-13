#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (binding = 0) uniform UBO 
{
    mat4 projectionMatrix;
    mat4 viewMatrix;
    mat4 modelMatrix;
    vec4 lightPos;
    float gamma;
    float exposure;    
} ubo;

layout (set = 0, binding = 1) uniform samplerCube samplerCubemap;
layout (set = 0, binding = 2) uniform sampler2D samplerBRDFLUT;
layout (set = 0, binding = 3) uniform samplerCube samplerIrradiance;
layout (set = 0, binding = 4) uniform samplerCube prefilteredMap;

layout (set = 1, binding = 0) uniform sampler2D samplerColor;
layout (set = 1, binding = 1) uniform sampler2D samplerNormal;
layout (set = 1, binding = 2) uniform sampler2D samplerOcclusion;
layout (set = 1, binding = 3) uniform sampler2D samplerMetalRoughness;
layout (set = 1, binding = 4) uniform sampler2D samplerEmissive;

layout (location = 0) in vec2 inUV;
layout (location = 1) in vec3 inN;
layout (location = 2) in vec3 inV;
layout (location = 3) in vec3 inWorldPos;

layout (location = 0) out vec4 outFragColor;

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

const float PI = 3.141592653589793;

// From http://filmicgames.com/archives/75
vec3 Uncharted2Tonemap(vec3 x)
{
    float A = 0.15;
    float B = 0.50;
    float C = 0.10;
    float D = 0.20;
    float E = 0.02;
    float F = 0.30;
    return ((x*(A*x+C*B)+D*E)/(x*(A*x+B)+D*F))-E/F;
}

// Normal Distribution function --------------------------------------
float D_GGX(float dotNH, float roughness)
{
    float alpha = roughness * roughness;
    float alpha2 = alpha * alpha;
    float denom = dotNH * dotNH * (alpha2 - 1.0) + 1.0;
    return (alpha2)/(PI * denom*denom);
}

// Geometric Shadowing function --------------------------------------
float G_SchlicksmithGGX(float dotNL, float dotNV, float roughness)
{
    float r = (roughness + 1.0);
    float k = (r*r) / 8.0;
    float GL = dotNL / (dotNL * (1.0 - k) + k);
    float GV = dotNV / (dotNV * (1.0 - k) + k);
    return GL * GV;
}

// Fresnel function ----------------------------------------------------
vec3 F_Schlick(float cosTheta, vec3 F0)
{
    return F0 + (1.0 - F0) * pow(1.0 - cosTheta, 5.0);
}
vec3 F_SchlickR(float cosTheta, vec3 F0, float roughness)
{
    return F0 + (max(vec3(1.0 - roughness), F0) - F0) * pow(1.0 - cosTheta, 5.0);
}

vec3 specularContribution(vec3 L, vec3 V, vec3 N, vec3 F0, float metallic, float roughness, vec3 albedo)
{
    // Precalculate vectors and dot products
    vec3 H = normalize (V + L);
    float dotNH = clamp(dot(N, H), 0.0, 1.0);
    float dotNV = clamp(dot(N, V), 0.0, 1.0);
    float dotNL = clamp(dot(N, L), 0.0, 1.0);

    // Light color fixed
    vec3 lightColor = vec3(1.0);

    vec3 color = vec3(0.0);

    if (dotNL > 0.0) {
        // D = Normal distribution (Distribution of the microfacets)
        float D = D_GGX(dotNH, roughness);
        // G = Geometric shadowing term (Microfacets shadowing)
        float G = G_SchlicksmithGGX(dotNL, dotNV, roughness);
        // F = Fresnel factor (Reflectance depending on angle of incidence)
        vec3 F = F_Schlick(dotNV, F0);
        vec3 spec = D * F * G / (4.0 * dotNL * dotNV + 0.001);
        vec3 kD = (vec3(1.0) - F) * (1.0 - metallic);
        color += (kD * albedo / PI + spec) * dotNL;
    }

    return color;
}

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

vec3 prefilteredReflection(vec3 R, float roughness)
{
    const float MAX_REFLECTION_LOD = 9.0; // todo: param/const
    float lod = roughness * MAX_REFLECTION_LOD;
    float lodf = floor(lod);
    float lodc = ceil(lod);
    vec3 a = textureLod(prefilteredMap, R, lodf).rgb;
    vec3 b = textureLod(prefilteredMap, R, lodc).rgb;
    return mix(a, b, lod - lodf);
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
    vec3 R = -normalize(reflect(V, N));
    
    vec3 F0 = vec3(0.04);
    F0 = mix(F0, base_color.rgb, metallic);
    float alpha = pow(rough, 2.0);

    vec3 L = normalize(ubo.lightPos.xyz);
    vec3 Lo = specularContribution(L, V, N, F0, metallic, rough, base_color.rgb);
    
    vec2 brdf = texture(samplerBRDFLUT, vec2(max(dot(N, V), 0.0), rough)).rg;
    vec3 reflection = prefilteredReflection(R, rough).rgb; //texture(samplerCubemap, R).rgb * (1.0-rough);    
    vec3 irradiance = texture(samplerIrradiance, N).rgb;

    // Diffuse based on irradiance
    vec3 diffuse = base_color.rgb;// * irradiance;

    vec3 F = F_SchlickR(max(dot(N, V), 0.0), F0, rough);
        
    vec3 specular = reflection * (F * brdf.x + brdf.y);

    // Ambient part
    vec3 kD = 1.0 - F;
    kD *= 1.0 - metallic;
    
    float ao = ((pc.availableAttachments & MAP_AO) == MAP_AO) ? texture(samplerOcclusion, inUV).r : 1.0f;
    vec3 ambient = (kD * diffuse + specular) * ao;
    vec3 color = ambient + Lo;

    // Tone mapping
    color = Uncharted2Tonemap(color * ubo.exposure);
    color = color * (1.0f / Uncharted2Tonemap(vec3(11.2f)));
    // Gamma correction
    color = pow(color, vec3(1.0f / ubo.gamma)) + emit;

    outFragColor = vec4(color, base_color.a);
}