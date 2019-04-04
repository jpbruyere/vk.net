#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec2 inUV;

layout (binding = 0) uniform UBO 
{
	mat4 projectionMatrix;
    mat4 viewMatrix;
	mat4 modelMatrix;
    vec4 lightPos;
    float gamma;
    float exposure;    
} ubo;

layout (location = 0) out vec2 outUV;
layout (location = 1) out vec3 outN;
layout (location = 2) out vec3 outV;//ViewDir
layout (location = 3) out vec3 outWorldPos;

out gl_PerVertex 
{
    vec4 gl_Position;   
};

layout(push_constant) uniform PushConsts {
    mat4 model;
} pc;

void main() 
{
    outUV = inUV;
    
    mat4 mod = ubo.modelMatrix * pc.model;
    vec4 pos = mod * vec4(inPos.xyz, 1.0);
    
    //outN = normalize(transpose(inverse(mat3(mod))) * inNormal);    
    outN = normalize(mat3(mod)* inNormal);    
    
    mat4 viewInv = inverse(ubo.viewMatrix);
    outV = -(ubo.viewMatrix * pos).xyz;//normalize(vec3(viewInv * vec4(0.0, 0.0, 0.0, 1.0) - pos));
    outWorldPos = pos.xyz;   
	gl_Position = ubo.projectionMatrix * ubo.viewMatrix * pos;
}
