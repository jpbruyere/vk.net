#version 420

#define NUM_LIGHTS 2

layout (triangles, invocations = NUM_LIGHTS) in;
layout (triangle_strip, max_vertices = 3) out;

struct Light {
    vec4 position;
    vec4 color;
    mat4 mvp;
};
layout (set = 0, binding = 0) uniform UBO {
    mat4 projection;
    mat4 model;
    mat4 view;    
};
layout (set = 0, binding = 1) uniform UBOLights {
    Light lights[NUM_LIGHTS];
};

//layout (location = 0) in int inInstanceIndex[];

layout(push_constant) uniform PushConsts {
    mat4 model;
} pc;

void main() 
{
	for (int i = 0; i < gl_in.length(); i++)
	{
		gl_Layer = gl_InvocationID;        
		gl_Position = lights[gl_InvocationID].mvp * model * pc.model * gl_in[i].gl_Position;        
		EmitVertex();
	}
	EndPrimitive();
}