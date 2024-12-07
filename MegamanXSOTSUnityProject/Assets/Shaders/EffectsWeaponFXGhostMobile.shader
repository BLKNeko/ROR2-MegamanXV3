Shader "Effects/WeaponFX/GhostMobile" {
	Properties {
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (1,1,1,0.5)
		_MainTex ("Main Texture", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Height ("_Height", Float) = 0.2
		_Speed ("Speed (x, y)", Vector) = (1,0,0,0)
		_FPOW ("FPOW Fresnel", Float) = 5
		_R0 ("R0 Fresnel", Float) = 0.05
		_BumpAmt ("Distortion", Float) = 1
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 24154
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 sv_position : SV_Position0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 color : COLOR0;
				float3 texcoord4 : TEXCOORD4;
				float3 texcoord5 : TEXCOORD5;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _Speed;
			float4 _BumpMap_ST;
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _BumpAmt;
			float4 _GrabTextureMobile_TexelSize;
			float4 _TintColor;
			float _FPOW;
			float _R0;
			float _DistortFixScale;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _GrabTextureMobile;
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
                o.sv_position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                tmp1.xy = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                o.texcoord1.xy = _Time.xx * _Speed.xy + tmp1.xy;
                o.texcoord2.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp1.xyz = tmp0.yyy * unity_MatrixVP._m01_m11_m31;
                tmp1.xyz = unity_MatrixVP._m00_m10_m30 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_MatrixVP._m02_m12_m32 * tmp0.zzz + tmp1.xyz;
                tmp0.xyz = unity_MatrixVP._m03_m13_m33 * tmp0.www + tmp0.xyz;
                tmp0.w = tmp0.y * _ProjectionParams.x;
                tmp0.xy = tmp0.zz + tmp0.xw;
                o.texcoord3.zw = tmp0.zz;
                o.texcoord3.xy = tmp0.xy * float2(0.5, 0.5);
                o.color = v.color;
                o.texcoord4.xyz = v.normal.xyz;
                tmp0.xyz = mul(unity_WorldToObject, _WorldSpaceCameraPos);
                tmp0.xyz = tmp0.xyz - v.vertex.xyz;
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                o.texcoord5.xyz = tmp0.www * tmp0.xyz;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_BumpMap, inp.texcoord1.xy);
                tmp0.x = tmp0.w * tmp0.x;
                tmp0.xy = tmp0.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.xy = tmp0.xy * _BumpAmt.xx;
                tmp0.xy = tmp0.xy * _GrabTextureMobile_TexelSize.xy;
                tmp0.zw = tmp0.xy * _DistortFixScale.xx + inp.texcoord2.xy;
                tmp0.xy = tmp0.xy * _DistortFixScale.xx;
                tmp0.xy = tmp0.xy * inp.texcoord3.zz + inp.texcoord3.xy;
                tmp0.xy = tmp0.xy / inp.texcoord3.ww;
                tmp1 = tex2D(_GrabTextureMobile, tmp0.xy);
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp0.x = dot(tmp0.xyz, float3(0.299, 0.587, 0.114));
                tmp0.xyz = tmp1.xyz * _TintColor.xyz + tmp0.xxx;
                tmp0.xyz = tmp0.xyz - tmp1.xyz;
                tmp0.w = dot(inp.texcoord4.xyz, inp.texcoord5.xyz);
                tmp0.w = saturate(1.0 - tmp0.w);
                tmp0.w = log(tmp0.w);
                tmp0.w = tmp0.w * _FPOW;
                tmp0.w = exp(tmp0.w);
                tmp1.w = 1.0 - _R0;
                tmp0.w = saturate(tmp1.w * tmp0.w + _R0);
                tmp0.w = 1.0 - tmp0.w;
                tmp1.w = tmp0.w * tmp0.w + tmp0.w;
                tmp0.w = tmp0.w * tmp1.w;
                tmp1.w = tmp0.w + tmp0.w;
                o.sv_target.w = saturate(tmp0.w * _TintColor.w);
                o.sv_target.xyz = tmp1.www * tmp0.xyz + tmp1.xyz;
                return o;
			}
			ENDCG
		}
	}
}