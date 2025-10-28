#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Matrices
matrix WorldViewProjection;

// Global color
float4 GlobalColor;

// Texture
texture texMapLine;
sampler LinearSampler = sampler_state
{
    Texture = <texMapLine>;
    MinFilter = Linear;
    MagFilter = Point;
    MipFilter = None;
    AddressU = Wrap;
    AddressV = Wrap;
};

// Vertex input
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float Visibility : TEXCOORD1;
};

// Vertex output
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float4 Color : COLOR0;
};

// Vertex Shader
VertexShaderOutput MainVS(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    // Transform to clip space
    output.Position = mul(input.Position, WorldViewProjection);

    // Visibility control (adjusted)
    float vis = saturate(input.Visibility * 3 - 1);

    // Color modulation
    output.Color = GlobalColor * vis * float4(0.65f, 0.65f, 0.65f, 0.5f);
    output.TexCoord = input.TexCoord;

    return output;
}

// Pixel Shader
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 textureColor = 1 - tex2D(LinearSampler, input.TexCoord);
    return input.Color * textureColor;
}

// Technique
technique AmbientTexturedTrail
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
