#version 450
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (set = 0, binding = 0) uniform UBO {
    mat4 projection;
    mat4 model;
    mat4 view;
    vec4 camPos;    
    float exposure;
    float gamma;
    float prefilteredCubeMipLevels;
    float scaleIBLAmbient;
} ubo;

layout (constant_id = 0) const uint NUM_LIGHTS = 1;

struct Light {
    vec4 position;
    vec4 color;
    mat4 mvp;
};

layout (set = 0, binding = 4) uniform UBOLights {
    Light lights[NUM_LIGHTS];
};

const float M_PI = 3.141592653589793;
const float c_MinRoughness = 0.04;
#define SHADOW_FACTOR 0.15

layout (input_attachment_index = 0, set = 1, binding = 0) uniform subpassInputMS samplerColorRough;
layout (input_attachment_index = 1, set = 1, binding = 1) uniform subpassInputMS samplerEmitMetal;
layout (input_attachment_index = 2, set = 1, binding = 2) uniform subpassInputMS samplerN_AO;
layout (input_attachment_index = 3, set = 1, binding = 3) uniform subpassInputMS samplerPos;

layout (set = 0, binding = 1) uniform samplerCube samplerIrradiance;
layout (set = 0, binding = 2) uniform samplerCube prefilteredMap;
layout (set = 0, binding = 3) uniform sampler2D samplerBRDFLUT;
layout (set = 0, binding = 6) uniform sampler2DArray samplerShadowMap;

layout (location = 0) in vec2 inUV;

layout (location = 0) out vec4 outColor;

// Encapsulate the various inputs used by the various functions in the shading equation
// We store values in this struct to simplify the integration of alternative implementations
// of the shading terms, outlined in the Readme.MD Appendix.
struct PBRInfo
{
    float NdotL;                  // cos angle between normal and light direction
    float NdotV;                  // cos angle between normal and view direction
    float NdotH;                  // cos angle between normal and half vector
    float LdotH;                  // cos angle between light direction and half vector
    float VdotH;                  // cos angle between view direction and half vector
    float perceptualRoughness;    // roughness value, as authored by the model creator (input to shader)
    float metalness;              // metallic value at the surface
    vec3 reflectance0;            // full reflectance color (normal incidence angle)
    vec3 reflectance90;           // reflectance color at grazing angle
    float alphaRoughness;         // roughness mapped to a more linear change in the roughness (proposed by [2])
    vec3 diffuseColor;            // color contribution from diffuse lighting
    vec3 specularColor;           // color contribution from specular lighting
};

// Calculation of the lighting contribution from an optional Image Based Light source.
// Precomputed Environment Maps are required uniform inputs and are computed as outlined in [1].
// See our README.md on Environment Maps [3] for additional discussion.
vec3 getIBLContribution(PBRInfo pbrInputs, vec3 n, vec3 reflection)
{
    float lod = (pbrInputs.perceptualRoughness * ubo.prefilteredCubeMipLevels);
    // retrieve a scale and bias to F0. See [1], Figure 3
    vec3 brdf = (texture(samplerBRDFLUT, vec2(pbrInputs.NdotV, 1.0 - pbrInputs.perceptualRoughness))).rgb;
    vec3 diffuseLight = texture(samplerIrradiance, reflection).rgb;

    vec3 specularLight = textureLod(prefilteredMap, reflection, lod).rgb;

    vec3 diffuse = diffuseLight * pbrInputs.diffuseColor;
    vec3 specular = specularLight * (pbrInputs.specularColor * brdf.x + brdf.y);

    // For presentation, this allows us to disable IBL terms
    // For presentation, this allows us to disable IBL terms
    diffuse *= ubo.scaleIBLAmbient;
    specular *= ubo.scaleIBLAmbient;

    return diffuse + specular;
}

// Basic Lambertian diffuse
// Implementation from Lambert's Photometria https://archive.org/details/lambertsphotome00lambgoog
// See also [1], Equation 1
vec3 diffuse(PBRInfo pbrInputs)
{
    return pbrInputs.diffuseColor / M_PI;
}

// The following equation models the Fresnel reflectance term of the spec equation (aka F())
// Implementation of fresnel from [4], Equation 15
vec3 specularReflection(PBRInfo pbrInputs)
{
    return pbrInputs.reflectance0 + (pbrInputs.reflectance90 - pbrInputs.reflectance0) * pow(clamp(1.0 - pbrInputs.VdotH, 0.0, 1.0), 5.0);
}

// This calculates the specular geometric attenuation (aka G()),
// where rougher material will reflect less light back to the viewer.
// This implementation is based on [1] Equation 4, and we adopt their modifications to
// alphaRoughness as input as originally proposed in [2].
float geometricOcclusion(PBRInfo pbrInputs)
{
    float NdotL = pbrInputs.NdotL;
    float NdotV = pbrInputs.NdotV;
    float r = pbrInputs.alphaRoughness;

    float attenuationL = 2.0 * NdotL / (NdotL + sqrt(r * r + (1.0 - r * r) * (NdotL * NdotL)));
    float attenuationV = 2.0 * NdotV / (NdotV + sqrt(r * r + (1.0 - r * r) * (NdotV * NdotV)));
    return attenuationL * attenuationV;
}

