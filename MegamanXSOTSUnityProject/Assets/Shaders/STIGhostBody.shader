Shader "STI/GhostBody" {
	Properties {
		_Albedo ("Albedo", 2D) = "white" {}
		_V_Position ("V_Position", Range(0, 1)) = 0.7907612
		_FresnelPower ("Fresnel Power", Range(1, 10)) = 2.4
		_FresnelScale ("Fresnel Scale", Range(1, 10)) = 5
		_DistrotionStrength ("Distrotion Strength", Range(0, 1)) = 0.15
		_VOffset ("V Offset", Range(-1, 1)) = 0
		_VAppearence ("V Appearence", Range(1, 5)) = 3
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "IsEmissive" = "true" "QUEUE" = "Transparent+0" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "IsEmissive" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent+0" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask RGB -1
			ZWrite Off
			GpuProgramID 8582
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
				float4 texcoord6 : TEXCOORD6;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _DistrotionStrength;
			float4 _texcoord_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Albedo_ST;
			float _FresnelScale;
			float _FresnelPower;
			float _VOffset;
			float _V_Position;
			float _VAppearence;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _Albedo;
			
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
                tmp0.xz = float2(0.0, 1.0);
                tmp1.xy = _Time.yy * float2(0.0, 2.0) + v.vertex.xy;
                tmp1.zw = tmp1.xy + tmp1.xy;
                tmp0.w = dot(tmp1.xy, float2(0.3660254, 0.3660254));
                tmp1.zw = tmp1.xy * float2(2.0, 2.0) + tmp0.ww;
                tmp1.zw = floor(tmp1.zw);
                tmp2.xy = tmp1.zw * float2(0.0034602, 0.0034602);
                tmp2.xy = floor(tmp2.xy);
                tmp2.xy = -tmp2.xy * float2(289.0, 289.0) + tmp1.zw;
                tmp1.xy = tmp1.xy * float2(2.0, 2.0) + -tmp1.zw;
                tmp0.w = dot(tmp1.xy, float2(0.2113249, 0.2113249));
                tmp1.xy = tmp0.ww + tmp1.xy;
                tmp0.w = tmp1.y < tmp1.x;
                tmp3 = tmp0.wwww ? float4(1.0, 0.0, -1.0, -0.0) : float4(0.0, 1.0, -0.0, -1.0);
                tmp0.y = tmp3.y;
                tmp0.xyz = tmp0.xyz + tmp2.yyy;
                tmp2.yzw = tmp0.xyz * float3(34.0, 34.0, 34.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * tmp2.yzw;
                tmp2.yzw = tmp0.xyz * float3(0.0034602, 0.0034602, 0.0034602);
                tmp2.yzw = floor(tmp2.yzw);
                tmp0.xyz = -tmp2.yzw * float3(289.0, 289.0, 289.0) + tmp0.xyz;
                tmp0.xyz = tmp2.xxx + tmp0.xyz;
                tmp2.xz = float2(0.0, 1.0);
                tmp2.y = tmp3.x;
                tmp0.xyz = tmp0.xyz + tmp2.xyz;
                tmp2.xyz = tmp0.xyz * float3(34.0, 34.0, 34.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * tmp2.xyz;
                tmp2.xyz = tmp0.xyz * float3(0.0034602, 0.0034602, 0.0034602);
                tmp2.xyz = floor(tmp2.xyz);
                tmp0.xyz = -tmp2.xyz * float3(289.0, 289.0, 289.0) + tmp0.xyz;
                tmp0.xyz = tmp0.xyz * float3(0.0243902, 0.0243902, 0.0243902);
                tmp0.xyz = frac(tmp0.xyz);
                tmp2.xyz = tmp0.xyz * float3(2.0, 2.0, 2.0) + float3(-0.5, -0.5, -0.5);
                tmp0.xyz = tmp0.xyz * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0);
                tmp2.xyz = floor(tmp2.xyz);
                tmp2.xyz = tmp0.xyz - tmp2.xyz;
                tmp0.xyz = abs(tmp0.xyz) - float3(0.5, 0.5, 0.5);
                tmp4.xyz = tmp0.xyz * tmp0.xyz;
                tmp4.xyz = tmp2.xyz * tmp2.xyz + tmp4.xyz;
                tmp4.xyz = -tmp4.xyz * float3(0.8537347, 0.8537347, 0.8537347) + float3(1.792843, 1.792843, 1.792843);
                tmp5.x = dot(tmp1.xy, tmp1.xy);
                tmp6 = tmp1.xyxy + float4(0.2113249, 0.2113249, -0.5773503, -0.5773503);
                tmp6.xy = tmp3.zw + tmp6.xy;
                tmp5.y = dot(tmp6.xy, tmp6.xy);
                tmp5.z = dot(tmp6.xy, tmp6.xy);
                tmp3.xyz = float3(0.5, 0.5, 0.5) - tmp5.xyz;
                tmp3.xyz = max(tmp3.xyz, float3(0.0, 0.0, 0.0));
                tmp3.xyz = tmp3.xyz * tmp3.xyz;
                tmp3.xyz = tmp3.xyz * tmp3.xyz;
                tmp3.xyz = tmp4.xyz * tmp3.xyz;
                tmp0.x = tmp1.y * tmp0.x;
                tmp0.yz = tmp0.yz * tmp6.yw;
                tmp4.yz = tmp2.yz * tmp6.xz + tmp0.yz;
                tmp4.x = tmp2.x * tmp1.x + tmp0.x;
                tmp0.x = dot(tmp3.xyz, tmp4.xyz);
                tmp0.x = tmp0.x * 65.0 + 0.5;
                tmp0.x = tmp0.x * _DistrotionStrength;
                tmp0.xyz = tmp0.xxx * float3(0.3333333, 0.3333333, 0.3333333) + v.vertex.xyz;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.texcoord2.xyz = tmp0.xyz;
                o.texcoord.xy = v.texcoord.xy * _texcoord_ST.xy + _texcoord_ST.zw;
                tmp0.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp0.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp0.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                o.texcoord1.xyz = tmp0.www * tmp0.xyz;
                o.texcoord3.xyz = float3(0.0, 0.0, 0.0);
                o.texcoord6 = float4(0.0, 0.0, 0.0, 0.0);
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
                tmp0.xz = float2(0.0, 1.0);
                tmp1.xy = inp.texcoord.xy * float2(8.0, 8.0);
                tmp1.xy = _Time.yy * float2(0.0, 2.0) + tmp1.xy;
                tmp0.w = dot(tmp1.xy, float2(0.3660254, 0.3660254));
                tmp1.zw = tmp0.ww + tmp1.xy;
                tmp1.zw = floor(tmp1.zw);
                tmp2.xy = tmp1.zw * float2(0.0034602, 0.0034602);
                tmp2.xy = floor(tmp2.xy);
                tmp2.xy = -tmp2.xy * float2(289.0, 289.0) + tmp1.zw;
                tmp1.xy = tmp1.xy - tmp1.zw;
                tmp0.w = dot(tmp1.xy, float2(0.2113249, 0.2113249));
                tmp1.xy = tmp0.ww + tmp1.xy;
                tmp0.w = tmp1.y < tmp1.x;
                tmp3 = tmp0.wwww ? float4(1.0, 0.0, -1.0, -0.0) : float4(0.0, 1.0, -0.0, -1.0);
                tmp0.y = tmp3.y;
                tmp0.xyz = tmp0.xyz + tmp2.yyy;
                tmp2.yzw = tmp0.xyz * float3(34.0, 34.0, 34.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * tmp2.yzw;
                tmp2.yzw = tmp0.xyz * float3(0.0034602, 0.0034602, 0.0034602);
                tmp2.yzw = floor(tmp2.yzw);
                tmp0.xyz = -tmp2.yzw * float3(289.0, 289.0, 289.0) + tmp0.xyz;
                tmp0.xyz = tmp2.xxx + tmp0.xyz;
                tmp2.xz = float2(0.0, 1.0);
                tmp2.y = tmp3.x;
                tmp0.xyz = tmp0.xyz + tmp2.xyz;
                tmp2.xyz = tmp0.xyz * float3(34.0, 34.0, 34.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * tmp2.xyz;
                tmp2.xyz = tmp0.xyz * float3(0.0034602, 0.0034602, 0.0034602);
                tmp2.xyz = floor(tmp2.xyz);
                tmp0.xyz = -tmp2.xyz * float3(289.0, 289.0, 289.0) + tmp0.xyz;
                tmp0.xyz = tmp0.xyz * float3(0.0243902, 0.0243902, 0.0243902);
                tmp0.xyz = frac(tmp0.xyz);
                tmp2.xyz = tmp0.xyz * float3(2.0, 2.0, 2.0) + float3(-0.5, -0.5, -0.5);
                tmp0.xyz = tmp0.xyz * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0);
                tmp2.xyz = floor(tmp2.xyz);
                tmp2.xyz = tmp0.xyz - tmp2.xyz;
                tmp0.xyz = abs(tmp0.xyz) - float3(0.5, 0.5, 0.5);
                tmp4.xyz = tmp0.xyz * tmp0.xyz;
                tmp4.xyz = tmp2.xyz * tmp2.xyz + tmp4.xyz;
                tmp4.xyz = -tmp4.xyz * float3(0.8537347, 0.8537347, 0.8537347) + float3(1.792843, 1.792843, 1.792843);
                tmp5.x = dot(tmp1.xy, tmp1.xy);
                tmp6 = tmp1.xyxy + float4(0.2113249, 0.2113249, -0.5773503, -0.5773503);
                tmp6.xy = tmp3.zw + tmp6.xy;
                tmp5.y = dot(tmp6.xy, tmp6.xy);
                tmp5.z = dot(tmp6.xy, tmp6.xy);
                tmp3.xyz = float3(0.5, 0.5, 0.5) - tmp5.xyz;
                tmp3.xyz = max(tmp3.xyz, float3(0.0, 0.0, 0.0));
                tmp3.xyz = tmp3.xyz * tmp3.xyz;
                tmp3.xyz = tmp3.xyz * tmp3.xyz;
                tmp3.xyz = tmp4.xyz * tmp3.xyz;
                tmp0.x = tmp1.y * tmp0.x;
                tmp0.yz = tmp0.yz * tmp6.yw;
                tmp4.yz = tmp2.yz * tmp6.xz + tmp0.yz;
                tmp4.x = tmp2.x * tmp1.x + tmp0.x;
                tmp0.x = dot(tmp3.xyz, tmp4.xyz);
                tmp0.x = tmp0.x * 65.0 + 0.5;
                tmp0.y = inp.texcoord.y + _VOffset;
                tmp0.y = tmp0.y - _V_Position;
                tmp0.x = tmp0.y - tmp0.x;
                o.sv_target.w = saturate(tmp0.y * _VAppearence + tmp0.x);
                tmp0.xzw = _WorldSpaceCameraPos - inp.texcoord2.xyz;
                tmp1.x = dot(tmp0.xyz, tmp0.xyz);
                tmp1.x = rsqrt(tmp1.x);
                tmp0.xzw = tmp0.xzw * tmp1.xxx;
                tmp0.x = dot(inp.texcoord1.xyz, tmp0.xyz);
                tmp0.x = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.z = _SinTime.w + 1.0;
                tmp0.z = tmp0.z * 0.15 + 0.7;
                tmp0.z = dot(float2(_FresnelScale.x, _FresnelPower.x), tmp0.xy);
                tmp0.x = tmp0.x * tmp0.z;
                tmp0.x = exp(tmp0.x);
                tmp0.x = tmp0.x * _FresnelScale;
                tmp0.zw = inp.texcoord.xy * _Albedo_ST.xy + _Albedo_ST.zw;
                tmp1 = tex2D(_Albedo, tmp0.zw);
                o.sv_target.xyz = tmp0.xxx * tmp0.yyy + tmp1.xyz;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "IsEmissive" = "true" "LIGHTMODE" = "FORWARDADD" "QUEUE" = "Transparent+0" "RenderType" = "Transparent" }
			Blend SrcAlpha One, SrcAlpha One
			ColorMask RGB -1
			ZWrite Off
			GpuProgramID 89590
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float3 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 unity_WorldToLight;
			float _DistrotionStrength;
			float4 _texcoord_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _VOffset;
			float _V_Position;
			float _VAppearence;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			
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
                tmp0.xz = float2(0.0, 1.0);
                tmp1.xy = _Time.yy * float2(0.0, 2.0) + v.vertex.xy;
                tmp1.zw = tmp1.xy + tmp1.xy;
                tmp0.w = dot(tmp1.xy, float2(0.3660254, 0.3660254));
                tmp1.zw = tmp1.xy * float2(2.0, 2.0) + tmp0.ww;
                tmp1.zw = floor(tmp1.zw);
                tmp2.xy = tmp1.zw * float2(0.0034602, 0.0034602);
                tmp2.xy = floor(tmp2.xy);
                tmp2.xy = -tmp2.xy * float2(289.0, 289.0) + tmp1.zw;
                tmp1.xy = tmp1.xy * float2(2.0, 2.0) + -tmp1.zw;
                tmp0.w = dot(tmp1.xy, float2(0.2113249, 0.2113249));
                tmp1.xy = tmp0.ww + tmp1.xy;
                tmp0.w = tmp1.y < tmp1.x;
                tmp3 = tmp0.wwww ? float4(1.0, 0.0, -1.0, -0.0) : float4(0.0, 1.0, -0.0, -1.0);
                tmp0.y = tmp3.y;
                tmp0.xyz = tmp0.xyz + tmp2.yyy;
                tmp2.yzw = tmp0.xyz * float3(34.0, 34.0, 34.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * tmp2.yzw;
                tmp2.yzw = tmp0.xyz * float3(0.0034602, 0.0034602, 0.0034602);
                tmp2.yzw = floor(tmp2.yzw);
                tmp0.xyz = -tmp2.yzw * float3(289.0, 289.0, 289.0) + tmp0.xyz;
                tmp0.xyz = tmp2.xxx + tmp0.xyz;
                tmp2.xz = float2(0.0, 1.0);
                tmp2.y = tmp3.x;
                tmp0.xyz = tmp0.xyz + tmp2.xyz;
                tmp2.xyz = tmp0.xyz * float3(34.0, 34.0, 34.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * tmp2.xyz;
                tmp2.xyz = tmp0.xyz * float3(0.0034602, 0.0034602, 0.0034602);
                tmp2.xyz = floor(tmp2.xyz);
                tmp0.xyz = -tmp2.xyz * float3(289.0, 289.0, 289.0) + tmp0.xyz;
                tmp0.xyz = tmp0.xyz * float3(0.0243902, 0.0243902, 0.0243902);
                tmp0.xyz = frac(tmp0.xyz);
                tmp2.xyz = tmp0.xyz * float3(2.0, 2.0, 2.0) + float3(-0.5, -0.5, -0.5);
                tmp0.xyz = tmp0.xyz * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0);
                tmp2.xyz = floor(tmp2.xyz);
                tmp2.xyz = tmp0.xyz - tmp2.xyz;
                tmp0.xyz = abs(tmp0.xyz) - float3(0.5, 0.5, 0.5);
                tmp4.xyz = tmp0.xyz * tmp0.xyz;
                tmp4.xyz = tmp2.xyz * tmp2.xyz + tmp4.xyz;
                tmp4.xyz = -tmp4.xyz * float3(0.8537347, 0.8537347, 0.8537347) + float3(1.792843, 1.792843, 1.792843);
                tmp5.x = dot(tmp1.xy, tmp1.xy);
                tmp6 = tmp1.xyxy + float4(0.2113249, 0.2113249, -0.5773503, -0.5773503);
                tmp6.xy = tmp3.zw + tmp6.xy;
                tmp5.y = dot(tmp6.xy, tmp6.xy);
                tmp5.z = dot(tmp6.xy, tmp6.xy);
                tmp3.xyz = float3(0.5, 0.5, 0.5) - tmp5.xyz;
                tmp3.xyz = max(tmp3.xyz, float3(0.0, 0.0, 0.0));
                tmp3.xyz = tmp3.xyz * tmp3.xyz;
                tmp3.xyz = tmp3.xyz * tmp3.xyz;
                tmp3.xyz = tmp4.xyz * tmp3.xyz;
                tmp0.x = tmp1.y * tmp0.x;
                tmp0.yz = tmp0.yz * tmp6.yw;
                tmp4.yz = tmp2.yz * tmp6.xz + tmp0.yz;
                tmp4.x = tmp2.x * tmp1.x + tmp0.x;
                tmp0.x = dot(tmp3.xyz, tmp4.xyz);
                tmp0.x = tmp0.x * 65.0 + 0.5;
                tmp0.x = tmp0.x * _DistrotionStrength;
                tmp0.xyz = tmp0.xxx * float3(0.3333333, 0.3333333, 0.3333333) + v.vertex.xyz;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp0 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.texcoord.xy = v.texcoord.xy * _texcoord_ST.xy + _texcoord_ST.zw;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                o.texcoord1.xyz = tmp1.www * tmp1.xyz;
                o.texcoord2.xyz = tmp0.xyz;
                tmp1.xyz = tmp0.yyy * unity_WorldToLight._m01_m11_m21;
                tmp1.xyz = unity_WorldToLight._m00_m10_m20 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_WorldToLight._m02_m12_m22 * tmp0.zzz + tmp1.xyz;
                o.texcoord3.xyz = unity_WorldToLight._m03_m13_m23 * tmp0.www + tmp0.xyz;
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
                tmp0.xz = float2(0.0, 1.0);
                tmp1.xy = inp.texcoord.xy * float2(8.0, 8.0);
                tmp1.xy = _Time.yy * float2(0.0, 2.0) + tmp1.xy;
                tmp0.w = dot(tmp1.xy, float2(0.3660254, 0.3660254));
                tmp1.zw = tmp0.ww + tmp1.xy;
                tmp1.zw = floor(tmp1.zw);
                tmp2.xy = tmp1.zw * float2(0.0034602, 0.0034602);
                tmp2.xy = floor(tmp2.xy);
                tmp2.xy = -tmp2.xy * float2(289.0, 289.0) + tmp1.zw;
                tmp1.xy = tmp1.xy - tmp1.zw;
                tmp0.w = dot(tmp1.xy, float2(0.2113249, 0.2113249));
                tmp1.xy = tmp0.ww + tmp1.xy;
                tmp0.w = tmp1.y < tmp1.x;
                tmp3 = tmp0.wwww ? float4(1.0, 0.0, -1.0, -0.0) : float4(0.0, 1.0, -0.0, -1.0);
                tmp0.y = tmp3.y;
                tmp0.xyz = tmp0.xyz + tmp2.yyy;
                tmp2.yzw = tmp0.xyz * float3(34.0, 34.0, 34.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * tmp2.yzw;
                tmp2.yzw = tmp0.xyz * float3(0.0034602, 0.0034602, 0.0034602);
                tmp2.yzw = floor(tmp2.yzw);
                tmp0.xyz = -tmp2.yzw * float3(289.0, 289.0, 289.0) + tmp0.xyz;
                tmp0.xyz = tmp2.xxx + tmp0.xyz;
                tmp2.xz = float2(0.0, 1.0);
                tmp2.y = tmp3.x;
                tmp0.xyz = tmp0.xyz + tmp2.xyz;
                tmp2.xyz = tmp0.xyz * float3(34.0, 34.0, 34.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * tmp2.xyz;
                tmp2.xyz = tmp0.xyz * float3(0.0034602, 0.0034602, 0.0034602);
                tmp2.xyz = floor(tmp2.xyz);
                tmp0.xyz = -tmp2.xyz * float3(289.0, 289.0, 289.0) + tmp0.xyz;
                tmp0.xyz = tmp0.xyz * float3(0.0243902, 0.0243902, 0.0243902);
                tmp0.xyz = frac(tmp0.xyz);
                tmp2.xyz = tmp0.xyz * float3(2.0, 2.0, 2.0) + float3(-0.5, -0.5, -0.5);
                tmp0.xyz = tmp0.xyz * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0);
                tmp2.xyz = floor(tmp2.xyz);
                tmp2.xyz = tmp0.xyz - tmp2.xyz;
                tmp0.xyz = abs(tmp0.xyz) - float3(0.5, 0.5, 0.5);
                tmp4.xyz = tmp0.xyz * tmp0.xyz;
                tmp4.xyz = tmp2.xyz * tmp2.xyz + tmp4.xyz;
                tmp4.xyz = -tmp4.xyz * float3(0.8537347, 0.8537347, 0.8537347) + float3(1.792843, 1.792843, 1.792843);
                tmp5.x = dot(tmp1.xy, tmp1.xy);
                tmp6 = tmp1.xyxy + float4(0.2113249, 0.2113249, -0.5773503, -0.5773503);
                tmp6.xy = tmp3.zw + tmp6.xy;
                tmp5.y = dot(tmp6.xy, tmp6.xy);
                tmp5.z = dot(tmp6.xy, tmp6.xy);
                tmp3.xyz = float3(0.5, 0.5, 0.5) - tmp5.xyz;
                tmp3.xyz = max(tmp3.xyz, float3(0.0, 0.0, 0.0));
                tmp3.xyz = tmp3.xyz * tmp3.xyz;
                tmp3.xyz = tmp3.xyz * tmp3.xyz;
                tmp3.xyz = tmp4.xyz * tmp3.xyz;
                tmp0.x = tmp1.y * tmp0.x;
                tmp0.yz = tmp0.yz * tmp6.yw;
                tmp4.yz = tmp2.yz * tmp6.xz + tmp0.yz;
                tmp4.x = tmp2.x * tmp1.x + tmp0.x;
                tmp0.x = dot(tmp3.xyz, tmp4.xyz);
                tmp0.x = tmp0.x * 65.0 + 0.5;
                tmp0.y = inp.texcoord.y + _VOffset;
                tmp0.y = tmp0.y - _V_Position;
                tmp0.x = tmp0.y - tmp0.x;
                o.sv_target.w = saturate(tmp0.y * _VAppearence + tmp0.x);
                o.sv_target.xyz = float3(0.0, 0.0, 0.0);
                return o;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}