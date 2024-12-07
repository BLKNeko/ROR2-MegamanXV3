Shader "Orange/Cloud" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_SpeedX ("Move Speed X", Range(-1, 1)) = 0.1
		_SpeedY ("Move Speed Y", Range(-1, 1)) = 0.1
		_Noise ("Noise", 2D) = "white" {}
		_distortFactorTime ("FactorTime", Range(0, 10)) = 0.5
		_distortFactor ("factor", Range(0, 1)) = 0.03
	}
	SubShader {
		LOD 100
		Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			LOD 100
			Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 33869
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float4 position : SV_POSITION0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _SpeedX;
			float _SpeedY;
			float _distortFactorTime;
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Color;
			float _distortFactor;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _Noise;
			sampler2D _MainTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.texcoord.xy = float2(_SpeedX.x, _SpeedY.x) * _Time.yy + tmp0.xy;
                o.texcoord1.xy = _distortFactorTime.xx * _Time.xy;
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                o.position = mul(unity_MatrixVP, tmp0);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0.xy = inp.texcoord1.xy + inp.texcoord.xy;
                tmp0 = tex2D(_Noise, tmp0.xy);
                tmp0.xy = tmp0.xy * _distortFactor.xx + inp.texcoord.xy;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                o.sv_target = tmp0 * _Color;
                return o;
			}
			ENDCG
		}
	}
}