#version 450

layout (binding = 2) uniform samplerCube samplerEnv;

layout (set = 0, location = 0) in vec3 inUVW;

layout (set = 0, location = 0) out vec4 outColor;

void main() 
{    
    outColor = vec4(textureLod(samplerEnv, inUVW, 1.5).rgb, 1.0);
}