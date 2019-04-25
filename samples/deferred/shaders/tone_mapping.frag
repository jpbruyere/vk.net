#version 450
#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (set = 0, binding = 0) uniform UBO {
    mat4 projection;
    mat4 model;
    mat4 view;
    vec4 camPos;
    vec4 lightDir;
    float exposure;
    float gamma;    
} ubo;


layout (input_attachment_index = 0, set = 2, binding = 4) uniform subpassInput samplerHDR;

layout (location = 0) in vec2 inUV;
layout (location = 0) out vec4 outColor;

vec3 Uncharted2Tonemap(vec3 color)
{
    float A = 0.15;
    float B = 0.50;
    float C = 0.10;
    float D = 0.20;
    float E = 0.02;
    float F = 0.30;
    float W = 11.2;
    return ((color*(A*color+C*B)+D*E)/(color*(A*color+B)+D*F))-E/F;
}

vec3 tonemap(vec3 color)
{
    vec3 outcol = Uncharted2Tonemap(color.rgb * ubo.exposure);
    outcol = outcol * (1.0f / Uncharted2Tonemap(vec3(11.2f)));  
    return pow(outcol, vec3(1.0f / ubo.gamma));
}

vec3 SRGBtoLINEAR(vec3 srgbIn)
{
    #ifdef MANUAL_SRGB
    #ifdef SRGB_FAST_APPROXIMATION
    return pow(srgbIn.xyz,vec3(2.2));
    #else //SRGB_FAST_APPROXIMATION
    vec3 bLess = step(vec3(0.04045),srgbIn.xyz);
    return linOut = mix( srgbIn.xyz/vec3(12.92), pow((srgbIn.xyz+vec3(0.055))/vec3(1.055),vec3(2.4)), bLess );
    #endif //SRGB_FAST_APPROXIMATION    
    #else //MANUAL_SRGB
    return srgbIn;
    #endif //MANUAL_SRGB
}

void main() 
{    
    vec4 color = subpassLoad(samplerHDR);    
    outColor = vec4(SRGBtoLINEAR(tonemap(color.rgb)), color.a);

}