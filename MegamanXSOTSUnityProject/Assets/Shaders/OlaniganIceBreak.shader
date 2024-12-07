Shader "Olanigan/IceBreak" {
	Properties {
		_snow ("snow", 2D) = "white" {}
		_Color ("Color", Color) = (0.5019608,0.5019608,0.5019608,1)
		_Metallic ("Metallic", Range(0, 1)) = 0.6030321
		_Gloss ("Gloss", Range(0, 1)) = 0.3252537
		_BumpMap ("Normal Map I", 2D) = "bump" {}
		_NormalMapII ("Normal Map II", 2D) = "bump" {}
		_snow_slider ("snow_slider", Range(0, 10)) = 7.705339
		_Freezeeffectnormal ("Freeze effect (normal)", Range(0, 10)) = 4.77537
		[MaterialToggle] _LocalGlobal ("Local/Global", Float) = 0
		_Transparency ("Transparency", Range(-1, 1)) = 0
		_Ice_fresnel ("Ice_fresnel", Range(0, 3)) = 0
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass {
		}
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 23624
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
				float3 texcoord5 : TEXCOORD5;
				float3 texcoord6 : TEXCOORD6;
				float4 texcoord7 : TEXCOORD7;
				float4 texcoord9 : TEXCOORD9;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float4 _Color;
			float4 _BumpMap_ST;
			float _Metallic;
			float _Gloss;
			float4 _snow_ST;
			float _snow_slider;
			float4 _NormalMapII_ST;
			float _Freezeeffectnormal;
			float _LocalGlobal;
			float _Transparency;
			float _Ice_fresnel;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _NormalMapII;
			sampler2D _GrabTexture;
			sampler2D _snow;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                float4 tmp4;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord3 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp0 = mul(unity_MatrixVP, tmp1);
                o.position = tmp0;
                o.texcoord.xy = v.texcoord.xy;
                o.texcoord1.xy = v.texcoord1.xy;
                o.texcoord2.xy = v.texcoord2.xy;
                tmp2.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp2.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp2.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.z = dot(tmp2.xyz, tmp2.xyz);
                tmp0.z = rsqrt(tmp0.z);
                tmp2.xyz = tmp0.zzz * tmp2.xyz;
                o.texcoord4.xyz = tmp2.xyz;
                tmp3.xyz = v.tangent.yyy * unity_ObjectToWorld._m01_m11_m21;
                tmp3.xyz = unity_ObjectToWorld._m00_m10_m20 * v.tangent.xxx + tmp3.xyz;
                tmp3.xyz = unity_ObjectToWorld._m02_m12_m22 * v.tangent.zzz + tmp3.xyz;
                tmp0.z = dot(tmp3.xyz, tmp3.xyz);
                tmp0.z = rsqrt(tmp0.z);
                tmp3.xyz = tmp0.zzz * tmp3.xyz;
                o.texcoord5.xyz = tmp3.xyz;
                tmp4.xyz = tmp2.zxy * tmp3.yzx;
                tmp2.xyz = tmp2.yzx * tmp3.zxy + -tmp4.xyz;
                tmp2.xyz = tmp2.xyz * v.tangent.www;
                tmp0.z = dot(tmp2.xyz, tmp2.xyz);
                tmp0.z = rsqrt(tmp0.z);
                o.texcoord6.xyz = tmp0.zzz * tmp2.xyz;
                tmp0.z = tmp1.y * unity_MatrixV._m21;
                tmp0.z = unity_MatrixV._m20 * tmp1.x + tmp0.z;
                tmp0.z = unity_MatrixV._m22 * tmp1.z + tmp0.z;
                tmp0.z = unity_MatrixV._m23 * tmp1.w + tmp0.z;
                o.texcoord7.z = -tmp0.z;
                tmp0.y = tmp0.y * _ProjectionParams.x;
                tmp1.xzw = tmp0.xwy * float3(0.5, 0.5, 0.5);
                o.texcoord7.w = tmp0.w;
                o.texcoord7.xy = tmp1.zz + tmp1.xw;
                o.texcoord9 = float4(0.0, 0.0, 0.0, 0.0);
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
                tmp0.xy = inp.texcoord.xy * _NormalMapII_ST.xy + _NormalMapII_ST.zw;
                tmp0 = tex2D(_NormalMapII, tmp0.xy);
                tmp0.x = tmp0.w * tmp0.x;
                tmp0.xy = tmp0.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.w = dot(tmp0.xy, tmp0.xy);
                tmp0.w = min(tmp0.w, 1.0);
                tmp0.w = 1.0 - tmp0.w;
                tmp0.z = sqrt(tmp0.w);
                tmp1.xy = inp.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                tmp1 = tex2D(_BumpMap, tmp1.xy);
                tmp1.x = tmp1.w * tmp1.x;
                tmp1.xy = tmp1.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.w = dot(tmp1.xy, tmp1.xy);
                tmp0.w = min(tmp0.w, 1.0);
                tmp0.w = 1.0 - tmp0.w;
                tmp1.z = sqrt(tmp0.w);
                tmp0.xyz = tmp0.xyz - tmp1.xyz;
                tmp0.w = dot(inp.texcoord4.xyz, inp.texcoord4.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp0.www * inp.texcoord4.xyz;
                tmp1.w = tmp2.y * unity_WorldToObject._m11;
                tmp1.w = unity_WorldToObject._m10 * tmp2.x + tmp1.w;
                tmp1.w = unity_WorldToObject._m12 * tmp2.z + tmp1.w;
                tmp0.w = inp.texcoord4.y * tmp0.w + -tmp1.w;
                tmp0.w = _LocalGlobal * tmp0.w + tmp1.w;
                tmp0.w = tmp0.w * abs(tmp0.w) + -tmp1.y;
                tmp0.w = saturate(tmp0.w * _snow_slider);
                tmp1.w = max(tmp1.y, 0.0);
                tmp1.w = min(tmp1.w, 0.8);
                tmp1.w = tmp1.w * _Freezeeffectnormal;
                tmp2.w = max(_snow_slider, 0.0);
                tmp2.w = min(tmp2.w, 1.2);
                tmp2.w = tmp2.w * 0.7;
                tmp0.w = tmp2.w * tmp0.w + tmp1.w;
                tmp0.xyz = tmp0.www * tmp0.xyz + tmp1.xyz;
                tmp1.xyz = tmp0.yyy * inp.texcoord6.xyz;
                tmp1.xyz = tmp0.xxx * inp.texcoord5.xyz + tmp1.xyz;
                tmp0.xyz = tmp0.zzz * tmp2.xyz + tmp1.xyz;
                tmp1.x = dot(tmp0.xyz, tmp0.xyz);
                tmp1.x = rsqrt(tmp1.x);
                tmp0.xyz = tmp0.xyz * tmp1.xxx;
                tmp1.xyz = _WorldSpaceCameraPos - inp.texcoord3.xyz;
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp2.xyz = tmp1.www * tmp1.xyz;
                tmp2.w = dot(-tmp2.xyz, tmp0.xyz);
                tmp2.w = tmp2.w + tmp2.w;
                tmp3.xyz = tmp0.xyz * -tmp2.www + -tmp2.xyz;
                tmp2.x = dot(tmp0.xyz, tmp2.xyz);
                tmp2.y = 0.7 - _Gloss;
                tmp2.y = tmp0.w * tmp2.y + _Gloss;
                tmp2.z = 1.0 - tmp2.y;
                tmp2.w = -tmp2.z * 0.7 + 1.7;
                tmp2.w = tmp2.w * tmp2.z;
                tmp2.w = tmp2.w * 6.0;
                tmp3 = UNITY_SAMPLE_TEXCUBE_SAMPLER(unity_SpecCube0, unity_SpecCube0, float4(tmp3.xyz, tmp2.w));
                tmp2.w = tmp3.w - 1.0;
                tmp2.w = unity_SpecCube0_HDR.w * tmp2.w + 1.0;
                tmp2.w = tmp2.w * unity_SpecCube0_HDR.x;
                tmp3.xyz = tmp3.xyz * tmp2.www;
                tmp2.w = tmp0.w * -_Metallic + _Metallic;
                tmp3.w = -tmp2.w * 0.7790837 + 0.7790837;
                tmp4.x = 1.0 - tmp3.w;
                tmp2.y = saturate(tmp2.y + tmp4.x);
                tmp4.xy = inp.texcoord.xy * _snow_ST.xy + _snow_ST.zw;
                tmp4 = tex2D(_snow, tmp4.xy);
                tmp4.xyz = tmp4.xyz - _Color.xyz;
                tmp4.xyz = tmp0.www * tmp4.xyz + _Color.xyz;
                tmp5.xyz = _Color.xyz * tmp4.xyz + float3(-0.2209163, -0.2209163, -0.2209163);
                tmp4.xyz = tmp4.xyz * _Color.xyz;
                tmp4.xyz = tmp3.www * tmp4.xyz;
                tmp5.xyz = tmp2.www * tmp5.xyz + float3(0.2209163, 0.2209163, 0.2209163);
                tmp6.xyz = tmp2.yyy - tmp5.xyz;
                tmp0.w = 1.0 - abs(tmp2.x);
                tmp2.y = tmp0.w * tmp0.w;
                tmp2.y = tmp2.y * tmp2.y;
                tmp0.w = tmp0.w * tmp2.y;
                tmp6.xyz = tmp0.www * tmp6.xyz + tmp5.xyz;
                tmp3.xyz = tmp3.xyz * tmp6.xyz;
                tmp2.y = tmp2.z * tmp2.z;
                tmp2.w = tmp2.y * tmp2.z;
                tmp2.w = -tmp2.w * 0.28 + 1.0;
                tmp3.xyz = tmp2.www * tmp3.xyz;
                tmp2.w = dot(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz);
                tmp2.w = rsqrt(tmp2.w);
                tmp6.xyz = tmp2.www * _WorldSpaceLightPos0.xyz;
                tmp1.xyz = tmp1.xyz * tmp1.www + tmp6.xyz;
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp1.xyz = tmp1.www * tmp1.xyz;
                tmp1.w = saturate(dot(tmp0.xyz, tmp1.xyz));
                tmp0.x = dot(tmp0.xyz, tmp6.xyz);
                tmp0.y = saturate(dot(tmp6.xyz, tmp1.xyz));
                tmp0.x = max(tmp0.x, 0.0);
                tmp0.z = tmp2.y * tmp2.y;
                tmp1.x = tmp1.w * tmp0.z + -tmp1.w;
                tmp1.x = tmp1.x * tmp1.w + 1.0;
                tmp1.x = tmp1.x * tmp1.x + 0.0000001;
                tmp0.z = tmp0.z * 0.3183099;
                tmp0.z = tmp0.z / tmp1.x;
                tmp1.x = -tmp2.z * tmp2.z + 1.0;
                tmp1.y = abs(tmp2.x) * tmp1.x + tmp2.y;
                tmp1.z = min(tmp0.x, 1.0);
                tmp1.x = tmp1.z * tmp1.x + tmp2.y;
                tmp1.x = tmp1.x * abs(tmp2.x);
                tmp1.w = max(tmp2.x, 0.0);
                tmp1.w = 1.0 - tmp1.w;
                tmp1.w = log(tmp1.w);
                tmp1.w = tmp1.w * _Ice_fresnel;
                tmp1.w = exp(tmp1.w);
                tmp1.x = tmp1.z * tmp1.y + tmp1.x;
                tmp1.x = tmp1.x + 0.00001;
                tmp1.x = 0.5 / tmp1.x;
                tmp0.z = tmp0.z * tmp1.x;
                tmp0.z = tmp0.z * 3.141593;
                tmp0.z = max(tmp0.z, 0.0001);
                tmp0.z = sqrt(tmp0.z);
                tmp0.z = tmp1.z * tmp0.z;
                tmp1.x = dot(tmp5.xyz, tmp5.xyz);
                tmp1.x = tmp1.x != 0.0;
                tmp1.x = tmp1.x ? 1.0 : 0.0;
                tmp0.z = tmp0.z * tmp1.x;
                tmp1.xyz = tmp0.zzz * _LightColor0.xyz;
                tmp2.xyw = float3(1.0, 1.0, 1.0) - tmp5.xyz;
                tmp0.z = 1.0 - tmp0.y;
                tmp3.w = tmp0.z * tmp0.z;
                tmp3.w = tmp3.w * tmp3.w;
                tmp0.z = tmp0.z * tmp3.w;
                tmp2.xyw = tmp2.xyw * tmp0.zzz + tmp5.xyz;
                tmp1.xyz = tmp1.xyz * tmp2.xyw + tmp3.xyz;
                tmp0.z = tmp0.y + tmp0.y;
                tmp0.y = tmp0.y * tmp0.z;
                tmp0.y = tmp0.y * tmp2.z + -0.5;
                tmp0.z = tmp0.y * tmp0.w + 1.0;
                tmp0.w = 1.0 - tmp0.x;
                tmp2.x = tmp0.w * tmp0.w;
                tmp2.x = tmp2.x * tmp2.x;
                tmp0.w = tmp0.w * tmp2.x;
                tmp0.y = tmp0.y * tmp0.w + 1.0;
                tmp0.y = tmp0.z * tmp0.y;
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.xyz = tmp0.xxx * _LightColor0.xyz;
                tmp0.xyz = tmp0.xyz * tmp4.xyz + tmp1.xyz;
                tmp1.xy = inp.texcoord7.xy / inp.texcoord7.ww;
                tmp1.xy = inp.texcoord.xy * tmp1.ww + tmp1.xy;
                tmp2 = tex2D(_GrabTexture, tmp1.xy);
                tmp0.xyz = tmp0.xyz - tmp2.xyz;
                tmp0.w = 1.0 - tmp1.w;
                tmp0.w = _Transparency * tmp0.w + tmp1.w;
                o.sv_target.xyz = tmp0.www * tmp0.xyz + tmp2.xyz;
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "FORWARD_DELTA"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDADD" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One One, One One
			GpuProgramID 97833
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
				float3 texcoord5 : TEXCOORD5;
				float3 texcoord6 : TEXCOORD6;
				float4 texcoord7 : TEXCOORD7;
				float3 texcoord8 : TEXCOORD8;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 unity_WorldToLight;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float4 _Color;
			float4 _BumpMap_ST;
			float _Metallic;
			float _Gloss;
			float4 _snow_ST;
			float _snow_slider;
			float4 _NormalMapII_ST;
			float _Freezeeffectnormal;
			float _LocalGlobal;
			float _Transparency;
			float _Ice_fresnel;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _NormalMapII;
			sampler2D _LightTexture0;
			sampler2D _snow;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                float4 tmp4;
                float4 tmp5;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp2 = mul(unity_MatrixVP, tmp1);
                o.position = tmp2;
                o.texcoord.xy = v.texcoord.xy;
                o.texcoord1.xy = v.texcoord1.xy;
                o.texcoord2.xy = v.texcoord2.xy;
                o.texcoord3 = tmp0;
                tmp3.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp3.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp3.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp2.z = dot(tmp3.xyz, tmp3.xyz);
                tmp2.z = rsqrt(tmp2.z);
                tmp3.xyz = tmp2.zzz * tmp3.xyz;
                o.texcoord4.xyz = tmp3.xyz;
                tmp4.xyz = v.tangent.yyy * unity_ObjectToWorld._m01_m11_m21;
                tmp4.xyz = unity_ObjectToWorld._m00_m10_m20 * v.tangent.xxx + tmp4.xyz;
                tmp4.xyz = unity_ObjectToWorld._m02_m12_m22 * v.tangent.zzz + tmp4.xyz;
                tmp2.z = dot(tmp4.xyz, tmp4.xyz);
                tmp2.z = rsqrt(tmp2.z);
                tmp4.xyz = tmp2.zzz * tmp4.xyz;
                o.texcoord5.xyz = tmp4.xyz;
                tmp5.xyz = tmp3.zxy * tmp4.yzx;
                tmp3.xyz = tmp3.yzx * tmp4.zxy + -tmp5.xyz;
                tmp3.xyz = tmp3.xyz * v.tangent.www;
                tmp2.z = dot(tmp3.xyz, tmp3.xyz);
                tmp2.z = rsqrt(tmp2.z);
                o.texcoord6.xyz = tmp2.zzz * tmp3.xyz;
                tmp1.y = tmp1.y * unity_MatrixV._m21;
                tmp1.x = unity_MatrixV._m20 * tmp1.x + tmp1.y;
                tmp1.x = unity_MatrixV._m22 * tmp1.z + tmp1.x;
                tmp1.x = unity_MatrixV._m23 * tmp1.w + tmp1.x;
                o.texcoord7.z = -tmp1.x;
                tmp1.x = tmp2.y * _ProjectionParams.x;
                tmp1.w = tmp1.x * 0.5;
                tmp1.xz = tmp2.xw * float2(0.5, 0.5);
                o.texcoord7.w = tmp2.w;
                o.texcoord7.xy = tmp1.zz + tmp1.xw;
                tmp1.xyz = tmp0.yyy * unity_WorldToLight._m01_m11_m21;
                tmp1.xyz = unity_WorldToLight._m00_m10_m20 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_WorldToLight._m02_m12_m22 * tmp0.zzz + tmp1.xyz;
                o.texcoord8.xyz = unity_WorldToLight._m03_m13_m23 * tmp0.www + tmp0.xyz;
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
                tmp0.xy = inp.texcoord.xy * _NormalMapII_ST.xy + _NormalMapII_ST.zw;
                tmp0 = tex2D(_NormalMapII, tmp0.xy);
                tmp0.x = tmp0.w * tmp0.x;
                tmp0.xy = tmp0.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.w = dot(tmp0.xy, tmp0.xy);
                tmp0.w = min(tmp0.w, 1.0);
                tmp0.w = 1.0 - tmp0.w;
                tmp0.z = sqrt(tmp0.w);
                tmp1.xy = inp.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                tmp1 = tex2D(_BumpMap, tmp1.xy);
                tmp1.x = tmp1.w * tmp1.x;
                tmp1.xy = tmp1.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.w = dot(tmp1.xy, tmp1.xy);
                tmp0.w = min(tmp0.w, 1.0);
                tmp0.w = 1.0 - tmp0.w;
                tmp1.z = sqrt(tmp0.w);
                tmp0.xyz = tmp0.xyz - tmp1.xyz;
                tmp0.w = dot(inp.texcoord4.xyz, inp.texcoord4.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp0.www * inp.texcoord4.xyz;
                tmp1.w = tmp2.y * unity_WorldToObject._m11;
                tmp1.w = unity_WorldToObject._m10 * tmp2.x + tmp1.w;
                tmp1.w = unity_WorldToObject._m12 * tmp2.z + tmp1.w;
                tmp0.w = inp.texcoord4.y * tmp0.w + -tmp1.w;
                tmp0.w = _LocalGlobal * tmp0.w + tmp1.w;
                tmp0.w = tmp0.w * abs(tmp0.w) + -tmp1.y;
                tmp0.w = saturate(tmp0.w * _snow_slider);
                tmp1.w = max(tmp1.y, 0.0);
                tmp1.w = min(tmp1.w, 0.8);
                tmp1.w = tmp1.w * _Freezeeffectnormal;
                tmp2.w = max(_snow_slider, 0.0);
                tmp2.w = min(tmp2.w, 1.2);
                tmp2.w = tmp2.w * 0.7;
                tmp0.w = tmp2.w * tmp0.w + tmp1.w;
                tmp0.xyz = tmp0.www * tmp0.xyz + tmp1.xyz;
                tmp1.xyz = tmp0.yyy * inp.texcoord6.xyz;
                tmp1.xyz = tmp0.xxx * inp.texcoord5.xyz + tmp1.xyz;
                tmp0.xyz = tmp0.zzz * tmp2.xyz + tmp1.xyz;
                tmp1.x = dot(tmp0.xyz, tmp0.xyz);
                tmp1.x = rsqrt(tmp1.x);
                tmp0.xyz = tmp0.xyz * tmp1.xxx;
                tmp1.xyz = _WorldSpaceLightPos0.www * -inp.texcoord3.xyz + _WorldSpaceLightPos0.xyz;
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp1.xyz = tmp1.www * tmp1.xyz;
                tmp2.xyz = _WorldSpaceCameraPos - inp.texcoord3.xyz;
                tmp1.w = dot(tmp2.xyz, tmp2.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp3.xyz = tmp2.xyz * tmp1.www + tmp1.xyz;
                tmp2.xyz = tmp1.www * tmp2.xyz;
                tmp1.w = dot(tmp0.xyz, tmp2.xyz);
                tmp2.x = dot(tmp3.xyz, tmp3.xyz);
                tmp2.x = rsqrt(tmp2.x);
                tmp2.xyz = tmp2.xxx * tmp3.xyz;
                tmp2.w = saturate(dot(tmp0.xyz, tmp2.xyz));
                tmp0.x = dot(tmp0.xyz, tmp1.xyz);
                tmp0.y = saturate(dot(tmp1.xyz, tmp2.xyz));
                tmp0.x = max(tmp0.x, 0.0);
                tmp0.z = 0.7 - _Gloss;
                tmp0.z = tmp0.w * tmp0.z + _Gloss;
                tmp0.z = 1.0 - tmp0.z;
                tmp1.x = tmp0.z * tmp0.z;
                tmp1.y = tmp1.x * tmp1.x;
                tmp1.z = tmp2.w * tmp1.y + -tmp2.w;
                tmp1.z = tmp1.z * tmp2.w + 1.0;
                tmp1.z = tmp1.z * tmp1.z + 0.0000001;
                tmp1.y = tmp1.y * 0.3183099;
                tmp1.y = tmp1.y / tmp1.z;
                tmp1.z = -tmp0.z * tmp0.z + 1.0;
                tmp2.x = abs(tmp1.w) * tmp1.z + tmp1.x;
                tmp2.y = min(tmp0.x, 1.0);
                tmp1.x = tmp2.y * tmp1.z + tmp1.x;
                tmp1.x = tmp1.x * abs(tmp1.w);
                tmp1.x = tmp2.y * tmp2.x + tmp1.x;
                tmp1.x = tmp1.x + 0.00001;
                tmp1.x = 0.5 / tmp1.x;
                tmp1.x = tmp1.y * tmp1.x;
                tmp1.x = tmp1.x * 3.141593;
                tmp1.x = max(tmp1.x, 0.0001);
                tmp1.x = sqrt(tmp1.x);
                tmp1.x = tmp2.y * tmp1.x;
                tmp1.yz = inp.texcoord.xy * _snow_ST.xy + _snow_ST.zw;
                tmp2 = tex2D(_snow, tmp1.yz);
                tmp2.xyz = tmp2.xyz - _Color.xyz;
                tmp2.xyz = tmp0.www * tmp2.xyz + _Color.xyz;
                tmp0.w = tmp0.w * -_Metallic + _Metallic;
                tmp3.xyz = _Color.xyz * tmp2.xyz + float3(-0.2209163, -0.2209163, -0.2209163);
                tmp2.xyz = tmp2.xyz * _Color.xyz;
                tmp3.xyz = tmp0.www * tmp3.xyz + float3(0.2209163, 0.2209163, 0.2209163);
                tmp0.w = -tmp0.w * 0.7790837 + 0.7790837;
                tmp2.xyz = tmp0.www * tmp2.xyz;
                tmp0.w = dot(tmp3.xyz, tmp3.xyz);
                tmp0.w = tmp0.w != 0.0;
                tmp0.w = tmp0.w ? 1.0 : 0.0;
                tmp0.w = tmp0.w * tmp1.x;
                tmp1.x = dot(inp.texcoord8.xyz, inp.texcoord8.xyz);
                tmp4 = tex2D(_LightTexture0, tmp1.xx);
                tmp1.xyz = tmp4.xxx * _LightColor0.xyz;
                tmp4.xyz = tmp0.www * tmp1.xyz;
                tmp5.xyz = float3(1.0, 1.0, 1.0) - tmp3.xyz;
                tmp0.w = 1.0 - tmp0.y;
                tmp2.w = tmp0.w * tmp0.w;
                tmp2.w = tmp2.w * tmp2.w;
                tmp0.w = tmp0.w * tmp2.w;
                tmp3.xyz = tmp5.xyz * tmp0.www + tmp3.xyz;
                tmp3.xyz = tmp3.xyz * tmp4.xyz;
                tmp0.w = tmp0.y + tmp0.y;
                tmp0.y = tmp0.y * tmp0.w;
                tmp0.y = tmp0.y * tmp0.z + -0.5;
                tmp0.z = 1.0 - tmp0.x;
                tmp0.w = tmp0.z * tmp0.z;
                tmp0.w = tmp0.w * tmp0.w;
                tmp0.z = tmp0.z * tmp0.w;
                tmp0.z = tmp0.y * tmp0.z + 1.0;
                tmp0.w = 1.0 - abs(tmp1.w);
                tmp1.w = max(tmp1.w, 0.0);
                tmp1.w = 1.0 - tmp1.w;
                tmp1.w = log(tmp1.w);
                tmp1.w = tmp1.w * _Ice_fresnel;
                tmp1.w = exp(tmp1.w);
                tmp2.w = tmp0.w * tmp0.w;
                tmp2.w = tmp2.w * tmp2.w;
                tmp0.w = tmp0.w * tmp2.w;
                tmp0.y = tmp0.y * tmp0.w + 1.0;
                tmp0.y = tmp0.y * tmp0.z;
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.xyz = tmp1.xyz * tmp0.xxx;
                tmp0.xyz = tmp0.xyz * tmp2.xyz + tmp3.xyz;
                tmp0.w = 1.0 - tmp1.w;
                tmp0.w = _Transparency * tmp0.w + tmp1.w;
                o.sv_target.xyz = tmp0.www * tmp0.xyz;
                o.sv_target.w = 0.0;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}