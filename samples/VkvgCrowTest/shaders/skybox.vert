#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (location = 0) in vec3 inPos;

layout (binding = 0) uniform UBO 
{
    mat4 projection;
    mat4 model;
    mat4 view;
} ubo;

layout (location = 0) out vec3 outUVW;

out gl_PerVertex 
{
    vec4 gl_Position;
};

void main() 
{
    outUVW = inPos;
    outUVW.y = -outUVW.y;        
    gl_Position = ubo.projection * mat4(mat3(ubo.view)) * vec4(inPos, 1.0);    
}
