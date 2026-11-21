// Faísca/estilhaço estilo anime: losango de borda dura com miolo branco, sem textura.
// Com Stretched Billboard vira uma "speed line" fina.
Shader "MegamanX/AnimeSpark"
{
    Properties
    {
        [HDR] _Color ("Color", Color) = (0.4, 0.9, 1, 1)
        [HDR] _CoreColor ("Core Color", Color) = (1, 1, 1, 1)
        _Intensity ("Intensity", Float) = 2.5
        _CoreSize ("Core Size", Range(0, 1)) = 0.45
        _EdgeSharpness ("Edge Softness", Range(0.001, 0.5)) = 0.06
        _Solidity ("Solidity (0 = aditivo puro)", Range(0, 1)) = 0.2
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "PreviewType" = "Plane" }
        Blend One OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color, _CoreColor;
            float _Intensity, _CoreSize, _EdgeSharpness, _Solidity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 d = abs(i.uv - 0.5) * 2.0;
                float diamond = 1.0 - (d.x + d.y);

                float shape = smoothstep(0.0, _EdgeSharpness, diamond);
                float core = smoothstep(_CoreSize, _CoreSize + _EdgeSharpness, diamond);

                float3 col = lerp(_Color.rgb * i.color.rgb, _CoreColor.rgb, core) * _Intensity;
                float a = shape * i.color.a;
                return fixed4(col * a, a * _Solidity);
            }
            ENDCG
        }
    }
    FallBack Off
}
