Shader "Custom/URP_TransparentShadow"
{
    Properties
    {
        _ShadowColor("Shadow Color", Color) = (0.2, 0.2, 0.2, 0.8)
    }

    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

    struct Attributes
    {
        float4 position : POSITION;
    };

    struct Varyings
    {
        float4 pos : SV_POSITION;
        float3 worldPos : TEXCOORD0;
        float4 shadowCoord : TEXCOORD1;
    };

    float4 _ShadowColor;

    Varyings Vert(Attributes v)
    {
        Varyings o;
        o.pos = TransformObjectToHClip(v.position.xyz);
        o.worldPos = TransformObjectToWorld(v.position.xyz);
        
        // Calculate main light shadow coordinates for URP
        #if defined(_MAIN_LIGHT_SHADOWS)
        o.shadowCoord = TransformWorldToShadowCoord(o.worldPos);
        #else
        o.shadowCoord = float4(0, 0, 0, 0);
        #endif
        
        return o;
    }

    half4 Frag(Varyings i) : SV_Target
    {
        // Sample shadow strength in URP, only if main light shadows are enabled
        half shadow = 1.0;
        #if defined(_MAIN_LIGHT_SHADOWS)
        shadow = MainLightRealtimeShadow(i.shadowCoord);
        #endif
        
        half atten = 1.0 - shadow; // Apply shadow strength inversion for shadow effect

        return half4(_ShadowColor.rgb, _ShadowColor.a * atten);
    }

    ENDHLSL

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        
        Pass
        {
            Name "ShadowReceiver"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            ENDHLSL
        }
    }

    Fallback Off
}
