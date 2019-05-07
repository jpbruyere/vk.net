#version 450
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

#define MANUAL_SRGB 0
#define DEBUG 0

layout (constant_id = 0) const float NEAR_PLANE = 0.1f;
layout (constant_id = 1) const float FAR_PLANE = 256.0f;
layout (constant_id = 2) const int MAT_COUNT = 1;

struct Material {
    vec4 baseColorFactor;
    vec4 emissiveFactor;
    vec4 diffuseFactor;
    vec4 specularFactor;
    
    float workflow;    
    uint tex0;    
    uint tex1;    
    int baseColorTex;
    
    int physicalDescTex;    
    int normalTex;
    int occlusionTex;
    int emissiveTex;
    
    float metallicFactor;       
    float roughnessFactor;  
    float alphaMask;    
    float alphaMaskCutoff;    
};
const float M_PI = 3.141592653589793;
const float c_MinRoughness = 0.04;

const float PBR_WORKFLOW_METALLIC_ROUGHNESS = 1.0;
const float PBR_WORKFLOW_SPECULAR_GLOSINESS = 2.0f;

const uint MAP_COLOR = 0x1;
const uint MAP_NORMAL = 0x2;
const uint MAP_AO = 0x4;
const uint MAP_METAL = 0x8;
const uint MAP_ROUGHNESS = 0x10;
const uint MAP_METALROUGHNESS = 0x20;
const uint MAP_EMISSIVE = 0x40;

layout (location = 0) in vec3 inWorldPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec2 inUV0;
layout (location = 3) in vec2 inUV1;

layout (set = 0, binding = 5) uniform UBOMaterials {
    Material materials[MAT_COUNT];
};
layout (set = 0, binding = 7) uniform sampler2DArray texArray;

layout (push_constant) uniform PushCsts {
    layout(offset = 64)
    int materialIdx;
};


layout (location = 0) out vec4 outColorRough;
layout (location = 1) out vec4 outEmitMetal;
layout (location = 2) out vec4 outN_AO;
layout (location = 3) out vec4 outPos;

vec4 SRGBtoLINEAR(vec4 srgbIn)
{
    #ifdef MANUAL_SRGB
    #ifdef SRGB_FAST_APPROXIMATION
    vec3 linOut = pow(srgbIn.xyz,vec3(2.2));
    #else //SRGB_FAST_APPROXIMATION
    vec3 bLess = step(vec3(0.04045),srgbIn.xyz);
    vec3 linOut = mix( srgbIn.xyz/vec3(12.92), pow((srgbIn.xyz+vec3(0.055))/vec3(1.055),vec3(2.4)), bLess );
    #endif //SRGB_FAST_APPROXIMATION
    return vec4(linOut,srgbIn.w);;
    #else //MANUAL_SRGB
    return srgbIn;
    #endif //MANUAL_SRGB
}

// Find the normal for this fragment, pulling either from a predefined normal map
// or from the interpolated mesh normal and tangent attributes.
vec3 getNormal()
{
    vec3 tangentNormal;
    // Perturb normal, see http://www.thetenthplanet.de/archives/1180
    if ((materials[materialIdx].tex0 & MAP_NORMAL) == MAP_NORMAL)
        tangentNormal = texture(texArray, vec3(inUV0, materials[materialIdx].normalTex)).xyz * 2.0 - 1.0;
    else if ((materials[materialIdx].tex1 & MAP_NORMAL) == MAP_NORMAL)
        tangentNormal = texture(texArray, vec3(inUV1, materials[materialIdx].normalTex)).xyz * 2.0 - 1.0;
    else
        return normalize(inNormal);
        
    vec3 q1 = dFdx(inWorldPos);
    vec3 q2 = dFdy(inWorldPos);
    vec2 st1 = dFdx(inUV0);
    vec2 st2 = dFdy(inUV0);

    vec3 N = normalize(inNormal);
    vec3 T = normalize(q1 * st2.t - q2 * st1.t);
    vec3 B = -normalize(cross(N, T));
    mat3 TBN = mat3(T, B, N);

    return normalize(TBN * tangentNormal);
}

// Gets metallic factor from specular glossiness workflow inputs 
float convertMetallic(vec3 diffuse, vec3 specular, float maxSpecular) {
    float perceivedDiffuse = sqrt(0.299 * diffuse.r * diffuse.r + 0.587 * diffuse.g * diffuse.g + 0.114 * diffuse.b * diffuse.b);
    float perceivedSpecular = sqrt(0.299 * specular.r * specular.r + 0.587 * specular.g * specular.g + 0.114 * specular.b * specular.b);
    if (perceivedSpecular < c_MinRoughness) {
        return 0.0;
    }
    float a = c_MinRoughness;
    float b = perceivedDiffuse * (1.0 - maxSpecular) / (1.0 - c_MinRoughness) + perceivedSpecular - 2.0 * c_MinRoughness;
    float c = c_MinRoughness - perceivedSpecular;
    float D = max(b * b - 4.0 * a * c, 0.0);
    return clamp((-b + sqrt(D)) / (2.0 * a), 0.0, 1.0);
}

float linearDepth(float depth)
{
    float z = depth * 2.0f - 1.0f; 
    return (2.0f * NEAR_PLANE * FAR_PLANE) / (FAR_PLANE + NEAR_PLANE - z * (FAR_PLANE - NEAR_PLANE));   
}

