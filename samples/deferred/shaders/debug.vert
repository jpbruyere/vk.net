#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inColor;

layout (location = 0) out vec3 outColor;

layout (binding = 0) uniform UBO 
{
    mat4 projectionMatrix;
    mat4 viewMatrix;
    mat4 modelMatrix;    
    float gamma;
    float exposure;    
} ubo;

layout(push_constant) uniform PushConsts {
    mat4 projectionMatrix;
} pc;

out gl_PerVertex 
{
    vec4 gl_Position;   
};

void main() 
{
    outColor = inColor;
    
	gl_Position = pc.projectionMatrix * vec4 ((ubo.viewMatrix * vec4(inPos.xyz, 0.0)).xyz, 1);
}
