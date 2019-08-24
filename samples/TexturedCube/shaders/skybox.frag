#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (binding = 1) uniform samplerCube skybox;

layout (location = 0) in vec3 TexCoords;

layout (location = 0) out vec4 outFragColor;

void main() 
{
    outFragColor = textureLod(skybox, TexCoords,0);
}
