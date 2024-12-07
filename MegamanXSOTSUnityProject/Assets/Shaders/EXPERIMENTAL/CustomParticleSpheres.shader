Shader "Custom/ParticleSpheres" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_CutoffMask ("Cutoff mask", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Speed ("Speed", Float) = 1
		_Amount ("Amount", Float) = 5
		_Distance ("Distance", Float) = 0.1
		_MeltStrength ("Melt strength", Float) = 1
		_MeltStartHeight ("Melt start height", Float) = 0.25
		[Space(20)] _CutoffCube ("Cutoff cube", Cube) = "" {}
		_CutoffThreshold ("Cutoff threshold", Float) = 1
		[Space(20)] _BulgeStrength ("Bulge strength", Float) = 1
		_OffsetStrength ("Offset strength", Float) = 1
		_NoiseTex ("_NoiseTex", 2D) = "white" {}
		_Tiling ("Noise Tiling", Vector) = (1,1,1,1)
		_NoiseSpeed ("_NoiseSpeed", Vector) = (1,1,1,1)
		_CollapseStrength ("Collapse Strength", Range(-5, 5)) = 0.5
		_Gravity ("Gravity Strength", Float) = -9.81
	}
	SubShader {
		LOD 200
		Tags { "RenderType" = "Opaque" }
		Pass {
			Name "FORWARD"
			LOD 200
			Tags { "LIGHTMODE" = "FORWARDBASE" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			GpuProgramID 22502
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord5 : TEXCOORD5;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
				float3 texcoord6 : TEXCOORD6;
				float4 texcoord8 : TEXCOORD8;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _CollapseStrength;
			float _OffsetStrength;
			float _Gravity;
			float4 _Tiling;
			float2 _NoiseSpeed;
			float3 _ParticlePositions[8];
			float _ParticleSizes[8];
			float _ParticleAlpha[8];
			float _BulgeStrength;
			float _Speed;
			float _Amount;
			float _Distance;
			float _MeltStrength;
			float _MeltStartHeight;
			float4 _CutoffMask_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float _Glossiness;
			float _Metallic;
			float4 _Color;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			// Texture params for Fragment Shader
			sampler2D _CutoffMask;
			sampler2D unity_NHxRoughness;
			
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
                float4 tmp7;
                float4 tmp8;
                float4 tmp9;
                float4 tmp10;
                tmp0.xyz = v.tangent.yzx * v.normal.zxy;
                tmp0.xyz = v.normal.yzx * v.tangent.zxy + -tmp0.xyz;
                tmp1.xy = v.texcoord.xy + v.texcoord1.yy;
                tmp1 = tex2Dlod(_MainTex, float4(tmp1.xy, 0, 0.0), 0);
                tmp0.w = tmp1.x + tmp1.x;
                tmp1.xy = v.texcoord.zw;
                tmp1.z = v.texcoord1.x;
                tmp1.xyz = v.vertex.xyz - tmp1.xyz;
                tmp1.w = v.texcoord1.y * 999999.6;
                tmp2 = mul(unity_WorldToObject, v.vertex);
                tmp3.xy = _Time.yy * _NoiseSpeed + _Tiling.xy;
                tmp3.xy = v.texcoord.xy * _Tiling.zw + tmp3.xy;
                tmp3 = tex2Dlod(_NoiseTex, float4(tmp3.xy, 0, 0.0), 0);
                tmp3.xyz = saturate(tmp3.xyz + _OffsetStrength.xxx);
                tmp3.w = dot(-tmp1.xyz, -tmp1.xyz);
                tmp3.w = rsqrt(tmp3.w);
                tmp4.xyz = -tmp1.xyz * tmp3.www;
                tmp4.xyz = tmp4.xyz * _CollapseStrength.xxx;
                tmp5.xyz = tmp4.xyz * tmp3.xyz + tmp2.xyz;
                tmp3.w = max(_CollapseStrength, 0.0);
                tmp3.w = min(tmp3.w, _CollapseStrength);
                tmp3.w = tmp3.w * _Gravity;
                tmp4.w = dot(tmp1.xyz, tmp1.xyz);
                tmp4.w = sqrt(tmp4.w);
                tmp5.w = tmp3.w * tmp4.w + tmp5.y;
                tmp6.xyz = tmp1.xyz * float3(9.0, 9.0, 9.0) + tmp0.www;
                tmp6.xyz = v.texcoord1.yyy * float3(999999.6, 999999.6, 999999.6) + tmp6.xyz;
                tmp6.xyz = _Time.www * _Speed.xxx + tmp6.xyz;
                tmp6.xyz = sin(tmp6.xyz);
                tmp6.xyz = tmp6.xyz + float3(1.0, 1.0, 1.0);
                tmp1.xyz = tmp1.xyz * tmp6.xyz;
                tmp1.xyz = tmp1.xyz * _Amount.xxx;
                tmp5.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp5.xwz;
                tmp6 = tmp5.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp6 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp5.xxxx + tmp6;
                tmp5 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp5.zzzz + tmp6;
                tmp5 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp2.wwww + tmp5;
                tmp0.w = saturate(tmp5.y / _MeltStartHeight);
                tmp0.w = 1.0 - tmp0.w;
                tmp0.w = tmp0.w * tmp0.w;
                tmp6.x = tmp1.w * tmp1.w;
                tmp6.x = rsqrt(tmp6.x);
                tmp1.w = tmp1.w * tmp6.x;
                tmp1.w = tmp1.w * _MeltStrength;
                tmp0.w = tmp0.w * tmp1.w;
                tmp5.xz = v.normal.xz * tmp0.ww + tmp5.xz;
                tmp6 = mul(unity_WorldToObject, tmp5);
                tmp0.w = _Time.w * 6.0;
                tmp5.w = 0.0;
                tmp7.xyz = tmp6.xyz;
                tmp7.w = 0.0;
                for (int i = tmp7.w; i < 8; i += 1) {
                    tmp8.xyz = tmp5.xyz - _ParticlePositions[i];
                    tmp8.x = dot(tmp8.xyz, tmp8.xyz);
                    tmp8.x = sqrt(tmp8.x);
                    tmp8.x = saturate(tmp8.x / _ParticleSizes[i]);
                    tmp8.x = 1.0 - tmp8.x;
                    tmp8.y = tmp8.x * 1.5;
                    tmp8.z = tmp8.x * 9.0 + tmp0.w;
                    tmp8.yz = sin(tmp8.yz);
                    tmp8.z = tmp8.z + 1.0;
                    tmp8.x = tmp8.x * tmp8.z;
                    tmp8.x = tmp8.x * 0.5 + tmp8.y;
                    tmp8.y = tmp8.x * _ParticleAlpha[i];
                    tmp5.w = tmp8.x * _ParticleAlpha[i] + tmp5.w;
                    tmp8.xzw = _ParticlePositions[i] - tmp5.xyz;
                    tmp9.xyz = tmp8.zzz * unity_WorldToObject._m01_m11_m21;
                    tmp9.xyz = unity_WorldToObject._m00_m10_m20 * tmp8.xxx + tmp9.xyz;
                    tmp8.xzw = unity_WorldToObject._m02_m12_m22 * tmp8.www + tmp9.xyz;
                    tmp8.xzw = tmp8.xzw + v.normal.xyz;
                    tmp8.xzw = tmp8.xzw * _BulgeStrength.xxx;
                    tmp7.xyz = tmp8.xzw * tmp8.yyy + tmp7.xyz;
                }
                o.texcoord5.y = tmp5.w;
                tmp5 = tmp7.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp5 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp7.xxxx + tmp5;
                tmp5 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp7.zzzz + tmp5;
                tmp5 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp6.wwww + tmp5;
                tmp5.y = max(tmp5.y, 0.01);
                tmp5.y = min(tmp5.y, 100000000.0);
                tmp6 = tmp5.yyyy * unity_WorldToObject._m01_m11_m21_m31;
                tmp6 = unity_WorldToObject._m00_m10_m20_m30 * tmp5.xxxx + tmp6;
                tmp6 = unity_WorldToObject._m02_m12_m22_m32 * tmp5.zzzz + tmp6;
                tmp5 = unity_WorldToObject._m03_m13_m23_m33 * tmp5.wwww + tmp6;
                tmp6 = v.tangent * _Distance.xxxx + tmp2;
                tmp7.xyz = tmp4.xyz * tmp3.xyz + tmp6.xyz;
                tmp7.w = tmp3.w * tmp4.w + tmp7.y;
                tmp6.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp7.xwz;
                tmp7 = tmp6.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp7 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp6.xxxx + tmp7;
                tmp7 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp6.zzzz + tmp7;
                tmp6 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp6.wwww + tmp7;
                tmp7.x = saturate(tmp6.y / _MeltStartHeight);
                tmp7.x = 1.0 - tmp7.x;
                tmp7.x = tmp7.x * tmp7.x;
                tmp7.x = tmp1.w * tmp7.x;
                tmp6.xz = v.normal.xz * tmp7.xx + tmp6.xz;
                tmp7 = mul(unity_WorldToObject, tmp6);
                tmp8.xyz = tmp7.xyz;
                tmp6.w = 0.0;
                for (int j = tmp6.w; j < 8; j += 1) {
                    tmp9.xyz = tmp6.xyz - _ParticlePositions[j];
                    tmp8.w = dot(tmp9.xyz, tmp9.xyz);
                    tmp8.w = sqrt(tmp8.w);
                    tmp8.w = saturate(tmp8.w / _ParticleSizes[j]);
                    tmp8.w = 1.0 - tmp8.w;
                    tmp9.x = tmp8.w * 1.5;
                    tmp9.y = tmp8.w * 9.0 + tmp0.w;
                    tmp9.xy = sin(tmp9.xy);
                    tmp9.y = tmp9.y + 1.0;
                    tmp8.w = tmp8.w * tmp9.y;
                    tmp8.w = tmp8.w * 0.5 + tmp9.x;
                    tmp8.w = tmp8.w * _ParticleAlpha[j];
                    tmp9.xyz = _ParticlePositions[j] - tmp6.xyz;
                    tmp10.xyz = tmp9.yyy * unity_WorldToObject._m01_m11_m21;
                    tmp9.xyw = unity_WorldToObject._m00_m10_m20 * tmp9.xxx + tmp10.xyz;
                    tmp9.xyz = unity_WorldToObject._m02_m12_m22 * tmp9.zzz + tmp9.xyw;
                    tmp9.xyz = tmp9.xyz + v.normal.xyz;
                    tmp9.xyz = tmp9.xyz * _BulgeStrength.xxx;
                    tmp8.xyz = tmp9.xyz * tmp8.www + tmp8.xyz;
                }
                tmp6 = tmp8.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp6 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp8.xxxx + tmp6;
                tmp6 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp8.zzzz + tmp6;
                tmp6 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp7.wwww + tmp6;
                tmp6.y = max(tmp6.y, 0.01);
                tmp6.y = min(tmp6.y, 100000000.0);
                tmp7.xyz = tmp6.yyy * unity_WorldToObject._m21_m01_m11;
                tmp7.xyz = unity_WorldToObject._m20_m00_m10 * tmp6.xxx + tmp7.xyz;
                tmp6.xyz = unity_WorldToObject._m22_m02_m12 * tmp6.zzz + tmp7.xyz;
                tmp6.xyz = unity_WorldToObject._m23_m03_m13 * tmp6.www + tmp6.xyz;
                tmp7.xyz = tmp0.xyz * _Distance.xxx;
                tmp7.w = 0.0;
                tmp2 = tmp2 + tmp7;
                tmp7.xyz = tmp4.xyz * tmp3.xyz + tmp2.xyz;
                tmp7.w = tmp3.w * tmp4.w + tmp7.y;
                tmp0.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp7.xwz;
                tmp3 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp3 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp3;
                tmp3 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp3;
                tmp2 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                tmp0.x = saturate(tmp2.y / _MeltStartHeight);
                tmp0.x = 1.0 - tmp0.x;
                tmp0.x = tmp0.x * tmp0.x;
                tmp0.x = tmp1.w * tmp0.x;
                tmp2.xz = v.normal.xz * tmp0.xx + tmp2.xz;
                tmp1 = mul(unity_WorldToObject, tmp2);
                tmp0.xyz = tmp1.xyz;
                tmp2.w = 0.0;
                for (int k = tmp2.w; k < 8; k += 1) {
                    tmp3.xyz = tmp2.xyz - _ParticlePositions[k];
                    tmp3.x = dot(tmp3.xyz, tmp3.xyz);
                    tmp3.x = sqrt(tmp3.x);
                    tmp3.x = saturate(tmp3.x / _ParticleSizes[k]);
                    tmp3.x = 1.0 - tmp3.x;
                    tmp3.y = tmp3.x * 1.5;
                    tmp3.z = tmp3.x * 9.0 + tmp0.w;
                    tmp3.yz = sin(tmp3.yz);
                    tmp3.z = tmp3.z + 1.0;
                    tmp3.x = tmp3.x * tmp3.z;
                    tmp3.x = tmp3.x * 0.5 + tmp3.y;
                    tmp3.x = tmp3.x * _ParticleAlpha[k];
                    tmp3.yzw = _ParticlePositions[k] - tmp2.xyz;
                    tmp4.xyz = tmp3.zzz * unity_WorldToObject._m01_m11_m21;
                    tmp4.xyz = unity_WorldToObject._m00_m10_m20 * tmp3.yyy + tmp4.xyz;
                    tmp3.yzw = unity_WorldToObject._m02_m12_m22 * tmp3.www + tmp4.xyz;
                    tmp3.yzw = tmp3.yzw + v.normal.xyz;
                    tmp3.yzw = tmp3.yzw * _BulgeStrength.xxx;
                    tmp0.xyz = tmp3.yzw * tmp3.xxx + tmp0.xyz;
                }
                tmp2 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                tmp0.y = max(tmp0.y, 0.01);
                tmp0.y = min(tmp0.y, 100000000.0);
                tmp1.xyz = tmp0.yyy * unity_WorldToObject._m11_m21_m01;
                tmp1.xyz = unity_WorldToObject._m10_m20_m00 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_WorldToObject._m12_m22_m02 * tmp0.zzz + tmp1.xyz;
                tmp0.xyz = unity_WorldToObject._m13_m23_m03 * tmp0.www + tmp0.xyz;
                tmp1.xyz = tmp6.xyz - tmp5.zxy;
                tmp0.xyz = tmp0.xyz - tmp5.yzx;
                tmp2.xyz = tmp0.xyz * tmp1.xyz;
                tmp0.xyz = tmp1.zxy * tmp0.yzx + -tmp2.xyz;
                tmp1 = mul(unity_ObjectToWorld, tmp5);
                tmp2 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp3 = tmp2 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp4 = tmp3.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp4 = unity_MatrixVP._m00_m10_m20_m30 * tmp3.xxxx + tmp4;
                tmp4 = unity_MatrixVP._m02_m12_m22_m32 * tmp3.zzzz + tmp4;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp3.wwww + tmp4;
                o.texcoord.xy = v.texcoord.xy * _CutoffMask_ST.xy + _CutoffMask_ST.zw;
                o.texcoord2.xyz = unity_ObjectToWorld._m03_m13_m23 * tmp1.www + tmp2.xyz;
                tmp1.x = dot(tmp0.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(tmp0.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(tmp0.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp1 = tmp0.yzzx * tmp0.xyzz;
                tmp2.x = dot(unity_SHBr, tmp1);
                tmp2.y = dot(unity_SHBg, tmp1);
                tmp2.z = dot(unity_SHBb, tmp1);
                tmp0.w = tmp0.y * tmp0.y;
                tmp0.w = tmp0.x * tmp0.x + -tmp0.w;
                o.texcoord6.xyz = unity_SHC.xyz * tmp0.www + tmp2.xyz;
                o.texcoord5.x = v.texcoord2.z;
                o.texcoord3 = v.color;
                o.texcoord4.xy = v.texcoord1.zw;
                o.texcoord4.zw = v.texcoord2.xy;
                o.texcoord8 = float4(0.0, 0.0, 0.0, 0.0);
                o.texcoord1.xyz = tmp0.xyz;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                float4 tmp4;
                float4 tmp5;
                float4 tmp6;
                float4 tmp7;
                tmp0.xy = inp.texcoord.xy + _Time.zz;
                tmp0 = tex2D(_CutoffMask, tmp0.xy);
                tmp0.x = tmp0.y < inp.texcoord5.x;
                if (tmp0.x) {
                    discard;
                }
                tmp0.xyz = inp.texcoord3.xyz * _Color.xyz;
                tmp1 = tex2D(_CutoffMask, inp.texcoord.xy);
                tmp1.xyz = tmp1.xyz * inp.texcoord4.xyz;
                tmp1.xyz = tmp1.xyz * inp.texcoord5.yyy + tmp1.xyz;
                tmp2.xyz = _WorldSpaceCameraPos - inp.texcoord2.xyz;
                tmp0.w = dot(tmp2.xyz, tmp2.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp0.www * tmp2.xyz;
                tmp3.xw = float2(1.0, 1.0) - _Glossiness.xx;
                tmp0.w = dot(-tmp2.xyz, inp.texcoord1.xyz);
                tmp0.w = tmp0.w + tmp0.w;
                tmp4.xyz = inp.texcoord1.xyz * -tmp0.www + -tmp2.xyz;
                tmp5.xyz = inp.texcoord1.xyz;
                tmp5.w = 1.0;
                tmp6.x = dot(unity_SHAr, tmp5);
                tmp6.y = dot(unity_SHAg, tmp5);
                tmp6.z = dot(unity_SHAb, tmp5);
                tmp5.xyz = tmp6.xyz + inp.texcoord6.xyz;
                tmp5.xyz = max(tmp5.xyz, float3(0.0, 0.0, 0.0));
                tmp5.xyz = log(tmp5.xyz);
                tmp5.xyz = tmp5.xyz * float3(0.4166667, 0.4166667, 0.4166667);
                tmp5.xyz = exp(tmp5.xyz);
                tmp5.xyz = tmp5.xyz * float3(1.055, 1.055, 1.055) + float3(-0.055, -0.055, -0.055);
                tmp5.xyz = max(tmp5.xyz, float3(0.0, 0.0, 0.0));
                tmp0.w = -tmp3.x * 0.7 + 1.7;
                tmp0.w = tmp0.w * tmp3.x;
                tmp0.w = tmp0.w * 6.0;
                tmp4 = UNITY_SAMPLE_TEXCUBE_SAMPLER(unity_SpecCube0, unity_SpecCube0, float4(tmp4.xyz, tmp0.w));
                tmp0.w = tmp4.w - 1.0;
                tmp0.w = unity_SpecCube0_HDR.w * tmp0.w + 1.0;
                tmp0.w = tmp0.w * unity_SpecCube0_HDR.x;
                tmp4.xyz = tmp4.xyz * tmp0.www;
                tmp0.w = dot(inp.texcoord1.xyz, inp.texcoord1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp6.xyz = tmp0.www * inp.texcoord1.xyz;
                tmp7.xyz = _Color.xyz * inp.texcoord3.xyz + float3(-0.2209163, -0.2209163, -0.2209163);
                tmp7.xyz = _Metallic.xxx * tmp7.xyz + float3(0.2209163, 0.2209163, 0.2209163);
                tmp0.w = -_Metallic * 0.7790837 + 0.7790837;
                tmp0.xyz = tmp0.www * tmp0.xyz;
                tmp1.w = dot(tmp2.xyz, tmp6.xyz);
                tmp2.w = tmp1.w + tmp1.w;
                tmp2.xyz = tmp6.xyz * -tmp2.www + tmp2.xyz;
                tmp2.w = saturate(dot(tmp6.xyz, _WorldSpaceLightPos0.xyz));
                tmp1.w = saturate(tmp1.w);
                tmp6.x = dot(tmp2.xyz, _WorldSpaceLightPos0.xyz);
                tmp6.y = 1.0 - tmp1.w;
                tmp6.zw = tmp6.xy * tmp6.xy;
                tmp2.xy = tmp6.xy * tmp6.xw;
                tmp3.yz = tmp6.zy * tmp2.xy;
                tmp0.w = 1.0 - tmp0.w;
                tmp0.w = saturate(tmp0.w + _Glossiness);
                tmp6 = tex2D(unity_NHxRoughness, tmp3.yw);
                tmp1.w = tmp6.x * 16.0;
                tmp2.xyz = tmp1.www * tmp7.xyz + tmp0.xyz;
                tmp3.xyw = tmp2.www * _LightColor0.xyz;
                tmp6.xyz = tmp0.www - tmp7.xyz;
                tmp6.xyz = tmp3.zzz * tmp6.xyz + tmp7.xyz;
                tmp4.xyz = tmp4.xyz * tmp6.xyz;
                tmp0.xyz = tmp5.xyz * tmp0.xyz + tmp4.xyz;
                tmp0.xyz = tmp2.xyz * tmp3.xyw + tmp0.xyz;
                o.sv_target.xyz = tmp1.xyz + tmp0.xyz;
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "FORWARD"
			LOD 200
			Tags { "LIGHTMODE" = "FORWARDADD" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			Blend One One, One One
			ZWrite Off
			GpuProgramID 84396
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord5 : TEXCOORD5;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
				float3 texcoord6 : TEXCOORD6;
				float4 texcoord7 : TEXCOORD7;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 unity_WorldToLight;
			float _CollapseStrength;
			float _OffsetStrength;
			float _Gravity;
			float4 _Tiling;
			float2 _NoiseSpeed;
			float3 _ParticlePositions[8];
			float _ParticleSizes[8];
			float _ParticleAlpha[8];
			float _BulgeStrength;
			float _Speed;
			float _Amount;
			float _Distance;
			float _MeltStrength;
			float _MeltStartHeight;
			float4 _CutoffMask_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float _Glossiness;
			float _Metallic;
			float4 _Color;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			// Texture params for Fragment Shader
			sampler2D _CutoffMask;
			sampler2D _LightTexture0;
			sampler2D unity_NHxRoughness;
			
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
                float4 tmp7;
                float4 tmp8;
                float4 tmp9;
                float4 tmp10;
                tmp0.xyz = v.tangent.yzx * v.normal.zxy;
                tmp0.xyz = v.normal.yzx * v.tangent.zxy + -tmp0.xyz;
                tmp1.xy = v.texcoord.xy + v.texcoord1.yy;
                tmp1 = tex2Dlod(_MainTex, float4(tmp1.xy, 0, 0.0), 0);
                tmp0.w = tmp1.x + tmp1.x;
                tmp1.xy = v.texcoord.zw;
                tmp1.z = v.texcoord1.x;
                tmp1.xyz = v.vertex.xyz - tmp1.xyz;
                tmp1.w = v.texcoord1.y * 999999.6;
                tmp2 = mul(unity_WorldToObject, v.vertex);
                tmp3.xy = _Time.yy * _NoiseSpeed + _Tiling.xy;
                tmp3.xy = v.texcoord.xy * _Tiling.zw + tmp3.xy;
                tmp3 = tex2Dlod(_NoiseTex, float4(tmp3.xy, 0, 0.0), 0);
                tmp3.xyz = saturate(tmp3.xyz + _OffsetStrength.xxx);
                tmp3.w = dot(-tmp1.xyz, -tmp1.xyz);
                tmp3.w = rsqrt(tmp3.w);
                tmp4.xyz = -tmp1.xyz * tmp3.www;
                tmp4.xyz = tmp4.xyz * _CollapseStrength.xxx;
                tmp5.xyz = tmp4.xyz * tmp3.xyz + tmp2.xyz;
                tmp3.w = max(_CollapseStrength, 0.0);
                tmp3.w = min(tmp3.w, _CollapseStrength);
                tmp3.w = tmp3.w * _Gravity;
                tmp4.w = dot(tmp1.xyz, tmp1.xyz);
                tmp4.w = sqrt(tmp4.w);
                tmp5.w = tmp3.w * tmp4.w + tmp5.y;
                tmp6.xyz = tmp1.xyz * float3(9.0, 9.0, 9.0) + tmp0.www;
                tmp6.xyz = v.texcoord1.yyy * float3(999999.6, 999999.6, 999999.6) + tmp6.xyz;
                tmp6.xyz = _Time.www * _Speed.xxx + tmp6.xyz;
                tmp6.xyz = sin(tmp6.xyz);
                tmp6.xyz = tmp6.xyz + float3(1.0, 1.0, 1.0);
                tmp1.xyz = tmp1.xyz * tmp6.xyz;
                tmp1.xyz = tmp1.xyz * _Amount.xxx;
                tmp5.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp5.xwz;
                tmp6 = tmp5.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp6 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp5.xxxx + tmp6;
                tmp5 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp5.zzzz + tmp6;
                tmp5 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp2.wwww + tmp5;
                tmp0.w = saturate(tmp5.y / _MeltStartHeight);
                tmp0.w = 1.0 - tmp0.w;
                tmp0.w = tmp0.w * tmp0.w;
                tmp6.x = tmp1.w * tmp1.w;
                tmp6.x = rsqrt(tmp6.x);
                tmp1.w = tmp1.w * tmp6.x;
                tmp1.w = tmp1.w * _MeltStrength;
                tmp0.w = tmp0.w * tmp1.w;
                tmp5.xz = v.normal.xz * tmp0.ww + tmp5.xz;
                tmp6 = mul(unity_WorldToObject, tmp5);
                tmp0.w = _Time.w * 6.0;
                tmp5.w = 0.0;
                tmp7.xyz = tmp6.xyz;
                tmp7.w = 0.0;
                for (int i = tmp7.w; i < 8; i += 1) {
                    tmp8.xyz = tmp5.xyz - _ParticlePositions[i];
                    tmp8.x = dot(tmp8.xyz, tmp8.xyz);
                    tmp8.x = sqrt(tmp8.x);
                    tmp8.x = saturate(tmp8.x / _ParticleSizes[i]);
                    tmp8.x = 1.0 - tmp8.x;
                    tmp8.y = tmp8.x * 1.5;
                    tmp8.z = tmp8.x * 9.0 + tmp0.w;
                    tmp8.yz = sin(tmp8.yz);
                    tmp8.z = tmp8.z + 1.0;
                    tmp8.x = tmp8.x * tmp8.z;
                    tmp8.x = tmp8.x * 0.5 + tmp8.y;
                    tmp8.y = tmp8.x * _ParticleAlpha[i];
                    tmp5.w = tmp8.x * _ParticleAlpha[i] + tmp5.w;
                    tmp8.xzw = _ParticlePositions[i] - tmp5.xyz;
                    tmp9.xyz = tmp8.zzz * unity_WorldToObject._m01_m11_m21;
                    tmp9.xyz = unity_WorldToObject._m00_m10_m20 * tmp8.xxx + tmp9.xyz;
                    tmp8.xzw = unity_WorldToObject._m02_m12_m22 * tmp8.www + tmp9.xyz;
                    tmp8.xzw = tmp8.xzw + v.normal.xyz;
                    tmp8.xzw = tmp8.xzw * _BulgeStrength.xxx;
                    tmp7.xyz = tmp8.xzw * tmp8.yyy + tmp7.xyz;
                }
                o.texcoord5.y = tmp5.w;
                tmp5 = tmp7.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp5 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp7.xxxx + tmp5;
                tmp5 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp7.zzzz + tmp5;
                tmp5 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp6.wwww + tmp5;
                tmp5.y = max(tmp5.y, 0.01);
                tmp5.y = min(tmp5.y, 100000000.0);
                tmp6 = tmp5.yyyy * unity_WorldToObject._m01_m11_m21_m31;
                tmp6 = unity_WorldToObject._m00_m10_m20_m30 * tmp5.xxxx + tmp6;
                tmp6 = unity_WorldToObject._m02_m12_m22_m32 * tmp5.zzzz + tmp6;
                tmp5 = unity_WorldToObject._m03_m13_m23_m33 * tmp5.wwww + tmp6;
                tmp6 = v.tangent * _Distance.xxxx + tmp2;
                tmp7.xyz = tmp4.xyz * tmp3.xyz + tmp6.xyz;
                tmp7.w = tmp3.w * tmp4.w + tmp7.y;
                tmp6.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp7.xwz;
                tmp7 = tmp6.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp7 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp6.xxxx + tmp7;
                tmp7 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp6.zzzz + tmp7;
                tmp6 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp6.wwww + tmp7;
                tmp7.x = saturate(tmp6.y / _MeltStartHeight);
                tmp7.x = 1.0 - tmp7.x;
                tmp7.x = tmp7.x * tmp7.x;
                tmp7.x = tmp1.w * tmp7.x;
                tmp6.xz = v.normal.xz * tmp7.xx + tmp6.xz;
                tmp7 = mul(unity_WorldToObject, tmp6);
                tmp8.xyz = tmp7.xyz;
                tmp6.w = 0.0;
                for (int j = tmp6.w; j < 8; j += 1) {
                    tmp9.xyz = tmp6.xyz - _ParticlePositions[j];
                    tmp8.w = dot(tmp9.xyz, tmp9.xyz);
                    tmp8.w = sqrt(tmp8.w);
                    tmp8.w = saturate(tmp8.w / _ParticleSizes[j]);
                    tmp8.w = 1.0 - tmp8.w;
                    tmp9.x = tmp8.w * 1.5;
                    tmp9.y = tmp8.w * 9.0 + tmp0.w;
                    tmp9.xy = sin(tmp9.xy);
                    tmp9.y = tmp9.y + 1.0;
                    tmp8.w = tmp8.w * tmp9.y;
                    tmp8.w = tmp8.w * 0.5 + tmp9.x;
                    tmp8.w = tmp8.w * _ParticleAlpha[j];
                    tmp9.xyz = _ParticlePositions[j] - tmp6.xyz;
                    tmp10.xyz = tmp9.yyy * unity_WorldToObject._m01_m11_m21;
                    tmp9.xyw = unity_WorldToObject._m00_m10_m20 * tmp9.xxx + tmp10.xyz;
                    tmp9.xyz = unity_WorldToObject._m02_m12_m22 * tmp9.zzz + tmp9.xyw;
                    tmp9.xyz = tmp9.xyz + v.normal.xyz;
                    tmp9.xyz = tmp9.xyz * _BulgeStrength.xxx;
                    tmp8.xyz = tmp9.xyz * tmp8.www + tmp8.xyz;
                }
                tmp6 = tmp8.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp6 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp8.xxxx + tmp6;
                tmp6 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp8.zzzz + tmp6;
                tmp6 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp7.wwww + tmp6;
                tmp6.y = max(tmp6.y, 0.01);
                tmp6.y = min(tmp6.y, 100000000.0);
                tmp7.xyz = tmp6.yyy * unity_WorldToObject._m21_m01_m11;
                tmp7.xyz = unity_WorldToObject._m20_m00_m10 * tmp6.xxx + tmp7.xyz;
                tmp6.xyz = unity_WorldToObject._m22_m02_m12 * tmp6.zzz + tmp7.xyz;
                tmp6.xyz = unity_WorldToObject._m23_m03_m13 * tmp6.www + tmp6.xyz;
                tmp7.xyz = tmp0.xyz * _Distance.xxx;
                tmp7.w = 0.0;
                tmp2 = tmp2 + tmp7;
                tmp7.xyz = tmp4.xyz * tmp3.xyz + tmp2.xyz;
                tmp7.w = tmp3.w * tmp4.w + tmp7.y;
                tmp0.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp7.xwz;
                tmp3 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp3 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp3;
                tmp3 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp3;
                tmp2 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                tmp0.x = saturate(tmp2.y / _MeltStartHeight);
                tmp0.x = 1.0 - tmp0.x;
                tmp0.x = tmp0.x * tmp0.x;
                tmp0.x = tmp1.w * tmp0.x;
                tmp2.xz = v.normal.xz * tmp0.xx + tmp2.xz;
                tmp1 = mul(unity_WorldToObject, tmp2);
                tmp0.xyz = tmp1.xyz;
                tmp2.w = 0.0;
                for (int k = tmp2.w; k < 8; k += 1) {
                    tmp3.xyz = tmp2.xyz - _ParticlePositions[k];
                    tmp3.x = dot(tmp3.xyz, tmp3.xyz);
                    tmp3.x = sqrt(tmp3.x);
                    tmp3.x = saturate(tmp3.x / _ParticleSizes[k]);
                    tmp3.x = 1.0 - tmp3.x;
                    tmp3.y = tmp3.x * 1.5;
                    tmp3.z = tmp3.x * 9.0 + tmp0.w;
                    tmp3.yz = sin(tmp3.yz);
                    tmp3.z = tmp3.z + 1.0;
                    tmp3.x = tmp3.x * tmp3.z;
                    tmp3.x = tmp3.x * 0.5 + tmp3.y;
                    tmp3.x = tmp3.x * _ParticleAlpha[k];
                    tmp3.yzw = _ParticlePositions[k] - tmp2.xyz;
                    tmp4.xyz = tmp3.zzz * unity_WorldToObject._m01_m11_m21;
                    tmp4.xyz = unity_WorldToObject._m00_m10_m20 * tmp3.yyy + tmp4.xyz;
                    tmp3.yzw = unity_WorldToObject._m02_m12_m22 * tmp3.www + tmp4.xyz;
                    tmp3.yzw = tmp3.yzw + v.normal.xyz;
                    tmp3.yzw = tmp3.yzw * _BulgeStrength.xxx;
                    tmp0.xyz = tmp3.yzw * tmp3.xxx + tmp0.xyz;
                }
                tmp2 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                tmp0.y = max(tmp0.y, 0.01);
                tmp0.y = min(tmp0.y, 100000000.0);
                tmp1.xyz = tmp0.yyy * unity_WorldToObject._m11_m21_m01;
                tmp1.xyz = unity_WorldToObject._m10_m20_m00 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_WorldToObject._m12_m22_m02 * tmp0.zzz + tmp1.xyz;
                tmp0.xyz = unity_WorldToObject._m13_m23_m03 * tmp0.www + tmp0.xyz;
                tmp1.xyz = tmp6.xyz - tmp5.zxy;
                tmp0.xyz = tmp0.xyz - tmp5.yzx;
                tmp2.xyz = tmp0.xyz * tmp1.xyz;
                tmp0.xyz = tmp1.zxy * tmp0.yzx + -tmp2.xyz;
                tmp1 = mul(unity_ObjectToWorld, tmp5);
                tmp2 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp3 = tmp2 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp4 = tmp3.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp4 = unity_MatrixVP._m00_m10_m20_m30 * tmp3.xxxx + tmp4;
                tmp4 = unity_MatrixVP._m02_m12_m22_m32 * tmp3.zzzz + tmp4;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp3.wwww + tmp4;
                o.texcoord.xy = v.texcoord.xy * _CutoffMask_ST.xy + _CutoffMask_ST.zw;
                o.texcoord2.xyz = unity_ObjectToWorld._m03_m13_m23 * tmp1.www + tmp2.xyz;
                tmp1.x = dot(tmp0.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(tmp0.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(tmp0.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                tmp0.x = rsqrt(tmp0.x);
                o.texcoord1.xyz = tmp0.xxx * tmp1.xyz;
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                tmp1.xyz = tmp0.yyy * unity_WorldToLight._m01_m11_m21;
                tmp1.xyz = unity_WorldToLight._m00_m10_m20 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_WorldToLight._m02_m12_m22 * tmp0.zzz + tmp1.xyz;
                o.texcoord6.xyz = unity_WorldToLight._m03_m13_m23 * tmp0.www + tmp0.xyz;
                o.texcoord5.x = v.texcoord2.z;
                o.texcoord3 = v.color;
                o.texcoord4.xy = v.texcoord1.zw;
                o.texcoord4.zw = v.texcoord2.xy;
                o.texcoord7 = float4(0.0, 0.0, 0.0, 0.0);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                float4 tmp4;
                float4 tmp5;
                tmp0.xy = inp.texcoord.xy + _Time.zz;
                tmp0 = tex2D(_CutoffMask, tmp0.xy);
                tmp0.x = tmp0.y < inp.texcoord5.x;
                if (tmp0.x) {
                    discard;
                }
                tmp0.xyz = inp.texcoord3.xyz * _Color.xyz;
                tmp1.xyz = _WorldSpaceLightPos0.xyz - inp.texcoord2.xyz;
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp2.xyz = _WorldSpaceCameraPos - inp.texcoord2.xyz;
                tmp0.w = dot(tmp2.xyz, tmp2.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp0.www * tmp2.xyz;
                tmp3.xyz = mul(unity_WorldToLight, inp.texcoord2.xyz);
                tmp0.w = dot(tmp3.xyz, tmp3.xyz);
                tmp3 = tex2D(_LightTexture0, tmp0.ww);
                tmp3.xyz = tmp3.xxx * _LightColor0.xyz;
                tmp0.w = dot(inp.texcoord1.xyz, inp.texcoord1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp4.xyz = tmp0.www * inp.texcoord1.xyz;
                tmp5.xyz = _Color.xyz * inp.texcoord3.xyz + float3(-0.2209163, -0.2209163, -0.2209163);
                tmp5.xyz = _Metallic.xxx * tmp5.xyz + float3(0.2209163, 0.2209163, 0.2209163);
                tmp0.w = -_Metallic * 0.7790837 + 0.7790837;
                tmp1.w = dot(tmp2.xyz, tmp4.xyz);
                tmp1.w = tmp1.w + tmp1.w;
                tmp2.xyz = tmp4.xyz * -tmp1.www + tmp2.xyz;
                tmp1.w = saturate(dot(tmp4.xyz, tmp1.xyz));
                tmp1.x = dot(tmp2.xyz, tmp1.xyz);
                tmp1.x = tmp1.x * tmp1.x;
                tmp1.x = tmp1.x * tmp1.x;
                tmp1.y = 1.0 - _Glossiness;
                tmp2 = tex2D(unity_NHxRoughness, tmp1.xy);
                tmp1.x = tmp2.x * 16.0;
                tmp1.xyz = tmp5.xyz * tmp1.xxx;
                tmp0.xyz = tmp0.xyz * tmp0.www + tmp1.xyz;
                tmp1.xyz = tmp1.www * tmp3.xyz;
                o.sv_target.xyz = tmp0.xyz * tmp1.xyz;
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "DEFERRED"
			LOD 200
			Tags { "LIGHTMODE" = "DEFERRED" "RenderType" = "Opaque" }
			GpuProgramID 184113
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord5 : TEXCOORD5;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
				float4 texcoord7 : TEXCOORD7;
				float3 texcoord8 : TEXCOORD8;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
				float4 sv_target1 : SV_Target1;
				float4 sv_target2 : SV_Target2;
				float4 sv_target3 : SV_Target3;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _CollapseStrength;
			float _OffsetStrength;
			float _Gravity;
			float4 _Tiling;
			float2 _NoiseSpeed;
			float3 _ParticlePositions[8];
			float _ParticleSizes[8];
			float _ParticleAlpha[8];
			float _BulgeStrength;
			float _Speed;
			float _Amount;
			float _Distance;
			float _MeltStrength;
			float _MeltStartHeight;
			float4 _CutoffMask_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _Glossiness;
			float _Metallic;
			float4 _Color;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			// Texture params for Fragment Shader
			sampler2D _CutoffMask;
			
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
                float4 tmp7;
                float4 tmp8;
                float4 tmp9;
                float4 tmp10;
                tmp0.xyz = v.tangent.yzx * v.normal.zxy;
                tmp0.xyz = v.normal.yzx * v.tangent.zxy + -tmp0.xyz;
                tmp1.xy = v.texcoord.xy + v.texcoord1.yy;
                tmp1 = tex2Dlod(_MainTex, float4(tmp1.xy, 0, 0.0), 0);
                tmp0.w = tmp1.x + tmp1.x;
                tmp1.xy = v.texcoord.zw;
                tmp1.z = v.texcoord1.x;
                tmp1.xyz = v.vertex.xyz - tmp1.xyz;
                tmp1.w = v.texcoord1.y * 999999.6;
                tmp2 = mul(unity_WorldToObject, v.vertex);
                tmp3.xy = _Time.yy * _NoiseSpeed + _Tiling.xy;
                tmp3.xy = v.texcoord.xy * _Tiling.zw + tmp3.xy;
                tmp3 = tex2Dlod(_NoiseTex, float4(tmp3.xy, 0, 0.0), 0);
                tmp3.xyz = saturate(tmp3.xyz + _OffsetStrength.xxx);
                tmp3.w = dot(-tmp1.xyz, -tmp1.xyz);
                tmp3.w = rsqrt(tmp3.w);
                tmp4.xyz = -tmp1.xyz * tmp3.www;
                tmp4.xyz = tmp4.xyz * _CollapseStrength.xxx;
                tmp5.xyz = tmp4.xyz * tmp3.xyz + tmp2.xyz;
                tmp3.w = max(_CollapseStrength, 0.0);
                tmp3.w = min(tmp3.w, _CollapseStrength);
                tmp3.w = tmp3.w * _Gravity;
                tmp4.w = dot(tmp1.xyz, tmp1.xyz);
                tmp4.w = sqrt(tmp4.w);
                tmp5.w = tmp3.w * tmp4.w + tmp5.y;
                tmp6.xyz = tmp1.xyz * float3(9.0, 9.0, 9.0) + tmp0.www;
                tmp6.xyz = v.texcoord1.yyy * float3(999999.6, 999999.6, 999999.6) + tmp6.xyz;
                tmp6.xyz = _Time.www * _Speed.xxx + tmp6.xyz;
                tmp6.xyz = sin(tmp6.xyz);
                tmp6.xyz = tmp6.xyz + float3(1.0, 1.0, 1.0);
                tmp1.xyz = tmp1.xyz * tmp6.xyz;
                tmp1.xyz = tmp1.xyz * _Amount.xxx;
                tmp5.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp5.xwz;
                tmp6 = tmp5.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp6 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp5.xxxx + tmp6;
                tmp5 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp5.zzzz + tmp6;
                tmp5 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp2.wwww + tmp5;
                tmp0.w = saturate(tmp5.y / _MeltStartHeight);
                tmp0.w = 1.0 - tmp0.w;
                tmp0.w = tmp0.w * tmp0.w;
                tmp6.x = tmp1.w * tmp1.w;
                tmp6.x = rsqrt(tmp6.x);
                tmp1.w = tmp1.w * tmp6.x;
                tmp1.w = tmp1.w * _MeltStrength;
                tmp0.w = tmp0.w * tmp1.w;
                tmp5.xz = v.normal.xz * tmp0.ww + tmp5.xz;
                tmp6 = mul(unity_WorldToObject, tmp5);
                tmp0.w = _Time.w * 6.0;
                tmp5.w = 0.0;
                tmp7.xyz = tmp6.xyz;
                tmp7.w = 0.0;
                for (int i = tmp7.w; i < 8; i += 1) {
                    tmp8.xyz = tmp5.xyz - _ParticlePositions[i];
                    tmp8.x = dot(tmp8.xyz, tmp8.xyz);
                    tmp8.x = sqrt(tmp8.x);
                    tmp8.x = saturate(tmp8.x / _ParticleSizes[i]);
                    tmp8.x = 1.0 - tmp8.x;
                    tmp8.y = tmp8.x * 1.5;
                    tmp8.z = tmp8.x * 9.0 + tmp0.w;
                    tmp8.yz = sin(tmp8.yz);
                    tmp8.z = tmp8.z + 1.0;
                    tmp8.x = tmp8.x * tmp8.z;
                    tmp8.x = tmp8.x * 0.5 + tmp8.y;
                    tmp8.y = tmp8.x * _ParticleAlpha[i];
                    tmp5.w = tmp8.x * _ParticleAlpha[i] + tmp5.w;
                    tmp8.xzw = _ParticlePositions[i] - tmp5.xyz;
                    tmp9.xyz = tmp8.zzz * unity_WorldToObject._m01_m11_m21;
                    tmp9.xyz = unity_WorldToObject._m00_m10_m20 * tmp8.xxx + tmp9.xyz;
                    tmp8.xzw = unity_WorldToObject._m02_m12_m22 * tmp8.www + tmp9.xyz;
                    tmp8.xzw = tmp8.xzw + v.normal.xyz;
                    tmp8.xzw = tmp8.xzw * _BulgeStrength.xxx;
                    tmp7.xyz = tmp8.xzw * tmp8.yyy + tmp7.xyz;
                }
                o.texcoord5.y = tmp5.w;
                tmp5 = tmp7.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp5 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp7.xxxx + tmp5;
                tmp5 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp7.zzzz + tmp5;
                tmp5 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp6.wwww + tmp5;
                tmp5.y = max(tmp5.y, 0.01);
                tmp5.y = min(tmp5.y, 100000000.0);
                tmp6 = tmp5.yyyy * unity_WorldToObject._m01_m11_m21_m31;
                tmp6 = unity_WorldToObject._m00_m10_m20_m30 * tmp5.xxxx + tmp6;
                tmp6 = unity_WorldToObject._m02_m12_m22_m32 * tmp5.zzzz + tmp6;
                tmp5 = unity_WorldToObject._m03_m13_m23_m33 * tmp5.wwww + tmp6;
                tmp6 = v.tangent * _Distance.xxxx + tmp2;
                tmp7.xyz = tmp4.xyz * tmp3.xyz + tmp6.xyz;
                tmp7.w = tmp3.w * tmp4.w + tmp7.y;
                tmp6.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp7.xwz;
                tmp7 = tmp6.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp7 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp6.xxxx + tmp7;
                tmp7 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp6.zzzz + tmp7;
                tmp6 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp6.wwww + tmp7;
                tmp7.x = saturate(tmp6.y / _MeltStartHeight);
                tmp7.x = 1.0 - tmp7.x;
                tmp7.x = tmp7.x * tmp7.x;
                tmp7.x = tmp1.w * tmp7.x;
                tmp6.xz = v.normal.xz * tmp7.xx + tmp6.xz;
                tmp7 = mul(unity_WorldToObject, tmp6);
                tmp8.xyz = tmp7.xyz;
                tmp6.w = 0.0;
                for (int j = tmp6.w; j < 8; j += 1) {
                    tmp9.xyz = tmp6.xyz - _ParticlePositions[j];
                    tmp8.w = dot(tmp9.xyz, tmp9.xyz);
                    tmp8.w = sqrt(tmp8.w);
                    tmp8.w = saturate(tmp8.w / _ParticleSizes[j]);
                    tmp8.w = 1.0 - tmp8.w;
                    tmp9.x = tmp8.w * 1.5;
                    tmp9.y = tmp8.w * 9.0 + tmp0.w;
                    tmp9.xy = sin(tmp9.xy);
                    tmp9.y = tmp9.y + 1.0;
                    tmp8.w = tmp8.w * tmp9.y;
                    tmp8.w = tmp8.w * 0.5 + tmp9.x;
                    tmp8.w = tmp8.w * _ParticleAlpha[j];
                    tmp9.xyz = _ParticlePositions[j] - tmp6.xyz;
                    tmp10.xyz = tmp9.yyy * unity_WorldToObject._m01_m11_m21;
                    tmp9.xyw = unity_WorldToObject._m00_m10_m20 * tmp9.xxx + tmp10.xyz;
                    tmp9.xyz = unity_WorldToObject._m02_m12_m22 * tmp9.zzz + tmp9.xyw;
                    tmp9.xyz = tmp9.xyz + v.normal.xyz;
                    tmp9.xyz = tmp9.xyz * _BulgeStrength.xxx;
                    tmp8.xyz = tmp9.xyz * tmp8.www + tmp8.xyz;
                }
                tmp6 = tmp8.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp6 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp8.xxxx + tmp6;
                tmp6 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp8.zzzz + tmp6;
                tmp6 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp7.wwww + tmp6;
                tmp6.y = max(tmp6.y, 0.01);
                tmp6.y = min(tmp6.y, 100000000.0);
                tmp7.xyz = tmp6.yyy * unity_WorldToObject._m21_m01_m11;
                tmp7.xyz = unity_WorldToObject._m20_m00_m10 * tmp6.xxx + tmp7.xyz;
                tmp6.xyz = unity_WorldToObject._m22_m02_m12 * tmp6.zzz + tmp7.xyz;
                tmp6.xyz = unity_WorldToObject._m23_m03_m13 * tmp6.www + tmp6.xyz;
                tmp7.xyz = tmp0.xyz * _Distance.xxx;
                tmp7.w = 0.0;
                tmp2 = tmp2 + tmp7;
                tmp7.xyz = tmp4.xyz * tmp3.xyz + tmp2.xyz;
                tmp7.w = tmp3.w * tmp4.w + tmp7.y;
                tmp0.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp7.xwz;
                tmp3 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp3 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp3;
                tmp3 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp3;
                tmp2 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                tmp0.x = saturate(tmp2.y / _MeltStartHeight);
                tmp0.x = 1.0 - tmp0.x;
                tmp0.x = tmp0.x * tmp0.x;
                tmp0.x = tmp1.w * tmp0.x;
                tmp2.xz = v.normal.xz * tmp0.xx + tmp2.xz;
                tmp1 = mul(unity_WorldToObject, tmp2);
                tmp0.xyz = tmp1.xyz;
                tmp2.w = 0.0;
                for (int k = tmp2.w; k < 8; k += 1) {
                    tmp3.xyz = tmp2.xyz - _ParticlePositions[k];
                    tmp3.x = dot(tmp3.xyz, tmp3.xyz);
                    tmp3.x = sqrt(tmp3.x);
                    tmp3.x = saturate(tmp3.x / _ParticleSizes[k]);
                    tmp3.x = 1.0 - tmp3.x;
                    tmp3.y = tmp3.x * 1.5;
                    tmp3.z = tmp3.x * 9.0 + tmp0.w;
                    tmp3.yz = sin(tmp3.yz);
                    tmp3.z = tmp3.z + 1.0;
                    tmp3.x = tmp3.x * tmp3.z;
                    tmp3.x = tmp3.x * 0.5 + tmp3.y;
                    tmp3.x = tmp3.x * _ParticleAlpha[k];
                    tmp3.yzw = _ParticlePositions[k] - tmp2.xyz;
                    tmp4.xyz = tmp3.zzz * unity_WorldToObject._m01_m11_m21;
                    tmp4.xyz = unity_WorldToObject._m00_m10_m20 * tmp3.yyy + tmp4.xyz;
                    tmp3.yzw = unity_WorldToObject._m02_m12_m22 * tmp3.www + tmp4.xyz;
                    tmp3.yzw = tmp3.yzw + v.normal.xyz;
                    tmp3.yzw = tmp3.yzw * _BulgeStrength.xxx;
                    tmp0.xyz = tmp3.yzw * tmp3.xxx + tmp0.xyz;
                }
                tmp2 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                tmp0.y = max(tmp0.y, 0.01);
                tmp0.y = min(tmp0.y, 100000000.0);
                tmp1.xyz = tmp0.yyy * unity_WorldToObject._m11_m21_m01;
                tmp1.xyz = unity_WorldToObject._m10_m20_m00 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_WorldToObject._m12_m22_m02 * tmp0.zzz + tmp1.xyz;
                tmp0.xyz = unity_WorldToObject._m13_m23_m03 * tmp0.www + tmp0.xyz;
                tmp1.xyz = tmp6.xyz - tmp5.zxy;
                tmp0.xyz = tmp0.xyz - tmp5.yzx;
                tmp2.xyz = tmp0.xyz * tmp1.xyz;
                tmp0.xyz = tmp1.zxy * tmp0.yzx + -tmp2.xyz;
                tmp1 = mul(unity_ObjectToWorld, tmp5);
                tmp2 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp3 = tmp2 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp4 = tmp3.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp4 = unity_MatrixVP._m00_m10_m20_m30 * tmp3.xxxx + tmp4;
                tmp4 = unity_MatrixVP._m02_m12_m22_m32 * tmp3.zzzz + tmp4;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp3.wwww + tmp4;
                o.texcoord.xy = v.texcoord.xy * _CutoffMask_ST.xy + _CutoffMask_ST.zw;
                o.texcoord2.xyz = unity_ObjectToWorld._m03_m13_m23 * tmp1.www + tmp2.xyz;
                tmp1.x = dot(tmp0.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(tmp0.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(tmp0.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp1 = tmp0.yzzx * tmp0.xyzz;
                tmp2.x = dot(unity_SHBr, tmp1);
                tmp2.y = dot(unity_SHBg, tmp1);
                tmp2.z = dot(unity_SHBb, tmp1);
                tmp0.w = tmp0.y * tmp0.y;
                tmp0.w = tmp0.x * tmp0.x + -tmp0.w;
                o.texcoord8.xyz = unity_SHC.xyz * tmp0.www + tmp2.xyz;
                o.texcoord5.x = v.texcoord2.z;
                o.texcoord3 = v.color;
                o.texcoord4.xy = v.texcoord1.zw;
                o.texcoord4.zw = v.texcoord2.xy;
                o.texcoord7 = float4(0.0, 0.0, 0.0, 0.0);
                o.texcoord1.xyz = tmp0.xyz;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = inp.texcoord.xy + _Time.zz;
                tmp0 = tex2D(_CutoffMask, tmp0.xy);
                tmp0.x = tmp0.y < inp.texcoord5.x;
                if (tmp0.x) {
                    discard;
                }
                tmp0.xyz = inp.texcoord3.xyz * _Color.xyz;
                tmp1 = tex2D(_CutoffMask, inp.texcoord.xy);
                tmp1.xyz = tmp1.xyz * inp.texcoord4.xyz;
                tmp1.xyz = tmp1.xyz * inp.texcoord5.yyy + tmp1.xyz;
                tmp2.xyz = inp.texcoord1.xyz;
                tmp2.w = 1.0;
                tmp3.x = dot(unity_SHAr, tmp2);
                tmp3.y = dot(unity_SHAg, tmp2);
                tmp3.z = dot(unity_SHAb, tmp2);
                tmp2.xyz = tmp3.xyz + inp.texcoord8.xyz;
                tmp2.xyz = max(tmp2.xyz, float3(0.0, 0.0, 0.0));
                tmp2.xyz = log(tmp2.xyz);
                tmp2.xyz = tmp2.xyz * float3(0.4166667, 0.4166667, 0.4166667);
                tmp2.xyz = exp(tmp2.xyz);
                tmp2.xyz = tmp2.xyz * float3(1.055, 1.055, 1.055) + float3(-0.055, -0.055, -0.055);
                tmp2.xyz = max(tmp2.xyz, float3(0.0, 0.0, 0.0));
                tmp3.xyz = _Color.xyz * inp.texcoord3.xyz + float3(-0.2209163, -0.2209163, -0.2209163);
                o.sv_target1.xyz = _Metallic.xxx * tmp3.xyz + float3(0.2209163, 0.2209163, 0.2209163);
                tmp0.w = -_Metallic * 0.7790837 + 0.7790837;
                tmp0.xyz = tmp0.www * tmp0.xyz;
                tmp1.xyz = tmp2.xyz * tmp0.xyz + tmp1.xyz;
                o.sv_target3.xyz = exp(-tmp1.xyz);
                o.sv_target.xyz = tmp0.xyz;
                o.sv_target.w = 1.0;
                o.sv_target1.w = _Glossiness;
                o.sv_target2.xyz = inp.texcoord1.xyz * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5);
                o.sv_target2.w = 1.0;
                o.sv_target3.w = 1.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "ShadowCaster"
			LOD 200
			Tags { "LIGHTMODE" = "SHADOWCASTER" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			GpuProgramID 247764
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord5 : TEXCOORD5;
				float3 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _CollapseStrength;
			float _OffsetStrength;
			float _Gravity;
			float4 _Tiling;
			float2 _NoiseSpeed;
			float3 _ParticlePositions[8];
			float _ParticleSizes[8];
			float _ParticleAlpha[8];
			float _BulgeStrength;
			float _Speed;
			float _Amount;
			float _Distance;
			float _MeltStrength;
			float _MeltStartHeight;
			float4 _CutoffMask_ST;
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			// Texture params for Fragment Shader
			sampler2D _CutoffMask;
			
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
                float4 tmp7;
                float4 tmp8;
                float4 tmp9;
                float4 tmp10;
                tmp0.xyz = v.tangent.yzx * v.normal.zxy;
                tmp0.xyz = v.normal.yzx * v.tangent.zxy + -tmp0.xyz;
                tmp1.xy = v.texcoord.xy + v.texcoord1.yy;
                tmp1 = tex2Dlod(_MainTex, float4(tmp1.xy, 0, 0.0), 0);
                tmp0.w = tmp1.x + tmp1.x;
                tmp1.xy = v.texcoord.zw;
                tmp1.z = v.texcoord1.x;
                tmp1.xyz = v.vertex.xyz - tmp1.xyz;
                tmp1.w = v.texcoord1.y * 999999.6;
                tmp2 = mul(unity_WorldToObject, v.vertex);
                tmp3.xy = _Time.yy * _NoiseSpeed + _Tiling.xy;
                tmp3.xy = v.texcoord.xy * _Tiling.zw + tmp3.xy;
                tmp3 = tex2Dlod(_NoiseTex, float4(tmp3.xy, 0, 0.0), 0);
                tmp3.xyz = saturate(tmp3.xyz + _OffsetStrength.xxx);
                tmp3.w = dot(-tmp1.xyz, -tmp1.xyz);
                tmp3.w = rsqrt(tmp3.w);
                tmp4.xyz = -tmp1.xyz * tmp3.www;
                tmp4.xyz = tmp4.xyz * _CollapseStrength.xxx;
                tmp5.xyz = tmp4.xyz * tmp3.xyz + tmp2.xyz;
                tmp3.w = max(_CollapseStrength, 0.0);
                tmp3.w = min(tmp3.w, _CollapseStrength);
                tmp3.w = tmp3.w * _Gravity;
                tmp4.w = dot(tmp1.xyz, tmp1.xyz);
                tmp4.w = sqrt(tmp4.w);
                tmp5.w = tmp3.w * tmp4.w + tmp5.y;
                tmp6.xyz = tmp1.xyz * float3(9.0, 9.0, 9.0) + tmp0.www;
                tmp6.xyz = v.texcoord1.yyy * float3(999999.6, 999999.6, 999999.6) + tmp6.xyz;
                tmp6.xyz = _Time.www * _Speed.xxx + tmp6.xyz;
                tmp6.xyz = sin(tmp6.xyz);
                tmp6.xyz = tmp6.xyz + float3(1.0, 1.0, 1.0);
                tmp1.xyz = tmp1.xyz * tmp6.xyz;
                tmp1.xyz = tmp1.xyz * _Amount.xxx;
                tmp5.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp5.xwz;
                tmp6 = tmp5.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp6 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp5.xxxx + tmp6;
                tmp5 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp5.zzzz + tmp6;
                tmp5 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp2.wwww + tmp5;
                tmp0.w = saturate(tmp5.y / _MeltStartHeight);
                tmp0.w = 1.0 - tmp0.w;
                tmp0.w = tmp0.w * tmp0.w;
                tmp6.x = tmp1.w * tmp1.w;
                tmp6.x = rsqrt(tmp6.x);
                tmp1.w = tmp1.w * tmp6.x;
                tmp1.w = tmp1.w * _MeltStrength;
                tmp0.w = tmp0.w * tmp1.w;
                tmp5.xz = v.normal.xz * tmp0.ww + tmp5.xz;
                tmp6 = mul(unity_WorldToObject, tmp5);
                tmp0.w = _Time.w * 6.0;
                tmp5.w = 0.0;
                tmp7.xyz = tmp6.xyz;
                tmp7.w = 0.0;
                for (int i = tmp7.w; i < 8; i += 1) {
                    tmp8.xyz = tmp5.xyz - _ParticlePositions[i];
                    tmp8.x = dot(tmp8.xyz, tmp8.xyz);
                    tmp8.x = sqrt(tmp8.x);
                    tmp8.x = saturate(tmp8.x / _ParticleSizes[i]);
                    tmp8.x = 1.0 - tmp8.x;
                    tmp8.y = tmp8.x * 1.5;
                    tmp8.z = tmp8.x * 9.0 + tmp0.w;
                    tmp8.yz = sin(tmp8.yz);
                    tmp8.z = tmp8.z + 1.0;
                    tmp8.x = tmp8.x * tmp8.z;
                    tmp8.x = tmp8.x * 0.5 + tmp8.y;
                    tmp8.y = tmp8.x * _ParticleAlpha[i];
                    tmp5.w = tmp8.x * _ParticleAlpha[i] + tmp5.w;
                    tmp8.xzw = _ParticlePositions[i] - tmp5.xyz;
                    tmp9.xyz = tmp8.zzz * unity_WorldToObject._m01_m11_m21;
                    tmp9.xyz = unity_WorldToObject._m00_m10_m20 * tmp8.xxx + tmp9.xyz;
                    tmp8.xzw = unity_WorldToObject._m02_m12_m22 * tmp8.www + tmp9.xyz;
                    tmp8.xzw = tmp8.xzw + v.normal.xyz;
                    tmp8.xzw = tmp8.xzw * _BulgeStrength.xxx;
                    tmp7.xyz = tmp8.xzw * tmp8.yyy + tmp7.xyz;
                }
                o.texcoord5.y = tmp5.w;
                tmp5 = tmp7.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp5 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp7.xxxx + tmp5;
                tmp5 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp7.zzzz + tmp5;
                tmp5 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp6.wwww + tmp5;
                tmp5.y = max(tmp5.y, 0.01);
                tmp5.y = min(tmp5.y, 100000000.0);
                tmp6 = tmp5.yyyy * unity_WorldToObject._m01_m11_m21_m31;
                tmp6 = unity_WorldToObject._m00_m10_m20_m30 * tmp5.xxxx + tmp6;
                tmp6 = unity_WorldToObject._m02_m12_m22_m32 * tmp5.zzzz + tmp6;
                tmp5 = unity_WorldToObject._m03_m13_m23_m33 * tmp5.wwww + tmp6;
                tmp6 = v.tangent * _Distance.xxxx + tmp2;
                tmp7.xyz = tmp4.xyz * tmp3.xyz + tmp6.xyz;
                tmp7.w = tmp3.w * tmp4.w + tmp7.y;
                tmp6.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp7.xwz;
                tmp7 = tmp6.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp7 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp6.xxxx + tmp7;
                tmp7 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp6.zzzz + tmp7;
                tmp6 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp6.wwww + tmp7;
                tmp7.x = saturate(tmp6.y / _MeltStartHeight);
                tmp7.x = 1.0 - tmp7.x;
                tmp7.x = tmp7.x * tmp7.x;
                tmp7.x = tmp1.w * tmp7.x;
                tmp6.xz = v.normal.xz * tmp7.xx + tmp6.xz;
                tmp7 = mul(unity_WorldToObject, tmp6);
                tmp8.xyz = tmp7.xyz;
                tmp6.w = 0.0;
                for (int j = tmp6.w; j < 8; j += 1) {
                    tmp9.xyz = tmp6.xyz - _ParticlePositions[j];
                    tmp8.w = dot(tmp9.xyz, tmp9.xyz);
                    tmp8.w = sqrt(tmp8.w);
                    tmp8.w = saturate(tmp8.w / _ParticleSizes[j]);
                    tmp8.w = 1.0 - tmp8.w;
                    tmp9.x = tmp8.w * 1.5;
                    tmp9.y = tmp8.w * 9.0 + tmp0.w;
                    tmp9.xy = sin(tmp9.xy);
                    tmp9.y = tmp9.y + 1.0;
                    tmp8.w = tmp8.w * tmp9.y;
                    tmp8.w = tmp8.w * 0.5 + tmp9.x;
                    tmp8.w = tmp8.w * _ParticleAlpha[j];
                    tmp9.xyz = _ParticlePositions[j] - tmp6.xyz;
                    tmp10.xyz = tmp9.yyy * unity_WorldToObject._m01_m11_m21;
                    tmp9.xyw = unity_WorldToObject._m00_m10_m20 * tmp9.xxx + tmp10.xyz;
                    tmp9.xyz = unity_WorldToObject._m02_m12_m22 * tmp9.zzz + tmp9.xyw;
                    tmp9.xyz = tmp9.xyz + v.normal.xyz;
                    tmp9.xyz = tmp9.xyz * _BulgeStrength.xxx;
                    tmp8.xyz = tmp9.xyz * tmp8.www + tmp8.xyz;
                }
                tmp6 = tmp8.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp6 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp8.xxxx + tmp6;
                tmp6 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp8.zzzz + tmp6;
                tmp6 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp7.wwww + tmp6;
                tmp6.y = max(tmp6.y, 0.01);
                tmp6.y = min(tmp6.y, 100000000.0);
                tmp7.xyz = tmp6.yyy * unity_WorldToObject._m21_m01_m11;
                tmp7.xyz = unity_WorldToObject._m20_m00_m10 * tmp6.xxx + tmp7.xyz;
                tmp6.xyz = unity_WorldToObject._m22_m02_m12 * tmp6.zzz + tmp7.xyz;
                tmp6.xyz = unity_WorldToObject._m23_m03_m13 * tmp6.www + tmp6.xyz;
                tmp7.xyz = tmp0.xyz * _Distance.xxx;
                tmp7.w = 0.0;
                tmp2 = tmp2 + tmp7;
                tmp7.xyz = tmp4.xyz * tmp3.xyz + tmp2.xyz;
                tmp7.w = tmp3.w * tmp4.w + tmp7.y;
                tmp0.xyz = tmp1.xyz * float3(0.5, 0.5, 0.5) + tmp7.xwz;
                tmp3 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp3 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp3;
                tmp3 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp3;
                tmp2 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                tmp0.x = saturate(tmp2.y / _MeltStartHeight);
                tmp0.x = 1.0 - tmp0.x;
                tmp0.x = tmp0.x * tmp0.x;
                tmp0.x = tmp1.w * tmp0.x;
                tmp2.xz = v.normal.xz * tmp0.xx + tmp2.xz;
                tmp1 = mul(unity_WorldToObject, tmp2);
                tmp0.xyz = tmp1.xyz;
                tmp2.w = 0.0;
                for (int k = tmp2.w; k < 8; k += 1) {
                    tmp3.xyz = tmp2.xyz - _ParticlePositions[k];
                    tmp3.x = dot(tmp3.xyz, tmp3.xyz);
                    tmp3.x = sqrt(tmp3.x);
                    tmp3.x = saturate(tmp3.x / _ParticleSizes[k]);
                    tmp3.x = 1.0 - tmp3.x;
                    tmp3.y = tmp3.x * 1.5;
                    tmp3.z = tmp3.x * 9.0 + tmp0.w;
                    tmp3.yz = sin(tmp3.yz);
                    tmp3.z = tmp3.z + 1.0;
                    tmp3.x = tmp3.x * tmp3.z;
                    tmp3.x = tmp3.x * 0.5 + tmp3.y;
                    tmp3.x = tmp3.x * _ParticleAlpha[k];
                    tmp3.yzw = _ParticlePositions[k] - tmp2.xyz;
                    tmp4.xyz = tmp3.zzz * unity_WorldToObject._m01_m11_m21;
                    tmp4.xyz = unity_WorldToObject._m00_m10_m20 * tmp3.yyy + tmp4.xyz;
                    tmp3.yzw = unity_WorldToObject._m02_m12_m22 * tmp3.www + tmp4.xyz;
                    tmp3.yzw = tmp3.yzw + v.normal.xyz;
                    tmp3.yzw = tmp3.yzw * _BulgeStrength.xxx;
                    tmp0.xyz = tmp3.yzw * tmp3.xxx + tmp0.xyz;
                }
                tmp2 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                tmp0.y = max(tmp0.y, 0.01);
                tmp0.y = min(tmp0.y, 100000000.0);
                tmp1.xyz = tmp0.yyy * unity_WorldToObject._m11_m21_m01;
                tmp1.xyz = unity_WorldToObject._m10_m20_m00 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_WorldToObject._m12_m22_m02 * tmp0.zzz + tmp1.xyz;
                tmp0.xyz = unity_WorldToObject._m13_m23_m03 * tmp0.www + tmp0.xyz;
                tmp1.xyz = tmp6.xyz - tmp5.zxy;
                tmp0.xyz = tmp0.xyz - tmp5.yzx;
                tmp2.xyz = tmp0.xyz * tmp1.xyz;
                tmp0.xyz = tmp1.zxy * tmp0.yzx + -tmp2.xyz;
                tmp1 = mul(unity_ObjectToWorld, tmp5);
                o.texcoord1.xy = v.texcoord.xy * _CutoffMask_ST.xy + _CutoffMask_ST.zw;
                tmp2.xyz = tmp1.yyy * unity_ObjectToWorld._m01_m11_m21;
                tmp2.xyz = unity_ObjectToWorld._m00_m10_m20 * tmp1.xxx + tmp2.xyz;
                tmp2.xyz = unity_ObjectToWorld._m02_m12_m22 * tmp1.zzz + tmp2.xyz;
                o.texcoord2.xyz = unity_ObjectToWorld._m03_m13_m23 * tmp1.www + tmp2.xyz;
                tmp2 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp1 = unity_ObjectToWorld._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                tmp0.w = unity_LightShadowBias.z != 0.0;
                tmp2.x = dot(tmp0.xyz, unity_WorldToObject._m00_m10_m20);
                tmp2.y = dot(tmp0.xyz, unity_WorldToObject._m01_m11_m21);
                tmp2.z = dot(tmp0.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.x = dot(tmp2.xyz, tmp2.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.xyz = tmp0.xxx * tmp2.xyz;
                tmp2.xyz = -tmp1.xyz * _WorldSpaceLightPos0.www + _WorldSpaceLightPos0.xyz;
                tmp2.w = dot(tmp2.xyz, tmp2.xyz);
                tmp2.w = rsqrt(tmp2.w);
                tmp2.xyz = tmp2.www * tmp2.xyz;
                tmp2.x = dot(tmp0.xyz, tmp2.xyz);
                tmp2.x = -tmp2.x * tmp2.x + 1.0;
                tmp2.x = sqrt(tmp2.x);
                tmp2.x = tmp2.x * unity_LightShadowBias.z;
                tmp0.xyz = -tmp0.xyz * tmp2.xxx + tmp1.xyz;
                tmp0.xyz = tmp0.www ? tmp0.xyz : tmp1.xyz;
                tmp2 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                tmp1.x = unity_LightShadowBias.x / tmp0.w;
                tmp1.x = min(tmp1.x, 0.0);
                tmp1.x = max(tmp1.x, -1.0);
                tmp0.z = tmp0.z + tmp1.x;
                tmp1.x = min(tmp0.w, tmp0.z);
                tmp1.x = tmp1.x - tmp0.z;
                o.position.z = unity_LightShadowBias.y * tmp1.x + tmp0.z;
                o.position.xyw = tmp0.xyw;
                o.texcoord5.x = v.texcoord2.z;
                o.texcoord3 = v.color;
                o.texcoord4.xy = v.texcoord1.zw;
                o.texcoord4.zw = v.texcoord2.xy;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0.xy = inp.texcoord1.xy + _Time.zz;
                tmp0 = tex2D(_CutoffMask, tmp0.xy);
                tmp0.x = tmp0.y < inp.texcoord5.x;
                if (tmp0.x) {
                    discard;
                }
                o.sv_target = float4(0.0, 0.0, 0.0, 0.0);
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}