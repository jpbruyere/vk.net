#version 450

layout (location = 0) in vec3 inPos;

layout(push_constant) uniform PushConsts {
    mat4 model;
} pc;

void main()
{
	gl_Position = pc.model * vec4(inPos,1);
}