#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

float4 GlobalColor;

// Vertex input
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float Visibility : TEXCOORD0; // single float for fade
};

// Vertex output
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

// Vertex Shader
VertexShaderOutput MainVS(VertexShaderInput input)
{
    VertexShaderOutput output;

    // Transform to clip space
    output.Position = mul(input.Position, WorldViewProjection);

    // Visibility control
    float vis = saturate(input.Visibility * 3 - 1);

    // Output color with fade
    output.Color = GlobalColor * vis;

    return output;
}

// Pixel Shader
float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Just output the color from the vertex shader
    return input.Color;
}

// Technique
technique AmbientTrail
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