// The following equation(s) model the distribution of microfacet normals across the area being drawn (aka D())
// Implementation from "Average Irregularity Representation of a Roughened Surface for Ray Reflection" by T. S. Trowbridge, and K. P. Reitz
// Follows the distribution function recommended in the SIGGRAPH 2013 course notes from EPIC Games [1], Equation 3.
float microfacetDistribution(PBRInfo pbrInputs)
{
    float roughnessSq = pbrInputs.alphaRoughness * pbrInputs.alphaRoughness;
    float f = (pbrInputs.NdotH * roughnessSq - pbrInputs.NdotH) * pbrInputs.NdotH + 1.0;
    return roughnessSq / (M_PI * f * f);
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

float textureProj(vec4 P, float layer, vec2 offset)
{
    float shadow = 1.0;
    vec4 shadowCoord = P / P.w;
    shadowCoord.st = shadowCoord.st * 0.5 + 0.5;
    
    if (shadowCoord.z > -1.0 && shadowCoord.z < 1.0) 
    {
        float dist = texture(samplerShadowMap, vec3(shadowCoord.st + offset, layer)).r;
        if (shadowCoord.w > 0.0 && dist < shadowCoord.z)         
            shadow = SHADOW_FACTOR;
    }else
        shadow = 0.05f;//for debug view out of light proj
    
    return shadow;
}

float filterPCF(vec4 sc, float layer)
{
    ivec2 texDim = textureSize(samplerShadowMap, 0).xy;
    float scale = 1.5;
    float dx = scale * 1.0 / float(texDim.x);
    float dy = scale * 1.0 / float(texDim.y);

    float shadowFactor = 0.0;
    int count = 0;
    int range = 1;
    
    for (int x = -range; x <= range; x++)
    {
        for (int y = -range; y <= range; y++)
        {
            shadowFactor += textureProj(sc, layer, vec2(dx*x, dy*y));
            count++;
        }
    
    }
    return shadowFactor / count;
}

void main() 
{
    if (subpassLoad(samplerPos, gl_SampleID).a == 1.0f)
        discard;
    
    float perceptualRoughness = subpassLoad(samplerColorRough, gl_SampleID).a;
    float metallic = subpassLoad(samplerEmitMetal, gl_SampleID).a;
    vec3 diffuseColor;
    vec4 baseColor = vec4(subpassLoad(samplerColorRough, gl_SampleID).rgb, 1);    
    vec3 emissive = subpassLoad (samplerEmitMetal, gl_SampleID).rgb;        

    vec3 f0 = vec3(0.04);
    
    diffuseColor = baseColor.rgb * (vec3(1.0) - f0);
    diffuseColor *= 1.0 - metallic;
        
    float alphaRoughness = perceptualRoughness * perceptualRoughness;

    vec3 specularColor = mix(f0, baseColor.rgb, metallic);

    // Compute reflectance.
    float reflectance = max(max(specularColor.r, specularColor.g), specularColor.b);

    // For typical incident reflectance range (between 4% to 100%) set the grazing reflectance to 100% for typical fresnel effect.
    // For very low reflectance range on highly diffuse objects (below 4%), incrementally reduce grazing reflecance to 0%.
    float reflectance90 = clamp(reflectance * 25.0, 0.0, 1.0);
    vec3 specularEnvironmentR0 = specularColor.rgb;
    vec3 specularEnvironmentR90 = vec3(1.0) * reflectance90;

    vec3 n = subpassLoad(samplerN_AO, gl_SampleID).rgb;
    vec3 pos = subpassLoad(samplerPos, gl_SampleID).rgb;
    vec3 v = normalize(ubo.camPos.xyz - pos); // Vector from surface point to camera
    
    vec3 colors = vec3(0);
    vec3 lightTarget = vec3(0);
    
    for (int i=0; i<NUM_LIGHTS; i++) {
    
        vec3 l = normalize(lights[i].position.xyz-pos);     // Vector from surface point to light
        vec3 h = normalize(l+v);                        // Half vector between both l and v
        vec3 reflection = -normalize(reflect(v, n));
        reflection.y *= -1.0f;

        float NdotL = clamp(dot(n, l), 0.001, 1.0);
        float NdotV = clamp(abs(dot(n, v)), 0.001, 1.0);
        float NdotH = clamp(dot(n, h), 0.0, 1.0);
        float LdotH = clamp(dot(l, h), 0.0, 1.0);
        float VdotH = clamp(dot(v, h), 0.0, 1.0);

        PBRInfo pbrInputs = PBRInfo(
            NdotL,
            NdotV,
            NdotH,
            LdotH,
            VdotH,
            perceptualRoughness,
            metallic,
            specularEnvironmentR0,
            specularEnvironmentR90,
            alphaRoughness,
            diffuseColor,
            specularColor
        );

        // Calculate the shading terms for the microfacet specular shading model
        vec3 F = specularReflection(pbrInputs);
        float G = geometricOcclusion(pbrInputs);
        float D = microfacetDistribution(pbrInputs);

        // Calculation of analytical lighting contribution
        vec3 diffuseContrib = (1.0 - F) * diffuse(pbrInputs);
        vec3 specContrib = F * G * D / (4.0 * NdotL * NdotV);        
        // Obtain final intensity as reflectance (BRDF) scaled by the energy of the light (cosine law)
        vec4 shadowClip = lights[i].mvp * vec4(pos, 1);
        float shadowFactor = filterPCF(shadowClip, i);

        
        vec3 color = NdotL * lights[i].color.rgb * (diffuseContrib + specContrib);

        // Calculate lighting contribution from image based lighting source (IBL)
        colors += shadowFactor * (color + getIBLContribution(pbrInputs, n, reflection));
        
        
    }
    colors /= NUM_LIGHTS;
    
    
    const float u_OcclusionStrength = 1.0f;
    const float u_EmissiveFactor = 1.0f;
    
    //AO is in the alpha channel of the normalAttachment    
    colors = mix(colors, colors * subpassLoad(samplerN_AO, gl_SampleID).a, u_OcclusionStrength);
    colors += emissive;             
    
    outColor = vec4(colors, baseColor.a);       
}