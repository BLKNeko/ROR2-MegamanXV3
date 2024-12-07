Shader "StageLib/DiffuseAlpha" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			ColorMask 0 -1
			Fog {
				Mode Off
			}
			GpuProgramID 11839
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 color : COLOR0;
				float4 position : SV_POSITION0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                o.color = saturate(v.color);
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                o.position = mul(unity_MatrixVP, tmp0);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target = inp.color;
                return o;
			}
			ENDCG
		}
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "Vertex" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask RGB -1
			ZWrite Off
			Fog {
				Mode Off
			}
			GpuProgramID 80608
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 color : COLOR0;
				float2 texcoord : TEXCOORD0;
				float4 position : SV_POSITION0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _Color;
			int4 unity_VertexLightParams;
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                float4 tmp4;
                float4 tmp5;
                float4 tmp6;
                tmp0.xyz = unity_ObjectToWorld._m10_m10_m10 * unity_MatrixV._m01_m11_m21;
                tmp0.xyz = unity_MatrixV._m00_m10_m20 * unity_ObjectToWorld._m00_m00_m00 + tmp0.xyz;
                tmp0.xyz = unity_MatrixV._m02_m12_m22 * unity_ObjectToWorld._m20_m20_m20 + tmp0.xyz;
                tmp0.xyz = unity_MatrixV._m03_m13_m23 * unity_ObjectToWorld._m30_m30_m30 + tmp0.xyz;
                tmp1.xyz = unity_ObjectToWorld._m11_m11_m11 * unity_MatrixV._m01_m11_m21;
                tmp1.xyz = unity_MatrixV._m00_m10_m20 * unity_ObjectToWorld._m01_m01_m01 + tmp1.xyz;
                tmp1.xyz = unity_MatrixV._m02_m12_m22 * unity_ObjectToWorld._m21_m21_m21 + tmp1.xyz;
                tmp1.xyz = unity_MatrixV._m03_m13_m23 * unity_ObjectToWorld._m31_m31_m31 + tmp1.xyz;
                tmp2.xyz = unity_ObjectToWorld._m12_m12_m12 * unity_MatrixV._m01_m11_m21;
                tmp2.xyz = unity_MatrixV._m00_m10_m20 * unity_ObjectToWorld._m02_m02_m02 + tmp2.xyz;
                tmp2.xyz = unity_MatrixV._m02_m12_m22 * unity_ObjectToWorld._m22_m22_m22 + tmp2.xyz;
                tmp2.xyz = unity_MatrixV._m03_m13_m23 * unity_ObjectToWorld._m32_m32_m32 + tmp2.xyz;
                tmp3.xyz = unity_ObjectToWorld._m13_m13_m13 * unity_MatrixV._m01_m11_m21;
                tmp3.xyz = unity_MatrixV._m00_m10_m20 * unity_ObjectToWorld._m03_m03_m03 + tmp3.xyz;
                tmp3.xyz = unity_MatrixV._m02_m12_m22 * unity_ObjectToWorld._m23_m23_m23 + tmp3.xyz;
                tmp3.xyz = unity_MatrixV._m03_m13_m23 * unity_ObjectToWorld._m33_m33_m33 + tmp3.xyz;
                tmp4.xyz = unity_WorldToObject._m01_m11_m21 * unity_MatrixInvV._m10_m10_m10;
                tmp4.xyz = unity_WorldToObject._m00_m10_m20 * unity_MatrixInvV._m00_m00_m00 + tmp4.xyz;
                tmp4.xyz = unity_WorldToObject._m02_m12_m22 * unity_MatrixInvV._m20_m20_m20 + tmp4.xyz;
                tmp4.xyz = unity_WorldToObject._m03_m13_m23 * unity_MatrixInvV._m30_m30_m30 + tmp4.xyz;
                tmp5.xyz = unity_WorldToObject._m01_m11_m21 * unity_MatrixInvV._m11_m11_m11;
                tmp5.xyz = unity_WorldToObject._m00_m10_m20 * unity_MatrixInvV._m01_m01_m01 + tmp5.xyz;
                tmp5.xyz = unity_WorldToObject._m02_m12_m22 * unity_MatrixInvV._m21_m21_m21 + tmp5.xyz;
                tmp5.xyz = unity_WorldToObject._m03_m13_m23 * unity_MatrixInvV._m31_m31_m31 + tmp5.xyz;
                tmp6.xyz = unity_WorldToObject._m01_m11_m21 * unity_MatrixInvV._m12_m12_m12;
                tmp6.xyz = unity_WorldToObject._m00_m10_m20 * unity_MatrixInvV._m02_m02_m02 + tmp6.xyz;
                tmp6.xyz = unity_WorldToObject._m02_m12_m22 * unity_MatrixInvV._m22_m22_m22 + tmp6.xyz;
                tmp6.xyz = unity_WorldToObject._m03_m13_m23 * unity_MatrixInvV._m32_m32_m32 + tmp6.xyz;
                tmp1.xyz = tmp1.xyz * v.vertex.yyy;
                tmp0.xyz = tmp0.xyz * v.vertex.xxx + tmp1.xyz;
                tmp0.xyz = tmp2.xyz * v.vertex.zzz + tmp0.xyz;
                tmp0.xyz = tmp3.xyz + tmp0.xyz;
                tmp1.x = dot(tmp4.xyz, v.normal.xyz);
                tmp1.y = dot(tmp5.xyz, v.normal.xyz);
                tmp1.z = dot(tmp6.xyz, v.normal.xyz);
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp2.xyz = _Color.xyz * glstate_lightmodel_ambient.xyz;
                tmp3.xyz = tmp2.xyz;
                tmp0.w = 0.0;
                for (int i = tmp0.w; i < unity_VertexLightParams.x; i += 1) {
                    tmp4.xyz = -tmp0.xyz * unity_LightPosition[i].www + unity_LightPosition[i].xyz;
                    tmp1.w = dot(tmp4.xyz, tmp4.xyz);
                    tmp2.w = unity_LightAtten[i].z * tmp1.w + 1.0;
                    tmp2.w = 1.0 / tmp2.w;
                    tmp3.w = unity_LightPosition[i].w != 0.0;
                    tmp4.w = unity_LightAtten[i].w < tmp1.w;
                    tmp3.w = tmp3.w ? tmp4.w : 0.0;
                    tmp1.w = max(tmp1.w, 0.000001);
                    tmp1.w = rsqrt(tmp1.w);
                    tmp4.xyz = tmp1.www * tmp4.xyz;
                    tmp1.w = tmp2.w * 0.5;
                    tmp1.w = tmp3.w ? 0.0 : tmp1.w;
                    tmp2.w = dot(tmp1.xyz, tmp4.xyz);
                    tmp2.w = max(tmp2.w, 0.0);
                    tmp4.xyz = tmp2.www * _Color.xyz;
                    tmp4.xyz = tmp4.xyz * unity_LightColor[i].xyz;
                    tmp4.xyz = tmp1.www * tmp4.xyz;
                    tmp4.xyz = min(tmp4.xyz, float3(1.0, 1.0, 1.0));
                    tmp3.xyz = tmp3.xyz + tmp4.xyz;
                }
                o.color.xyz = saturate(tmp3.xyz);
                o.color.w = saturate(_Color.w);
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                tmp0.xyz = tmp0.xyz * inp.color.xyz;
                o.sv_target.w = tmp0.w * inp.color.w;
                o.sv_target.xyz = tmp0.xyz + tmp0.xyz;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}