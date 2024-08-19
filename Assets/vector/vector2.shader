Shader "Custom/vector2"
{
    Properties
    {
        _MotionVectors ("Motion Vectors", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata_t
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

sampler2D _MotionVectors;

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

half4 frag(v2f i) : SV_Target
{
    float2 motion = tex2D(_MotionVectors, i.uv).xy;
    half4 color = half4(motion, 0.0, 1.0); // Map motion vector to color
    return color;
}
            ENDCG
        }
    }
FallBack"Diffuse"
}