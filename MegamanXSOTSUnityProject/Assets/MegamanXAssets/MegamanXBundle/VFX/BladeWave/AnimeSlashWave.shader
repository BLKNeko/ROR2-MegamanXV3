// Onda de corte estilo anime moderno (cel shading em faixas duras + fios que se desfazem na traseira).
// UV.x = ao longo do arco (0..1) | UV.y = através da lâmina.
// Blend One OneMinusSrcAlpha: a cor soma como aditivo e o alpha (Solidity) escurece um pouco o fundo,
// então o corte continua legível mesmo em fases muito claras.
// A cor de vértice (Color over Lifetime das partículas) multiplica cor e alpha.
Shader "MegamanX/AnimeSlashWave"
{
    Properties
    {
        [HDR] _CoreColor ("Core Color", Color) = (1, 1, 1, 1)
        [HDR] _MainColor ("Main Color", Color) = (0.3, 0.9, 1, 1)
        [HDR] _EdgeColor ("Edge Color", Color) = (0.35, 0.2, 1, 1)
        _Intensity ("Intensity", Float) = 2
        _Solidity ("Solidity (0 = aditivo puro)", Range(0, 1)) = 0.35

        _CenterProfile ("Perfil: 0 = borda da frente, 1 = centro", Range(0, 1)) = 0
        _CoreWidth ("Core Width", Range(0, 0.5)) = 0.1
        _MainWidth ("Main Width", Range(0, 1)) = 0.45
        _EdgeWidth ("Edge Width", Range(0, 1)) = 0.85
        _BandSoftness ("Band Softness", Range(0.001, 0.2)) = 0.02
        _EdgeAlpha ("Edge Band Alpha", Range(0, 1)) = 0.6
        _TipFade ("Tip Fade", Range(0.01, 0.5)) = 0.1

        _StrandDensity ("Strand Density", Float) = 14
        _StrandSpeed ("Strand Speed", Float) = 3
        _StrandCut ("Strand Cut", Range(0, 1)) = 0.55

        _FlickerSpeed ("Flicker Speed", Float) = 30
        _FlickerAmount ("Flicker Amount", Range(0, 1)) = 0.12
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

            fixed4 _CoreColor, _MainColor, _EdgeColor;
            float _Intensity, _Solidity, _CenterProfile;
            float _CoreWidth, _MainWidth, _EdgeWidth, _BandSoftness, _EdgeAlpha, _TipFade;
            float _StrandDensity, _StrandSpeed, _StrandCut;
            float _FlickerSpeed, _FlickerAmount;

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

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f);
                float a = hash21(i);
                float b = hash21(i + float2(1, 0));
                float c = hash21(i + float2(0, 1));
                float d = hash21(i + float2(1, 1));
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

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
                float u = i.uv.x;
                float v = i.uv.y;
                float t = _Time.y;

                // 1 = parte mais brilhante (borda da frente ou centro da lâmina)
                float p = lerp(v, 1.0 - abs(v * 2.0 - 1.0), _CenterProfile);

                // Pontas somem suavemente
                float tip = smoothstep(0.0, _TipFade, u) * smoothstep(0.0, _TipFade, 1.0 - u);

                // Fios paralelos ao arco que se desfazem na traseira da onda
                float n = valueNoise(float2(u * 3.0 - t * _StrandSpeed, p * _StrandDensity));
                float trailing = 1.0 - p;
                float strands = smoothstep(_StrandCut - 0.05, _StrandCut + 0.05, n + (1.0 - trailing) * 0.6);

                // Faixas de cor duras (cel shading)
                float s = _BandSoftness;
                float core = smoothstep(1.0 - _CoreWidth - s, 1.0 - _CoreWidth, p);
                float mainBand = smoothstep(1.0 - _MainWidth - s, 1.0 - _MainWidth, p);
                float edge = smoothstep(1.0 - _EdgeWidth - s, 1.0 - _EdgeWidth, p);

                float3 col = _EdgeColor.rgb;
                col = lerp(col, _MainColor.rgb, mainBand);
                col = lerp(col, _CoreColor.rgb, core);

                float a = edge * strands * tip;
                a *= lerp(_EdgeAlpha, 1.0, mainBand);

                float flicker = 1.0 - _FlickerAmount * (0.5 + 0.5 * sin(t * _FlickerSpeed + u * 20.0));
                a *= flicker * i.color.a;

                col *= i.color.rgb * _Intensity;
                return fixed4(col * a, a * _Solidity);
            }
            ENDCG
        }
    }
    FallBack Off
}
