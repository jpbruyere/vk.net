#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (location = 0) in vec3 aPos;

layout (location = 0) out vec3 TexCoords;

layout (binding = 0) uniform UBO 
{
    mat4 projection;
    mat4 view;    
} ubo;

out gl_PerVertex 
{
    vec4 gl_Position;   
};

void main()
{
    TexCoords = aPos;
    gl_Position = ubo.projection * ubo.view * vec4(aPos, 1.0);
}
