Shader "Orange/UVScrollNoiseS" {
	Properties {
		_MainTex ("Base layer (RGB)", 2D) = "white" {}
		_NoiseTex ("Warp Noise", 2D) = "white" {}
		_ScrollX ("Base layer Scroll speed X", Float) = 1
		_ScrollY ("Base layer Scroll speed Y", Float) = 0
		_Intensity ("Intensity", Float) = 1
		_Alpha ("Alpha", Range(0, 1)) = 1
	}
	SubShader {
		LOD 150
		Tags { "RenderType" = "Opaque" }
		Pass {
			LOD 100
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Opaque" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			Fog {
				Mode Off
			}
			GpuProgramID 43960
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD2;
				float4 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			float _ScrollX;
			float _ScrollY;
			float _Intensity;
			float _Alpha;
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _NoiseTex;
			sampler2D _MainTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                tmp0.xy = -float2(_ScrollX.x, _ScrollY.x) * _Time.xy;
                tmp0.xy = tmp0.xy * float2(-0.5, -0.5);
                tmp0.xy = frac(tmp0.xy);
                tmp0.zw = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.texcoord2.xy = tmp0.xy + tmp0.zw;
                tmp0.xy = float2(_ScrollX.x, _ScrollY.x) * _Time.xy;
                tmp0.xy = frac(tmp0.xy);
                o.texcoord.xy = tmp0.xy + tmp0.zw;
                o.texcoord1 = float4(_Intensity.xxx, _Alpha.x);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = _Time * float4(6.0, 16.0, 19.0, 27.0);
                tmp0 = sin(tmp0);
                tmp0.x = tmp0.x + 1.0;
                tmp0.yzw = tmp0.yzw * float3(0.5, 0.5, 0.5) + float3(1.0, 1.0, 1.0);
                tmp0.x = tmp0.x * 0.5;
                tmp0.x = tmp0.y * tmp0.x;
                tmp0.x = tmp0.z * tmp0.x;
                tmp0.x = tmp0.w * tmp0.x;
                tmp0.y = -tmp0.x * 0.025 + 1.0;
                tmp1.x = tmp0.x * 0.05 + inp.texcoord.x;
                tmp1.z = -tmp0.x * 0.05 + inp.texcoord.x;
                tmp1.yw = inp.texcoord.yy;
                tmp2 = tex2D(_NoiseTex, inp.texcoord2.xy);
                tmp0.xz = tmp2.yz * float2(0.25, 0.25) + tmp1.xy;
                tmp1.xy = tmp2.yz * float2(0.25, 0.25) + tmp1.zw;
                tmp1.zw = tmp2.yz * float2(0.25, 0.25) + inp.texcoord.xy;
                tmp2 = tex2D(_MainTex, tmp1.zw);
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp2.z = tmp1.z;
                tmp1 = tex2D(_MainTex, tmp0.xz);
                tmp2.x = tmp1.x;
                tmp0 = tmp0.yyyy * tmp2;
                o.sv_target = tmp0 * inp.texcoord1;
                return o;
			}
			ENDCG
		}
	}
}