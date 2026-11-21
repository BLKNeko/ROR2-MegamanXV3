// Raio estilo anime para Trails de partículas (ou LineRenderer).
// UV.x = ao longo do raio | UV.y = através. Miolo branco duro + brilho colorido nas bordas.
// A cor/alpha de vértice (cor da partícula / Color over Trail) multiplica tudo.
Shader "MegamanX/AnimeLightning"
{
    Properties
    {
        [HDR] _CoreColor ("Core Color", Color) = (1, 1, 1, 1)
        [HDR] _GlowColor ("Glow Color", Color) = (0.3, 0.85, 1, 1)
        _Intensity ("Intensity", Float) = 3
        _CoreWidth ("Core Width", Range(0, 1)) = 0.35
        _GlowFalloff ("Glow Falloff", Range(0.5, 6)) = 2
        _Solidity ("Solidity (0 = aditivo puro)", Range(0, 1)) = 0.1
        _FlickerSpeed ("Flicker Speed", Float) = 45
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

            fixed4 _CoreColor, _GlowColor;
            float _Intensity, _CoreWidth, _GlowFalloff, _Solidity, _FlickerSpeed;

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
                float across = 1.0 - abs(i.uv.y * 2.0 - 1.0);           // 1 no centro, 0 nas bordas
                float core = smoothstep(1.0 - _CoreWidth, 1.0 - _CoreWidth + 0.08, across);
                float glow = pow(saturate(across), _GlowFalloff);

                // pisca rápido, como eletricidade
                float flicker = 0.75 + 0.25 * sin(_Time.y * _FlickerSpeed + i.uv.x * 17.0);

                float3 col = lerp(_GlowColor.rgb * i.color.rgb, _CoreColor.rgb, core) * _Intensity * flicker;
                float a = saturate(max(core, glow * 0.7)) * i.color.a;
                return fixed4(col * a, a * _Solidity);
            }
            ENDCG
        }
    }
    FallBack Off
}
