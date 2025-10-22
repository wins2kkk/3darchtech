Shader "Custom/SpriteGradientFlexibleWithVector" {
Properties {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Start Color", Color) = (1, 1, 1, 1)  // Màu bắt đầu
    _Color2 ("End Color", Color) = (1, 1, 1, 1)   // Màu kết thúc
    _Scale ("Scale", Float) = 1.0                 // Hệ số nhân gradient
    _Direction ("Gradient Direction (X,Y)", Vector) = (1, 0, 0, 0) // Hướng gradient

    // Các thuộc tính Stencil cần thiết để tương thích với UI
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _ColorMask ("Color Mask", Float) = 15
}

SubShader {
    Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
    LOD 100

    Blend SrcAlpha OneMinusSrcAlpha // Hỗ trợ alpha blending
    Cull Off
    ZWrite Off

    Pass {
        CGPROGRAM
        #pragma vertex vert  
        #pragma fragment frag
        #include "UnityCG.cginc"

        // Các thuộc tính shader
        sampler2D _MainTex;
        float4 _Color;      // Màu bắt đầu
        float4 _Color2;     // Màu kết thúc
        float _Scale;       // Hệ số nhân gradient
        float4 _Direction;  // Hướng gradient (vector 4D)

        struct v2f {
            float4 pos : SV_POSITION; // Vị trí trong clip space
            float2 uv : TEXCOORD0;    // UV của texture
            fixed4 col : COLOR;       // Màu gradient
        };

        v2f vert (appdata_full v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex); // Chuyển vertex sang clip space
            o.uv = v.texcoord;                      // Lấy UV
            
            // Tính toán giá trị gradient dựa trên hướng (_Direction.xy)
            float gradientFactor = dot(v.texcoord, normalize(_Direction.xy));
            o.col = lerp(_Color, _Color2, saturate(gradientFactor * _Scale));
            return o;
        }

        float4 frag (v2f i) : SV_Target {
            // Lấy màu từ texture
            float4 texColor = tex2D(_MainTex, i.uv);
            // Nhân màu texture với gradient
            return texColor * i.col;
        }
        ENDCG
    }
}
}
