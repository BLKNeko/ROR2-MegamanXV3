Shader "Effects/WeaponFX/WaterBlend" {
	Properties {
		_TintColor ("Main Color", Vector) = (1,1,1,1)
		_RimColor ("Rim Color", Vector) = (1,1,1,0.5)
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_PerlinNoise ("Perlin Noise Map (r)", 2D) = "white" {}
		_DropWavesScale ("Waves Scale (X) Height (YZ) Time (W)", Vector) = (1,1,1,1)
		_NoiseScale ("Noize Scale (XYZ) Height (W)", Vector) = (1,1,1,0.2)
		_Speed ("Distort Direction Speed (XY)", Vector) = (1,0,0,0)
		_FPOW ("FPOW Fresnel", Float) = 5
		_R0 ("R0 Fresnel", Float) = 0.05
		_BumpAmt ("Distortion Scale", Float) = 10
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass {
			"_GrabTexture"
		}
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 54881
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 sv_position : SV_Position0;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
				float4 color : COLOR0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _Speed;
			float4 _DropWavesScale;
			float4 _NoiseScale;
			float4 _BumpMap_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _GrabTexture_TexelSize;
			float4 _TintColor;
			float4 _RimColor;
			float4 _LightColor0;
			float _BumpAmt;
			float _FPOW;
			float _R0;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			sampler2D _PerlinNoise;
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _GrabTexture;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = _DropWavesScale.xy * _Time.xx;
                tmp0.xy = tmp0.xy * float2(2.0, 4.0);
                tmp1 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp1;
                tmp1 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp1;
                tmp0.zw = unity_ObjectToWorld._m03_m13 * v.vertex.ww + tmp1.xy;
                tmp1 = tmp1 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp0.xy = tmp0.zw * _NoiseScale.xy + tmp0.xy;
                tmp0.zw = tmp0.zw * _DropWavesScale.xx;
                tmp2 = tex2Dlod(_PerlinNoise, float4(tmp0.zw, 0, 0.0), 0);
                tmp0.z = tmp2.w * 2.0 + -0.5;
                tmp2 = tex2Dlod(_PerlinNoise, float4(tmp0.xy, 0, 0.0), 0);
                tmp0.xyw = tmp2.xyz * _NoiseScale.www;
                tmp2.x = _DropWavesScale.z * 0.005;
                tmp0.xyw = v.normal.xyz * tmp2.xxx + tmp0.xyw;
                tmp0.xyw = -_NoiseScale.www * float3(0.5, 0.5, 0.5) + tmp0.xyw;
                tmp2.xyz = v.normal.xyz * _DropWavesScale.yyy;
                tmp2.xyz = tmp0.zzz * tmp2.xyz;
                tmp2.xyz = tmp2.xyz * float3(0.01, 0.01, 0.01) + v.vertex.xyz;
                tmp0.xyz = tmp0.xyw + tmp2.xyz;
                tmp2 = mul(unity_ObjectToWorld, float4(tmp0.xyz, 1.0));
                tmp3 = tmp2.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp3 = unity_MatrixVP._m00_m10_m20_m30 * tmp2.xxxx + tmp3;
                tmp3 = unity_MatrixVP._m02_m12_m22_m32 * tmp2.zzzz + tmp3;
                tmp2 = unity_MatrixVP._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                o.sv_position = tmp2;
                tmp3.xy = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                tmp3.zw = _Speed.xy * _Time.xx;
                o.texcoord.xy = tmp3.zw * _DropWavesScale.xy + tmp3.xy;
                tmp3.xyz = tmp1.yyy * unity_MatrixVP._m01_m11_m31;
                tmp3.xyz = unity_MatrixVP._m00_m10_m30 * tmp1.xxx + tmp3.xyz;
                tmp1.xyz = unity_MatrixVP._m02_m12_m32 * tmp1.zzz + tmp3.xyz;
                tmp1.xyz = unity_MatrixVP._m03_m13_m33 * tmp1.www + tmp1.xyz;
                tmp1.xyz = tmp2.xyw + tmp1.xyz;
                tmp1.w = -tmp1.y;
                tmp1.xy = tmp1.zz + tmp1.xw;
                o.texcoord1.zw = tmp1.zz;
                o.texcoord1.xy = tmp1.xy * float2(0.5, 0.5);
                o.texcoord2.xyz = v.normal.xyz;
                tmp1.xyz = mul(unity_WorldToObject, _WorldSpaceCameraPos);
                tmp0.xyz = tmp1.xyz - tmp0.xyz;
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                o.texcoord3.xyz = tmp0.www * tmp0.xyz;
                o.color = v.color;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = dot(inp.texcoord2.xyz, inp.texcoord3.xyz);
                tmp0.x = saturate(1.0 - tmp0.x);
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _FPOW;
                tmp0.x = exp(tmp0.x);
                tmp0.y = 1.0 - _R0;
                tmp0.x = saturate(tmp0.y * tmp0.x + _R0);
                tmp0.x = tmp0.x * tmp0.x + tmp0.x;
                tmp0.x = min(tmp0.x, 1.0);
                tmp0.xyz = tmp0.xxx * _RimColor.xyz;
                tmp0.xyz = tmp0.xyz * _LightColor0.xyz;
                tmp0.xyz = tmp0.xyz * _LightColor0.www;
                tmp0.xyz = tmp0.xyz + tmp0.xyz;
                tmp0.xyz = tmp0.xyz * inp.color.xyz;
                tmp1 = tex2D(_BumpMap, inp.texcoord.xy);
                tmp1.x = tmp1.w * tmp1.x;
                tmp1.xy = tmp1.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp1.xy = tmp1.xy * _BumpAmt.xx;
                tmp1.xy = tmp1.xy * _GrabTexture_TexelSize.xy;
                tmp1.xy = tmp1.xy * inp.color.ww;
                tmp1.xy = tmp1.xy * inp.texcoord1.zz + inp.texcoord1.xy;
                tmp1.xy = tmp1.xy / inp.texcoord1.ww;
                tmp1 = tex2D(_GrabTexture, tmp1.xy);
                o.sv_target.xyz = tmp1.xyz * _TintColor.xyz + tmp0.xyz;
                o.sv_target.w = inp.color.w * _TintColor.w;
                return o;
			}
			ENDCG
		}
	}
}