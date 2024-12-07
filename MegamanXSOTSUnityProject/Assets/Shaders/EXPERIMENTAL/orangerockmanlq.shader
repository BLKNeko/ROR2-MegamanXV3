Shader "Orange/Model/OrangeRockmanLQ" {
	Properties {
		[TCP2HeaderHelp(BASE, Base Properties)] _Color ("Color", Vector) = (1,1,1,1)
		_HColor ("Highlight Color", Vector) = (0.785,0.785,0.785,1)
		_SColor ("Shadow Color", Vector) = (0.195,0.195,0.195,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		[TCP2Separator] [TCP2Header(RAMP SETTINGS)] _RampThreshold ("Ramp Threshold", Range(0, 1)) = 0.5
		_RampSmooth ("Ramp Smoothing", Range(0.001, 1)) = 0.1
		[TCP2Separator] [Header(Masks)] [NoScaleOffset] _Mask2 ("Mask 2 (Emission)", 2D) = "black" {}
		[TCP2Separator] [TCP2HeaderHelp(Intensity, Intensity)] _Intensity ("Intensity (Emission)", Range(0, 10)) = 2
		[TCP2Separator] [TCP2HeaderHelp(SPECULAR, Specular)] _SpecColor ("Specular Color", Vector) = (0.5,0.5,0.5,1)
		_Smoothness ("Smoothness", Range(0, 1)) = 0.336
		_SpecSmooth ("SpecSmooth", Range(0, 1)) = 1
		_GradientMax ("Gradient Max", Range(0, 1)) = 0.8
		_SpecColorTex ("Specular Color Texture", 2D) = "white" {}
		[TCP2Separator] [TCP2HeaderHelp(RIM, Rim)] _RimColor ("Rim Color", Vector) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0, 1)) = 0.57
		_RimMax ("Rim Max", Range(0, 1)) = 0.98
		_RimDir ("Rim Direction", Vector) = (0,0,1,0)
		[TCP2Separator] [TCP2HeaderHelp(TRANSPARENCY)] [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendTCP2 ("Blending Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendTCP2 ("Blending Dest", Float) = 10
		[TCP2Separator] [TCP2HeaderHelp(DISSOLVE)] [NoScaleOffset] _DissolveMap ("Dissolve Map", 2D) = "white" {}
		_DissolveValue ("Dissolve Value", Range(0, 1)) = 0.5
		[TCP2Gradient] _DissolveRamp ("Dissolve Ramp", 2D) = "white" {}
		_DissolveGradientWidth ("Ramp Width", Range(0, 1)) = 0.7
		[HDR] _DissolveEdge ("Dissolve Edge", Vector) = (0,1,0,1)
		_DissolveEdgeOffset ("Dissolve Edge Width", Range(0, 1)) = 0.1
		_DissolveModelHeight ("Dissolve Model Height", Range(0, 10)) = 1
		[TCP2Separator] [HideInInspector] __dummy__ ("unused", Float) = 0
	}
	SubShader {
		Tags { "FORCENOSHADOWCASTING" = "true" "IGNOREPROJECTOR" = "true" "QUEUE" = "AlphaTest" "RenderType" = "Opaque" }
		Pass {
			Name "FORWARD"
			Tags { "FORCENOSHADOWCASTING" = "true" "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "AlphaTest" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			Blend Zero Zero, Zero Zero
			Stencil {
				Pass Replace
				Fail Keep
				ZFail Keep
			}
			GpuProgramID 24873
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
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float _DissolveValue;
			float _DissolveGradientWidth;
			float4 _DissolveEdge;
			float _DissolveEdgeOffset;
			float _DissolveModelHeight;
			float4 _Color;
			float _Intensity;
			float _Smoothness;
			float4 _RimColor;
			float _RimMin;
			float _RimMax;
			float4 _RimDir;
			float4 _HColor;
			float4 _SColor;
			float _RampThreshold;
			float _RampSmooth;
			float _SpecSmooth;
			float _GradientMax;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _Mask2;
			sampler2D _SpecColorTex;
			sampler2D _DissolveMap;
			sampler2D _DissolveRamp;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp0.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                o.texcoord1.xyz = tmp0.www * tmp1.xyz;
                o.texcoord2.xyz = tmp0.xyz;
                o.texcoord3.xyz = _WorldSpaceCameraPos - tmp0.xyz;
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
                tmp0.x = _DissolveValue > 0.1;
                tmp0.x = tmp0.x ? 1.0 : 0.0;
                tmp0.y = inp.texcoord2.y - unity_ObjectToWorld._m13;
                tmp0.y = tmp0.y + _DissolveModelHeight;
                tmp0.y = tmp0.y * _DissolveValue;
                tmp0.z = _DissolveGradientWidth + _DissolveEdgeOffset;
                tmp0.z = tmp0.z + 1.0;
                tmp0.y = tmp0.y * tmp0.z + -_DissolveGradientWidth;
                tmp1 = tex2D(_DissolveMap, inp.texcoord.xy);
                tmp0.z = tmp1.x - tmp0.y;
                tmp2 = tex2D(_MainTex, inp.texcoord.xy);
                tmp3 = tmp2 * _Color;
                tmp0.z = tmp3.w * tmp0.z + -tmp3.w;
                tmp0.x = tmp0.x * tmp0.z + tmp3.w;
                tmp0.z = tmp0.x + _DissolveEdgeOffset;
                o.sv_target.w = tmp0.x;
                tmp0.x = tmp0.z < 0.0;
                if (tmp0.x) {
                    discard;
                }
                tmp0.x = tmp1.x + _DissolveGradientWidth;
                tmp0.z = tmp1.x - _DissolveGradientWidth;
                tmp0.xy = tmp0.xy - tmp0.zz;
                tmp0.x = 1.0 / tmp0.x;
                tmp0.x = saturate(tmp0.x * tmp0.y);
                tmp0.y = tmp0.x * -2.0 + 3.0;
                tmp0.x = tmp0.x * tmp0.x;
                tmp0.x = tmp0.x * tmp0.y;
                tmp1 = tex2D(_DissolveRamp, tmp0.xx);
                tmp0.x = tmp0.x * 3.0;
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp0.w = _DissolveValue <= 0.0;
                tmp1.xyz = tmp0.www ? float3(0.0, 0.0, 0.0) : _DissolveEdge.xyz;
                tmp0.xyz = tmp0.xyz * tmp1.xyz;
                tmp1 = tex2D(_Mask2, inp.texcoord.xy);
                tmp1.xyz = tmp1.www * tmp2.xyz;
                tmp2.xyz = -tmp2.xyz * _Color.xyz + _RimColor.xyz;
                tmp0.xyz = tmp1.xyz * _Intensity.xxx + tmp0.xyz;
                tmp1.x = _RimDir.x * unity_MatrixV._m00;
                tmp1.y = _RimDir.x * unity_MatrixV._m01;
                tmp1.z = _RimDir.x * unity_MatrixV._m02;
                tmp4.x = _RimDir.y * unity_MatrixV._m10;
                tmp4.y = _RimDir.y * unity_MatrixV._m11;
                tmp4.z = _RimDir.y * unity_MatrixV._m12;
                tmp1.xyz = tmp1.xyz + tmp4.xyz;
                tmp4.x = _RimDir.z * unity_MatrixV._m20;
                tmp4.y = _RimDir.z * unity_MatrixV._m21;
                tmp4.z = _RimDir.z * unity_MatrixV._m22;
                tmp1.xyz = tmp1.xyz + tmp4.xyz;
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp0.w = saturate(dot(tmp1.xyz, inp.texcoord1.xyz));
                tmp0.w = 1.0 - tmp0.w;
                tmp0.w = tmp0.w - _RimMin;
                tmp1.x = _RimMax - _RimMin;
                tmp1.x = 1.0 / tmp1.x;
                tmp0.w = saturate(tmp0.w * tmp1.x);
                tmp1.x = tmp0.w * -2.0 + 3.0;
                tmp0.w = tmp0.w * tmp0.w;
                tmp0.w = tmp0.w * tmp1.x;
                tmp0.w = tmp0.w * _RimColor.w;
                tmp1.xyz = tmp0.www * tmp2.xyz + tmp3.xyz;
                tmp1.xyz = tmp1.xyz * _LightColor0.xyz;
                tmp0.w = dot(inp.texcoord3.xyz, inp.texcoord3.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = inp.texcoord3.xyz * tmp0.www + _WorldSpaceLightPos0.xyz;
                tmp0.w = dot(tmp2.xyz, tmp2.xyz);
                tmp0.w = max(tmp0.w, 0.001);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp0.www * tmp2.xyz;
                tmp0.w = dot(inp.texcoord1.xyz, inp.texcoord1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp3.xyz = tmp0.www * inp.texcoord1.xyz;
                tmp0.w = saturate(dot(tmp3.xyz, tmp2.xyz));
                tmp1.w = dot(tmp3.xyz, _WorldSpaceLightPos0.xyz);
                tmp1.w = max(tmp1.w, 0.0);
                tmp2.x = 1.0 - _Smoothness;
                tmp3 = tex2D(_SpecColorTex, inp.texcoord.xy);
                tmp2.x = tmp2.x * tmp3.w;
                tmp2.yzw = tmp3.www * tmp3.xyz;
                tmp2.yzw = tmp2.yzw * _LightColor0.xyz;
                tmp2.x = tmp2.x * tmp2.x;
                tmp2.x = min(tmp2.x, 1.0);
                tmp2.x = tmp2.x * tmp2.x;
                tmp3.x = tmp0.w * tmp2.x + -tmp0.w;
                tmp0.w = tmp3.x * tmp0.w + 1.0;
                tmp0.w = tmp0.w * tmp0.w + 0.0000001;
                tmp2.x = tmp2.x * 0.3183099;
                tmp0.w = tmp2.x / tmp0.w;
                tmp0.w = tmp0.w * 0.1570796;
                tmp0.w = max(tmp0.w, 0.0001);
                tmp0.w = sqrt(tmp0.w);
                tmp2.x = -_SpecSmooth * 0.5 + 0.5;
                tmp0.w = tmp0.w * tmp1.w + -tmp2.x;
                tmp3.x = _SpecSmooth * 0.5 + 0.5;
                tmp2.x = tmp3.x - tmp2.x;
                tmp2.x = 1.0 / tmp2.x;
                tmp0.w = saturate(tmp0.w * tmp2.x);
                tmp2.x = tmp0.w * -2.0 + 3.0;
                tmp0.w = tmp0.w * tmp0.w;
                tmp3.x = tmp2.x * tmp0.w + -_GradientMax;
                tmp0.w = tmp0.w * tmp2.x;
                tmp2.x = tmp3.x > 0.0;
                tmp3.x = tmp3.x < 0.0;
                tmp2.x = tmp3.x - tmp2.x;
                tmp2.x = floor(tmp2.x);
                tmp0.w = max(tmp0.w, tmp2.x);
                tmp2.xyz = tmp0.www * tmp2.yzw;
                tmp0.w = -_RampSmooth * 0.5 + _RampThreshold;
                tmp1.w = tmp1.w - tmp0.w;
                tmp2.w = _RampSmooth * 0.5 + _RampThreshold;
                tmp0.w = tmp2.w - tmp0.w;
                tmp0.w = 1.0 / tmp0.w;
                tmp0.w = saturate(tmp0.w * tmp1.w);
                tmp1.w = tmp0.w * -2.0 + 3.0;
                tmp0.w = tmp0.w * tmp0.w;
                tmp0.w = tmp0.w * tmp1.w;
                tmp3.xyz = _SColor.xyz - _HColor.xyz;
                tmp3.xyz = _SColor.www * tmp3.xyz + _HColor.xyz;
                tmp4.xyz = _HColor.xyz - tmp3.xyz;
                tmp3.xyz = tmp0.www * tmp4.xyz + tmp3.xyz;
                tmp1.xyz = tmp1.xyz * tmp3.xyz + tmp2.xyz;
                o.sv_target.xyz = tmp0.xyz + tmp1.xyz;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}