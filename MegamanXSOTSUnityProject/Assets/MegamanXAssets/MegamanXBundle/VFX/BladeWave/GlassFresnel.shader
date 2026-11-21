// Vidro simples para o visor/cúpula da Ultimate Armor CM.
// Transparente com borda (fresnel) mais forte e um brilho especular falso; não depende de luz da cena,
// então funciona igual em qualquer pipeline/fase.
Shader "MegamanX/GlassFresnel"
{
    Properties
    {
        _Color ("Tint (alpha = quanto o vidro cobre o fundo)", Color) = (0.92, 0.96, 1, 0.06)
        [HDR] _RimColor ("Rim Color", Color) = (0.75, 0.85, 0.95, 1)
        _RimPower ("Rim Power", Range(0.5, 8)) = 4
        _RimAlpha ("Rim Alpha", Range(0, 1)) = 0.3
        [HDR] _SpecColor2 ("Highlight Color", Color) = (1, 1, 1, 1)
        _SpecPower ("Highlight Sharpness", Range(4, 256)) = 64
        _SpecDir ("Highlight Direction (view space)", Vector) = (-0.4, 0.6, 0.7, 0)
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        Blend One OneMinusSrcAlpha // pré-multiplicado: o tint quase não pinta, borda e brilho somam luz
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color, _RimColor, _SpecColor2;
            float _RimPower, _RimAlpha, _SpecPower;
            float4 _SpecDir;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 viewNormal : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 n = normalize(i.viewNormal);
                float facing = saturate(n.z);                 // 1 = virado para a câmera
                float rim = pow(1.0 - facing, _RimPower);
                float spec = pow(saturate(dot(n, normalize(_SpecDir.xyz))), _SpecPower);

                float rimA = rim * _RimAlpha;
                float3 col = _Color.rgb * _Color.a + _RimColor.rgb * rimA + _SpecColor2.rgb * spec;
                float a = saturate(_Color.a + rimA);
                return fixed4(col, a);
            }
            ENDCG
        }
    }
    FallBack Off
}
