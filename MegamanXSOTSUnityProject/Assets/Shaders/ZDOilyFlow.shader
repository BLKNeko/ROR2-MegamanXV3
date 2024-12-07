Shader "ZD/OilyFlow" {
	Properties {
		_SpecColor ("Specular Color", Color) = (1,1,1,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_BumpMap ("BumpMap", 2D) = "white" {}
		_BumpScale ("BumpScale", Float) = 1
		_Specular ("Specular", Range(0, 1)) = 0
		_Gloss ("Gloss", Range(0, 10)) = 0
		_WaveIntansity ("WaveIntansity", Range(0, 1)) = 1
		_WaveExp ("WaveExp", Float) = 3
		_WaveRepeat ("WaveRepeat", Float) = 2
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Geometry+0" "RenderType" = "Opaque" }
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Geometry+0" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 25030
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord6 : TEXCOORD6;
				float4 texcoord7 : TEXCOORD7;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _WaveRepeat;
			float _WaveExp;
			float _WaveIntansity;
			float4 _texcoord_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float4 _SpecColor;
			float4 _BumpMap_ST;
			float _BumpScale;
			float4 _MainTex_ST;
			float _Specular;
			float _Gloss;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _MainTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = float2(0.5, 0.5) - v.texcoord.xy;
                tmp0.xy = tmp0.xy + tmp0.xy;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * _WaveRepeat + -_Time.y;
                tmp0.x = frac(abs(tmp0.x));
                tmp0.y = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _WaveExp;
                tmp0.x = exp(tmp0.x);
                tmp0.y = log(tmp0.y);
                tmp0.y = tmp0.y * _WaveExp;
                tmp0.y = exp(tmp0.y);
                tmp0.x = tmp0.y + tmp0.x;
                tmp0.x = tmp0.x - 0.3;
                tmp0.x = tmp0.x * 1.428571;
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.y = dot(tmp1.xyz, tmp1.xyz);
                tmp0.y = rsqrt(tmp0.y);
                tmp0.yzw = tmp0.yyy * tmp1.xyz;
                tmp1.xyz = tmp0.xxx * tmp0.zwy;
                tmp0.x = _WaveIntansity * 0.1;
                tmp1.xyz = tmp1.xyz * tmp0.xxx + v.vertex.xyz;
                tmp2 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp1 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp2 = tmp1 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp1.xyz;
                tmp3 = tmp2.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp3 = unity_MatrixVP._m00_m10_m20_m30 * tmp2.xxxx + tmp3;
                tmp3 = unity_MatrixVP._m02_m12_m22_m32 * tmp2.zzzz + tmp3;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                o.texcoord.xy = v.texcoord.xy * _texcoord_ST.xy + _texcoord_ST.zw;
                o.texcoord1.w = tmp1.x;
                tmp2.xyz = v.tangent.yyy * unity_ObjectToWorld._m11_m21_m01;
                tmp2.xyz = unity_ObjectToWorld._m10_m20_m00 * v.tangent.xxx + tmp2.xyz;
                tmp2.xyz = unity_ObjectToWorld._m12_m22_m02 * v.tangent.zzz + tmp2.xyz;
                tmp0.x = dot(tmp2.xyz, tmp2.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp2.xyz = tmp0.xxx * tmp2.xyz;
                tmp3.xyz = tmp0.yzw * tmp2.xyz;
                tmp3.xyz = tmp0.wyz * tmp2.yzx + -tmp3.xyz;
                tmp0.x = v.tangent.w * unity_WorldTransformParams.w;
                tmp3.xyz = tmp0.xxx * tmp3.xyz;
                o.texcoord1.y = tmp3.x;
                o.texcoord1.z = tmp0.z;
                o.texcoord1.x = tmp2.z;
                o.texcoord2.w = tmp1.y;
                o.texcoord3.w = tmp1.z;
                o.texcoord2.x = tmp2.x;
                o.texcoord3.x = tmp2.y;
                o.texcoord2.z = tmp0.w;
                o.texcoord3.z = tmp0.y;
                o.texcoord2.y = tmp3.y;
                o.texcoord3.y = tmp3.z;
                o.texcoord6 = float4(0.0, 0.0, 0.0, 0.0);
                o.texcoord7 = float4(0.0, 0.0, 0.0, 0.0);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.xy = float2(0.5, 0.5) - inp.texcoord.xy;
                tmp0.xy = tmp0.xy + tmp0.xy;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * _WaveRepeat + -_Time.y;
                tmp0.x = frac(abs(tmp0.x));
                tmp0.y = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _WaveExp;
                tmp0.x = exp(tmp0.x);
                tmp0.y = log(tmp0.y);
                tmp0.y = tmp0.y * _WaveExp;
                tmp0.y = exp(tmp0.y);
                tmp0.x = tmp0.y + tmp0.x;
                tmp0.x = tmp0.x - 0.3;
                tmp0.x = tmp0.x * 1.428571;
                tmp1.x = inp.texcoord1.z;
                tmp1.y = inp.texcoord2.z;
                tmp1.z = inp.texcoord3.z;
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp1.xy = _WaveIntansity.xx * float2(0.1, 100.0);
                tmp0.xyz = tmp0.xyz * tmp1.xxx;
                tmp1.xz = inp.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                tmp2 = tex2D(_BumpMap, tmp1.xz);
                tmp2.x = tmp2.w * tmp2.x;
                tmp1.xz = tmp2.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp2.xy = tmp1.xz * _BumpScale.xx;
                tmp0.w = dot(tmp2.xy, tmp2.xy);
                tmp0.w = min(tmp0.w, 1.0);
                tmp0.w = 1.0 - tmp0.w;
                tmp2.z = sqrt(tmp0.w);
                tmp0.xyz = -tmp1.yyy * tmp0.xyz + tmp2.xyz;
                tmp1.x = dot(inp.texcoord1.xyz, tmp0.xyz);
                tmp1.y = dot(inp.texcoord2.xyz, tmp0.xyz);
                tmp1.z = dot(inp.texcoord3.xyz, tmp0.xyz);
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp1.x = inp.texcoord1.w;
                tmp1.y = inp.texcoord2.w;
                tmp1.z = inp.texcoord3.w;
                tmp1.xyz = _WorldSpaceCameraPos - tmp1.xyz;
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp1.xyz * tmp0.www + _WorldSpaceLightPos0.xyz;
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp0.w = dot(tmp0.xyz, tmp1.xyz);
                tmp0.x = dot(tmp0.xyz, _WorldSpaceLightPos0.xyz);
                tmp0.xy = max(tmp0.xw, float2(0.0, 0.0));
                tmp0.y = log(tmp0.y);
                tmp0.z = _Specular * 128.0;
                tmp0.y = tmp0.y * tmp0.z;
                tmp0.y = exp(tmp0.y);
                tmp0.z = _Gloss * _Gloss;
                tmp0.z = tmp0.z * _Specular;
                tmp0.y = tmp0.z * tmp0.y;
                tmp1.xyz = _LightColor0.xyz * _SpecColor.xyz;
                tmp0.yzw = tmp0.yyy * tmp1.xyz;
                tmp1.xy = inp.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp1.xyz = tmp1.xyz * _LightColor0.xyz;
                o.sv_target.xyz = tmp1.xyz * tmp0.xxx + tmp0.yzw;
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "PREPASS"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "PREPASSBASE" "QUEUE" = "Geometry+0" "RenderType" = "Opaque" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 65886
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _WaveRepeat;
			float _WaveExp;
			float _WaveIntansity;
			float4 _texcoord_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _BumpMap_ST;
			float _BumpScale;
			float _Specular;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = float2(0.5, 0.5) - v.texcoord.xy;
                tmp0.xy = tmp0.xy + tmp0.xy;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * _WaveRepeat + -_Time.y;
                tmp0.x = frac(abs(tmp0.x));
                tmp0.y = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _WaveExp;
                tmp0.x = exp(tmp0.x);
                tmp0.y = log(tmp0.y);
                tmp0.y = tmp0.y * _WaveExp;
                tmp0.y = exp(tmp0.y);
                tmp0.x = tmp0.y + tmp0.x;
                tmp0.x = tmp0.x - 0.3;
                tmp0.x = tmp0.x * 1.428571;
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.y = dot(tmp1.xyz, tmp1.xyz);
                tmp0.y = rsqrt(tmp0.y);
                tmp0.yzw = tmp0.yyy * tmp1.xyz;
                tmp1.xyz = tmp0.xxx * tmp0.zwy;
                tmp0.x = _WaveIntansity * 0.1;
                tmp1.xyz = tmp1.xyz * tmp0.xxx + v.vertex.xyz;
                tmp2 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp1 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp2 = tmp1 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp1.xyz;
                tmp3 = tmp2.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp3 = unity_MatrixVP._m00_m10_m20_m30 * tmp2.xxxx + tmp3;
                tmp3 = unity_MatrixVP._m02_m12_m22_m32 * tmp2.zzzz + tmp3;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                o.texcoord.xy = v.texcoord.xy * _texcoord_ST.xy + _texcoord_ST.zw;
                o.texcoord1.w = tmp1.x;
                tmp2.xyz = v.tangent.yyy * unity_ObjectToWorld._m11_m21_m01;
                tmp2.xyz = unity_ObjectToWorld._m10_m20_m00 * v.tangent.xxx + tmp2.xyz;
                tmp2.xyz = unity_ObjectToWorld._m12_m22_m02 * v.tangent.zzz + tmp2.xyz;
                tmp0.x = dot(tmp2.xyz, tmp2.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp2.xyz = tmp0.xxx * tmp2.xyz;
                tmp3.xyz = tmp0.yzw * tmp2.xyz;
                tmp3.xyz = tmp0.wyz * tmp2.yzx + -tmp3.xyz;
                tmp0.x = v.tangent.w * unity_WorldTransformParams.w;
                tmp3.xyz = tmp0.xxx * tmp3.xyz;
                o.texcoord1.y = tmp3.x;
                o.texcoord1.z = tmp0.z;
                o.texcoord1.x = tmp2.z;
                o.texcoord2.w = tmp1.y;
                o.texcoord3.w = tmp1.z;
                o.texcoord2.x = tmp2.x;
                o.texcoord3.x = tmp2.y;
                o.texcoord2.z = tmp0.w;
                o.texcoord3.z = tmp0.y;
                o.texcoord2.y = tmp3.y;
                o.texcoord3.y = tmp3.z;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.xy = float2(0.5, 0.5) - inp.texcoord.xy;
                tmp0.xy = tmp0.xy + tmp0.xy;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * _WaveRepeat + -_Time.y;
                tmp0.x = frac(abs(tmp0.x));
                tmp0.y = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _WaveExp;
                tmp0.x = exp(tmp0.x);
                tmp0.y = log(tmp0.y);
                tmp0.y = tmp0.y * _WaveExp;
                tmp0.y = exp(tmp0.y);
                tmp0.x = tmp0.y + tmp0.x;
                tmp0.x = tmp0.x - 0.3;
                tmp0.x = tmp0.x * 1.428571;
                tmp1.x = inp.texcoord1.z;
                tmp1.y = inp.texcoord2.z;
                tmp1.z = inp.texcoord3.z;
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp1.xy = _WaveIntansity.xx * float2(0.1, 100.0);
                tmp0.xyz = tmp0.xyz * tmp1.xxx;
                tmp1.xz = inp.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                tmp2 = tex2D(_BumpMap, tmp1.xz);
                tmp2.x = tmp2.w * tmp2.x;
                tmp1.xz = tmp2.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp2.xy = tmp1.xz * _BumpScale.xx;
                tmp0.w = dot(tmp2.xy, tmp2.xy);
                tmp0.w = min(tmp0.w, 1.0);
                tmp0.w = 1.0 - tmp0.w;
                tmp2.z = sqrt(tmp0.w);
                tmp0.xyz = -tmp1.yyy * tmp0.xyz + tmp2.xyz;
                tmp1.x = dot(inp.texcoord1.xyz, tmp0.xyz);
                tmp1.y = dot(inp.texcoord2.xyz, tmp0.xyz);
                tmp1.z = dot(inp.texcoord3.xyz, tmp0.xyz);
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                o.sv_target.xyz = tmp0.xyz * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5);
                o.sv_target.w = _Specular;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "PREPASS"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "PREPASSFINAL" "QUEUE" = "Geometry+0" "RenderType" = "Opaque" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 168894
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float3 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _WaveRepeat;
			float _WaveExp;
			float _WaveIntansity;
			float4 _texcoord_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _SpecColor;
			float4 _MainTex_ST;
			float _Specular;
			float _Gloss;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _LightBuffer;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = float2(0.5, 0.5) - v.texcoord.xy;
                tmp0.xy = tmp0.xy + tmp0.xy;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * _WaveRepeat + -_Time.y;
                tmp0.x = frac(abs(tmp0.x));
                tmp0.y = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _WaveExp;
                tmp0.x = exp(tmp0.x);
                tmp0.y = log(tmp0.y);
                tmp0.y = tmp0.y * _WaveExp;
                tmp0.y = exp(tmp0.y);
                tmp0.x = tmp0.y + tmp0.x;
                tmp0.x = tmp0.x - 0.3;
                tmp0.x = tmp0.x * 1.428571;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.y = dot(tmp1.xyz, tmp1.xyz);
                tmp0.y = rsqrt(tmp0.y);
                tmp1.xyz = tmp0.yyy * tmp1.xyz;
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp0.w = _WaveIntansity * 0.1;
                tmp0.xyz = tmp0.xyz * tmp0.www + v.vertex.xyz;
                tmp2 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp2;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp2;
                tmp2 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0 = mul(unity_MatrixVP, tmp2);
                o.position = tmp0;
                o.texcoord.xy = v.texcoord.xy * _texcoord_ST.xy + _texcoord_ST.zw;
                tmp0.y = tmp0.y * _ProjectionParams.x;
                tmp2.xzw = tmp0.xwy * float3(0.5, 0.5, 0.5);
                o.texcoord2.zw = tmp0.zw;
                o.texcoord2.xy = tmp2.zz + tmp2.xw;
                o.texcoord3 = float4(0.0, 0.0, 0.0, 0.0);
                tmp0.x = tmp1.y * tmp1.y;
                tmp0.x = tmp1.x * tmp1.x + -tmp0.x;
                tmp2 = tmp1.yzzx * tmp1.xyzz;
                tmp3.x = dot(unity_SHBr, tmp2);
                tmp3.y = dot(unity_SHBg, tmp2);
                tmp3.z = dot(unity_SHBb, tmp2);
                tmp0.xyz = unity_SHC.xyz * tmp0.xxx + tmp3.xyz;
                tmp1.w = 1.0;
                tmp2.x = dot(unity_SHAr, tmp1);
                tmp2.y = dot(unity_SHAg, tmp1);
                tmp2.z = dot(unity_SHAb, tmp1);
                tmp0.xyz = tmp0.xyz + tmp2.xyz;
                tmp0.xyz = max(tmp0.xyz, float3(0.0, 0.0, 0.0));
                tmp0.xyz = log(tmp0.xyz);
                tmp0.xyz = tmp0.xyz * float3(0.4166667, 0.4166667, 0.4166667);
                tmp0.xyz = exp(tmp0.xyz);
                tmp0.xyz = tmp0.xyz * float3(1.055, 1.055, 1.055) + float3(-0.055, -0.055, -0.055);
                o.texcoord4.xyz = max(tmp0.xyz, float3(0.0, 0.0, 0.0));
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.x = _Gloss * _Gloss;
                tmp0.x = tmp0.x * _Specular;
                tmp0.yz = inp.texcoord2.xy / inp.texcoord2.ww;
                tmp1 = tex2D(_LightBuffer, tmp0.yz);
                tmp1 = log(tmp1);
                tmp0.x = tmp0.x * -tmp1.w;
                tmp0.yzw = inp.texcoord4.xyz - tmp1.xyz;
                tmp1.xyz = tmp0.yzw * _SpecColor.xyz;
                tmp1.xyz = tmp0.xxx * tmp1.xyz;
                tmp2.xy = inp.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp2 = tex2D(_MainTex, tmp2.xy);
                o.sv_target.xyz = tmp2.xyz * tmp0.yzw + tmp1.xyz;
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "DEFERRED"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "DEFERRED" "QUEUE" = "Geometry+0" "RenderType" = "Opaque" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 225581
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
				float4 sv_target1 : SV_Target1;
				float4 sv_target2 : SV_Target2;
				float4 sv_target3 : SV_Target3;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _WaveRepeat;
			float _WaveExp;
			float _WaveIntansity;
			float4 _texcoord_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _SpecColor;
			float4 _BumpMap_ST;
			float _BumpScale;
			float4 _MainTex_ST;
			float _Specular;
			float _Gloss;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _MainTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = float2(0.5, 0.5) - v.texcoord.xy;
                tmp0.xy = tmp0.xy + tmp0.xy;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * _WaveRepeat + -_Time.y;
                tmp0.x = frac(abs(tmp0.x));
                tmp0.y = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _WaveExp;
                tmp0.x = exp(tmp0.x);
                tmp0.y = log(tmp0.y);
                tmp0.y = tmp0.y * _WaveExp;
                tmp0.y = exp(tmp0.y);
                tmp0.x = tmp0.y + tmp0.x;
                tmp0.x = tmp0.x - 0.3;
                tmp0.x = tmp0.x * 1.428571;
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.y = dot(tmp1.xyz, tmp1.xyz);
                tmp0.y = rsqrt(tmp0.y);
                tmp0.yzw = tmp0.yyy * tmp1.xyz;
                tmp1.xyz = tmp0.xxx * tmp0.zwy;
                tmp0.x = _WaveIntansity * 0.1;
                tmp1.xyz = tmp1.xyz * tmp0.xxx + v.vertex.xyz;
                tmp2 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp1 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp2 = tmp1 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp1.xyz;
                tmp3 = tmp2.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp3 = unity_MatrixVP._m00_m10_m20_m30 * tmp2.xxxx + tmp3;
                tmp3 = unity_MatrixVP._m02_m12_m22_m32 * tmp2.zzzz + tmp3;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                o.texcoord.xy = v.texcoord.xy * _texcoord_ST.xy + _texcoord_ST.zw;
                o.texcoord1.w = tmp1.x;
                tmp2.xyz = v.tangent.yyy * unity_ObjectToWorld._m11_m21_m01;
                tmp2.xyz = unity_ObjectToWorld._m10_m20_m00 * v.tangent.xxx + tmp2.xyz;
                tmp2.xyz = unity_ObjectToWorld._m12_m22_m02 * v.tangent.zzz + tmp2.xyz;
                tmp0.x = dot(tmp2.xyz, tmp2.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp2.xyz = tmp0.xxx * tmp2.xyz;
                tmp3.xyz = tmp0.yzw * tmp2.xyz;
                tmp3.xyz = tmp0.wyz * tmp2.yzx + -tmp3.xyz;
                tmp0.x = v.tangent.w * unity_WorldTransformParams.w;
                tmp3.xyz = tmp0.xxx * tmp3.xyz;
                o.texcoord1.y = tmp3.x;
                o.texcoord1.z = tmp0.z;
                o.texcoord1.x = tmp2.z;
                o.texcoord2.w = tmp1.y;
                o.texcoord3.w = tmp1.z;
                o.texcoord2.x = tmp2.x;
                o.texcoord3.x = tmp2.y;
                o.texcoord2.z = tmp0.w;
                o.texcoord3.z = tmp0.y;
                o.texcoord2.y = tmp3.y;
                o.texcoord3.y = tmp3.z;
                o.texcoord4 = float4(0.0, 0.0, 0.0, 0.0);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.xy = inp.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                o.sv_target.xyz = tmp0.xyz;
                o.sv_target.w = 1.0;
                tmp0.x = _Gloss * _Gloss;
                tmp0.x = tmp0.x * _Specular;
                tmp0.xyz = tmp0.xxx * _SpecColor.xyz;
                o.sv_target1.xyz = tmp0.xyz * float3(0.3183099, 0.3183099, 0.3183099);
                o.sv_target1.w = _Specular;
                tmp0.xy = float2(0.5, 0.5) - inp.texcoord.xy;
                tmp0.xy = tmp0.xy + tmp0.xy;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * _WaveRepeat + -_Time.y;
                tmp0.x = frac(abs(tmp0.x));
                tmp0.y = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _WaveExp;
                tmp0.x = exp(tmp0.x);
                tmp0.y = log(tmp0.y);
                tmp0.y = tmp0.y * _WaveExp;
                tmp0.y = exp(tmp0.y);
                tmp0.x = tmp0.y + tmp0.x;
                tmp0.x = tmp0.x - 0.3;
                tmp0.x = tmp0.x * 1.428571;
                tmp1.x = inp.texcoord1.z;
                tmp1.y = inp.texcoord2.z;
                tmp1.z = inp.texcoord3.z;
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                tmp1.xy = _WaveIntansity.xx * float2(0.1, 100.0);
                tmp0.xyz = tmp0.xyz * tmp1.xxx;
                tmp1.xz = inp.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                tmp2 = tex2D(_BumpMap, tmp1.xz);
                tmp2.x = tmp2.w * tmp2.x;
                tmp1.xz = tmp2.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp2.xy = tmp1.xz * _BumpScale.xx;
                tmp0.w = dot(tmp2.xy, tmp2.xy);
                tmp0.w = min(tmp0.w, 1.0);
                tmp0.w = 1.0 - tmp0.w;
                tmp2.z = sqrt(tmp0.w);
                tmp0.xyz = -tmp1.yyy * tmp0.xyz + tmp2.xyz;
                tmp1.x = dot(inp.texcoord1.xyz, tmp0.xyz);
                tmp1.y = dot(inp.texcoord2.xyz, tmp0.xyz);
                tmp1.z = dot(inp.texcoord3.xyz, tmp0.xyz);
                tmp0.x = dot(tmp1.xyz, tmp1.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.xyz = tmp0.xxx * tmp1.xyz;
                o.sv_target2.xyz = tmp0.xyz * float3(0.5, 0.5, 0.5) + float3(0.5, 0.5, 0.5);
                o.sv_target2.w = 1.0;
                o.sv_target3 = float4(1.0, 1.0, 1.0, 1.0);
                return o;
			}
			ENDCG
		}
		Pass {
			Name "ShadowCaster"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "SHADOWCASTER" "QUEUE" = "Geometry+0" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 270236
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _WaveRepeat;
			float _WaveExp;
			float _WaveIntansity;
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
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = float2(0.5, 0.5) - v.texcoord.xy;
                tmp0.xy = tmp0.xy + tmp0.xy;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * _WaveRepeat + -_Time.y;
                tmp0.x = frac(abs(tmp0.x));
                tmp0.y = 1.0 - tmp0.x;
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _WaveExp;
                tmp0.x = exp(tmp0.x);
                tmp0.y = log(tmp0.y);
                tmp0.y = tmp0.y * _WaveExp;
                tmp0.y = exp(tmp0.y);
                tmp0.x = tmp0.y + tmp0.x;
                tmp0.x = tmp0.x - 0.3;
                tmp0.x = tmp0.x * 1.428571;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.y = dot(tmp1.xyz, tmp1.xyz);
                tmp0.y = rsqrt(tmp0.y);
                tmp0.yzw = tmp0.yyy * tmp1.xyz;
                tmp1.xyz = tmp0.xxx * tmp0.yzw;
                tmp0.x = _WaveIntansity * 0.1;
                tmp1.xyz = tmp1.xyz * tmp0.xxx + v.vertex.xyz;
                tmp2 = tmp1.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp2 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp2 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp2;
                tmp3.xyz = -tmp2.xyz * _WorldSpaceLightPos0.www + _WorldSpaceLightPos0.xyz;
                tmp0.x = dot(tmp3.xyz, tmp3.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp3.xyz = tmp0.xxx * tmp3.xyz;
                tmp0.x = dot(tmp0.xyz, tmp3.xyz);
                tmp0.x = -tmp0.x * tmp0.x + 1.0;
                tmp0.x = sqrt(tmp0.x);
                tmp0.x = tmp0.x * unity_LightShadowBias.z;
                tmp3.xyz = -tmp0.yzw * tmp0.xxx + tmp2.xyz;
                tmp0.x = unity_LightShadowBias.z != 0.0;
                tmp2.xyz = tmp0.xxx ? tmp3.xyz : tmp2.xyz;
                tmp3 = tmp2.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp3 = unity_MatrixVP._m00_m10_m20_m30 * tmp2.xxxx + tmp3;
                tmp3 = unity_MatrixVP._m02_m12_m22_m32 * tmp2.zzzz + tmp3;
                tmp2 = unity_MatrixVP._m03_m13_m23_m33 * tmp2.wwww + tmp3;
                tmp0.x = unity_LightShadowBias.x / tmp2.w;
                tmp0.x = min(tmp0.x, 0.0);
                tmp0.x = max(tmp0.x, -1.0);
                tmp0.x = tmp0.x + tmp2.z;
                tmp1.w = min(tmp2.w, tmp0.x);
                o.position.xyw = tmp2.xyw;
                tmp1.w = tmp1.w - tmp0.x;
                o.position.z = unity_LightShadowBias.y * tmp1.w + tmp0.x;
                o.texcoord1.xy = v.texcoord.xy;
                tmp2.xyz = tmp1.yyy * unity_ObjectToWorld._m01_m11_m21;
                tmp1.xyw = unity_ObjectToWorld._m00_m10_m20 * tmp1.xxx + tmp2.xyz;
                tmp1.xyz = unity_ObjectToWorld._m02_m12_m22 * tmp1.zzz + tmp1.xyw;
                tmp1.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp1.xyz;
                o.texcoord2.w = tmp1.x;
                tmp2.xyz = v.tangent.yyy * unity_ObjectToWorld._m11_m21_m01;
                tmp2.xyz = unity_ObjectToWorld._m10_m20_m00 * v.tangent.xxx + tmp2.xyz;
                tmp2.xyz = unity_ObjectToWorld._m12_m22_m02 * v.tangent.zzz + tmp2.xyz;
                tmp0.x = dot(tmp2.xyz, tmp2.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp2.xyz = tmp0.xxx * tmp2.xyz;
                tmp3.xyz = tmp0.wyz * tmp2.xyz;
                tmp3.xyz = tmp0.zwy * tmp2.yzx + -tmp3.xyz;
                tmp0.x = v.tangent.w * unity_WorldTransformParams.w;
                tmp3.xyz = tmp0.xxx * tmp3.xyz;
                o.texcoord2.y = tmp3.x;
                o.texcoord2.z = tmp0.y;
                o.texcoord2.x = tmp2.z;
                o.texcoord3.w = tmp1.y;
                o.texcoord4.w = tmp1.z;
                o.texcoord3.x = tmp2.x;
                o.texcoord4.x = tmp2.y;
                o.texcoord3.z = tmp0.z;
                o.texcoord4.z = tmp0.w;
                o.texcoord3.y = tmp3.y;
                o.texcoord4.y = tmp3.z;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                o.sv_target = float4(0.0, 0.0, 0.0, 0.0);
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}