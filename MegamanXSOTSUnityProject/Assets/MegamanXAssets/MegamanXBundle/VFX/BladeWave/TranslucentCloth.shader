// Tecido translúcido (echarpe da skin Command Mission).
// Usa o alpha da textura, os dois lados visíveis, luz direcional simples + luz ambiente.
// Sem multi_compile: nada para o build remover, funciona igual no RoR2.
Shader "MegamanX/TranslucentCloth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _Opacity ("Opacity", Range(0, 1)) = 0.85
        _AlphaBoost ("Alpha Boost", Range(0.5, 3)) = 1
        _Wrap ("Light Wrap (suaviza a sombra)", Range(0, 1)) = 0.5
        _AmbientStrength ("Ambient Strength", Range(0, 2)) = 1
        _Emission ("Emission", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Opacity, _AlphaBoost, _Wrap, _AmbientStrength, _Emission;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 ambient : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.ambient = ShadeSH9(float4(o.worldNormal, 1));
                return o;
            }

            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;

                // Cull Off: o lado de trás usa a normal invertida
                float3 n = normalize(i.worldNormal) * (facing > 0 ? 1 : -1);
                float ndl = dot(n, normalize(_WorldSpaceLightPos0.xyz));
                float diffuse = saturate((ndl + _Wrap) / (1 + _Wrap));

                float3 light = _LightColor0.rgb * diffuse + i.ambient * _AmbientStrength + _Emission;
                float alpha = saturate(tex.a * _AlphaBoost) * _Opacity;
                return fixed4(tex.rgb * light, alpha);
            }
            ENDCG
        }
    }
    FallBack Off
}