void main() 
{
    float perceptualRoughness;
    float metallic;    
    vec4 baseColor;
    vec3 emissive = vec3(0);    
    
    baseColor = materials[materialIdx].baseColorFactor;
    
    if (materials[materialIdx].workflow == PBR_WORKFLOW_METALLIC_ROUGHNESS) {
        perceptualRoughness = materials[materialIdx].roughnessFactor;
        metallic = materials[materialIdx].metallicFactor;        
        // Roughness is stored in the 'g' channel, metallic is stored in the 'b' channel.
        // This layout intentionally reserves the 'r' channel for (optional) occlusion map data
        if ((materials[materialIdx].tex0 & MAP_METALROUGHNESS) == MAP_METALROUGHNESS){
            perceptualRoughness *= texture(texArray, vec3(inUV0, materials[materialIdx].physicalDescTex)).g;            
            metallic *= texture(texArray, vec3(inUV0, materials[materialIdx].physicalDescTex)).b;
        }else if ((materials[materialIdx].tex1 & MAP_METALROUGHNESS) == MAP_METALROUGHNESS){
            perceptualRoughness *= texture(texArray, vec3(inUV1, materials[materialIdx].physicalDescTex)).g;            
            metallic *= texture(texArray, vec3(inUV1, materials[materialIdx].physicalDescTex)).b;
        }               
        perceptualRoughness = clamp(perceptualRoughness, c_MinRoughness, 1.0);
        metallic = clamp(metallic, 0.0, 1.0);        

        // The albedo may be defined from a base texture or a flat color
        if ((materials[materialIdx].tex0 & MAP_COLOR) == MAP_COLOR)        
            baseColor *= SRGBtoLINEAR(texture(texArray, vec3(inUV0, materials[materialIdx].baseColorTex)));
        else if ((materials[materialIdx].tex1 & MAP_COLOR) == MAP_COLOR)
            baseColor *= SRGBtoLINEAR(texture(texArray, vec3(inUV1, materials[materialIdx].baseColorTex)));
    }
    
    if (materials[materialIdx].alphaMask == 1.0f) {            
        if (baseColor.a < materials[materialIdx].alphaMaskCutoff) 
            discard;        
    }

    if (materials[materialIdx].workflow == PBR_WORKFLOW_SPECULAR_GLOSINESS) {
        // Values from specular glossiness workflow are converted to metallic roughness
        if ((materials[materialIdx].tex0 & MAP_METALROUGHNESS) == MAP_METALROUGHNESS)
            perceptualRoughness = 1.0 - texture(texArray, vec3(inUV0, materials[materialIdx].physicalDescTex)).a;            
        else if ((materials[materialIdx].tex1 & MAP_METALROUGHNESS) == MAP_METALROUGHNESS)
            perceptualRoughness = 1.0 - texture(texArray, vec3(inUV1, materials[materialIdx].physicalDescTex)).a;            
        else
            perceptualRoughness = 0.0;

        const float epsilon = 1e-6;

        vec4 diffuse = SRGBtoLINEAR(texture(texArray, vec3(inUV0, materials[materialIdx].baseColorTex)));
        vec3 specular = SRGBtoLINEAR(texture(texArray, vec3(inUV0, materials[materialIdx].physicalDescTex))).rgb;

        float maxSpecular = max(max(specular.r, specular.g), specular.b);

        // Convert metallic value from specular glossiness inputs
        metallic = convertMetallic(diffuse.rgb, specular, maxSpecular);

        vec3 baseColorDiffusePart = diffuse.rgb * ((1.0 - maxSpecular) / (1 - c_MinRoughness) / max(1 - metallic, epsilon)) * materials[materialIdx].diffuseFactor.rgb;
        vec3 baseColorSpecularPart = specular - (vec3(c_MinRoughness) * (1 - metallic) * (1 / max(metallic, epsilon))) * materials[materialIdx].specularFactor.rgb;
        baseColor = vec4(mix(baseColorDiffusePart, baseColorSpecularPart, metallic * metallic), diffuse.a);

    }        
    
    const float u_OcclusionStrength = 1.0f;
    const float u_EmissiveFactor = 1.0f;
    float ao = 1.0f;
    
    if ((materials[materialIdx].tex0 & MAP_EMISSIVE) == MAP_EMISSIVE)    
        emissive = SRGBtoLINEAR(texture(texArray, vec3(inUV0, materials[materialIdx].emissiveTex))).rgb * u_EmissiveFactor;
    else if ((materials[materialIdx].tex1 & MAP_EMISSIVE) == MAP_EMISSIVE)    
        emissive = SRGBtoLINEAR(texture(texArray, vec3(inUV1, materials[materialIdx].emissiveTex))).rgb * u_EmissiveFactor;
    
    if ((materials[materialIdx].tex0 & MAP_AO) == MAP_AO)
        ao = texture(texArray, vec3(inUV0, materials[materialIdx].occlusionTex)).r;
    else if ((materials[materialIdx].tex1 & MAP_AO) == MAP_AO)
        ao = texture(texArray, vec3(inUV1, materials[materialIdx].occlusionTex)).r;
        
    vec3 n = getNormal();    
    vec3 p = inWorldPos;
    
    outColorRough = vec4 (baseColor.rgb, perceptualRoughness);
    outEmitMetal = vec4 (emissive, metallic);
    outN_AO = vec4 (n, ao);
    outPos = vec4 (p, linearDepth(gl_FragCoord.z));
}