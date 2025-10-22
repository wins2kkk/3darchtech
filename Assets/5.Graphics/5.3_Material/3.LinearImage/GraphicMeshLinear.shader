Shader "UI/RadialGradient"
{
    Properties
    {
        _ColorInner ("Inner Color", Color) = (1,1,1,1)
        _ColorOuter ("Outer Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
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

            float4 _ColorInner;
            float4 _ColorOuter;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * 2.0 - 1.0; // Center UV at (0,0)
                float dist = length(uv); // Calculate distance from center
                dist = saturate(dist);   // Clamp distance to [0,1]
                return lerp(_ColorInner, _ColorOuter, dist); // Linear gradient
            }
            ENDCG
        }
    }
}
