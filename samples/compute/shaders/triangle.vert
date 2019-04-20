#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (location = 0) in vec2 inPos;

layout(push_constant) uniform PushConsts {
    int imgDim;
    int xPad;
    int yPad;
    int zoom;
};

out gl_PerVertex 
{
    vec4 gl_Position;   
};


void main() 
{	
	gl_Position = vec4(inPos.xy * vec2(2) / vec2(imgDim) - vec2(1), 0.0, 1.0);
}
