#version 450

#extension GL_ARB_separate_shader_objects : enable
#extension GL_ARB_shading_language_420pack : enable

layout (set = 1, binding = 0) uniform sampler2D samplerColor;
layout (set = 1, binding = 1) uniform sampler2D samplerNormal;
layout (set = 1, binding = 2) uniform sampler2D samplerOcclusion;

layout (location = 0) in vec2 inUV;
layout (location = 1) in vec3 inN;
layout (location = 2) in vec3 inV;//ViewDir

layout (location = 0) out vec4 outFragColor;

vec3 light = vec3(1.0,.0,1.0);

// http://www.thetenthplanet.de/archives/1180
mat3 cotangent_frame(vec3 N, vec3 p, vec2 uv)
{
    // get edge vectors of the pixel triangle
    vec3 dp1 = dFdx( p );
    vec3 dp2 = dFdy( p );
    vec2 duv1 = dFdx( uv );
    vec2 duv2 = dFdy( uv );
 
    // solve the linear system
    vec3 dp2perp = cross( dp2, N );
    vec3 dp1perp = cross( N, dp1 );
    vec3 T = dp2perp * duv1.x + dp1perp * duv2.x;
    vec3 B = dp2perp * duv1.y + dp1perp * duv2.y;
 
    // construct a scale-invariant frame 
    float invmax = inversesqrt( max( dot(T,T), dot(B,B) ) );
    return mat3( T * invmax, B * invmax, N );
}

vec3 perturb_normal( vec3 N, vec3 V, vec2 texcoord )
{
    // assume N, the interpolated vertex normal and 
    // V, the view vector (vertex to eye)
    vec3 map = texture(samplerNormal, texcoord).xyz;
    map = map * 255./127. - 128./127.;
    mat3 TBN = cotangent_frame(N, -V, texcoord);
    return normalize(TBN * map);
}



void main() 
{    
    vec4 diff = texture(samplerColor, inUV);
    
    vec3 n = normalize(inN);
    vec3 l = normalize(-light);
    vec3 pn = perturb_normal(n, inV, inUV);    
    
    float lambert = max(0.0, dot(pn, l));
    
    //diff.rgb *= lambert * texture(samplerOcclusion, inUV).rgb;
    vec3 spec = vec3(0);
    vec3 amb = vec3(0.1);
    if (lambert >= 0.0) {
       vec3 rd = reflect(l, pn);
       float s = dot(rd, normalize(inV));
       spec = vec3(0.9,0.9,0.9)* //light.specular.xyz * material.specular.xyz *
             pow(max(0.0, s), 10.5); //0.5 = mat shininess
    }
    //outFragColor = vec4(1.0);  // 
    //outFragColor = vec4(diff.rgb , 1.0);  // 
    outFragColor = vec4(diff.rgb + amb + spec , diff.a);  // 
}