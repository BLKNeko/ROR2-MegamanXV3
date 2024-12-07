Shader "Shader Forge/SF_Em040_01_B" {
	Properties {
		_BodyColor ("Body Color", Color) = (1,0.7686275,0,1)
		_EyeColor ("Eye Color", Color) = (0.5,0.5,0.5,1)
		_Map ("Map", 2D) = "white" {}
		_EyeLight ("Eye Light", Range(0, 1)) = 0
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		Pass {
			Name "FORWARD"
			Tags { "LIGHTMODE" = "FORWARDBASE" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			GpuProgramID 43755
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float4 _BodyColor;
			float4 _EyeColor;
			float4 _Map_ST;
			float _EyeLight;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _Map;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord1 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                o.position = mul(unity_MatrixVP, tmp1);
                o.texcoord.xy = v.texcoord.xy;
                tmp0.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp0.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp0.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                o.texcoord2.xyz = tmp0.www * tmp0.xyz;
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
                tmp0.x = _Time.y + _Time.y;
                tmp0.x = round(tmp0.x);
                tmp0.yz = tmp0.xx + inp.texcoord.xy;
                tmp0.yz = tmp0.yz - float2(0.5, 0.5);
                tmp1.xyz = _Time.zzw * float3(6.0, 12.0, 9.0);
                tmp0.w = round(tmp1.y);
                tmp1.xy = sin(tmp1.xz);
                tmp0.w = tmp0.w + tmp0.w;
                tmp2.x = sin(tmp0.w);
                tmp3.x = cos(tmp0.w);
                tmp4.z = tmp2.x;
                tmp4.y = tmp3.x;
                tmp4.x = -tmp2.x;
                tmp2.y = dot(tmp0.xy, tmp4.xy);
                tmp2.x = dot(tmp0.xy, tmp4.xy);
                tmp0.xy = tmp0.xx + tmp2.xy;
                tmp0.xy = tmp0.xy + float2(0.5, 0.5);
                tmp0.xy = tmp0.xy * _Map_ST.xy + _Map_ST.zw;
                tmp0 = tex2D(_Map, tmp0.xy);
                tmp0.x = tmp0.y + 1.0;
                tmp0.yz = inp.texcoord.xy * _Map_ST.xy + _Map_ST.zw;
                tmp2 = tex2D(_Map, tmp0.yz);
                tmp0.y = 1.0 - tmp2.x;
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.xyz = tmp0.xxx * _BodyColor.xyz;
                tmp0.xyz = tmp0.xyz * tmp2.zzz;
                tmp2.xyz = tmp2.xxx * _EyeColor.xyz;
                tmp0.w = dot(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp3.xyz = tmp0.www * _WorldSpaceLightPos0.xyz;
                tmp4.xyz = _WorldSpaceCameraPos - inp.texcoord1.xyz;
                tmp0.w = dot(tmp4.xyz, tmp4.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp5.xyz = tmp4.xyz * tmp0.www + tmp3.xyz;
                tmp4.xyz = tmp0.www * tmp4.xyz;
                tmp0.w = dot(tmp5.xyz, tmp5.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp5.xyz = tmp0.www * tmp5.xyz;
                tmp0.w = dot(inp.texcoord2.xyz, inp.texcoord2.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp6.xyz = tmp0.www * inp.texcoord2.xyz;
                tmp0.w = dot(tmp5.xyz, tmp6.xyz);
                tmp0.w = max(tmp0.w, 0.0);
                tmp0.w = log(tmp0.w);
                tmp0.w = tmp0.w * 128.0;
                tmp0.w = exp(tmp0.w);
                tmp5.xyz = tmp0.www * _LightColor0.xyz;
                tmp5.xyz = tmp5.xyz + tmp5.xyz;
                tmp0.w = dot(tmp6.xyz, tmp3.xyz);
                tmp1.z = dot(tmp6.xyz, tmp4.xyz);
                tmp1.z = max(tmp1.z, 0.0);
                tmp1.z = 1.0 - tmp1.z;
                tmp1.z = log(tmp1.z);
                tmp0.w = max(tmp0.w, 0.0);
                tmp3.xyz = glstate_lightmodel_ambient.xyz + glstate_lightmodel_ambient.xyz;
                tmp3.xyz = tmp0.www * _LightColor0.xyz + tmp3.xyz;
                tmp0.xyz = tmp3.xyz * tmp0.xyz + tmp5.xyz;
                tmp0.w = sin(_Time.y);
                tmp0.w = max(tmp0.w, 0.0);
                tmp0.w = tmp0.w + 2.0;
                tmp0.w = tmp1.z * tmp0.w;
                tmp0.w = exp(tmp0.w);
                tmp1.x = tmp1.x * _EyeLight;
                tmp1.xy = max(tmp1.xy, float2(0.5, 0.0));
                tmp1.x = min(tmp1.x, 1.0);
                tmp1.y = tmp1.y + 1.0;
                tmp0.w = tmp0.w * tmp1.y;
                tmp0.w = tmp0.w * 0.5;
                tmp1.yzw = tmp0.www * _BodyColor.xyz;
                tmp1.xyz = tmp2.xyz * tmp1.xxx + tmp1.yzw;
                o.sv_target.xyz = tmp0.xyz + tmp1.xyz;
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "FORWARD_DELTA"
			Tags { "LIGHTMODE" = "FORWARDADD" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			Blend One One, One One
			GpuProgramID 96762
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 unity_WorldToLight;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float4 _BodyColor;
			float4 _Map_ST;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _LightTexture0;
			sampler2D _Map;
			
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
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.texcoord.xy = v.texcoord.xy;
                o.texcoord1 = tmp0;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                o.texcoord2.xyz = tmp1.www * tmp1.xyz;
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
                tmp0.x = _Time.z * 12.0;
                tmp0.x = round(tmp0.x);
                tmp0.x = tmp0.x + tmp0.x;
                tmp0.x = sin(tmp0.x);
                tmp1.x = cos(tmp0.x);
                tmp2.z = tmp0.x;
                tmp0.y = _Time.y + _Time.y;
                tmp0.y = round(tmp0.y);
                tmp0.zw = tmp0.yy + inp.texcoord.xy;
                tmp0.zw = tmp0.zw - float2(0.5, 0.5);
                tmp2.y = tmp1.x;
                tmp2.x = -tmp0.x;
                tmp1.y = dot(tmp0.xy, tmp2.xy);
                tmp1.x = dot(tmp0.xy, tmp2.xy);
                tmp0.xy = tmp0.yy + tmp1.xy;
                tmp0.xy = tmp0.xy + float2(0.5, 0.5);
                tmp0.xy = tmp0.xy * _Map_ST.xy + _Map_ST.zw;
                tmp0 = tex2D(_Map, tmp0.xy);
                tmp0.x = tmp0.y + 1.0;
                tmp0.yz = inp.texcoord.xy * _Map_ST.xy + _Map_ST.zw;
                tmp1 = tex2D(_Map, tmp0.yz);
                tmp0.y = 1.0 - tmp1.x;
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.xyz = tmp0.xxx * _BodyColor.xyz;
                tmp0.xyz = tmp0.xyz * tmp1.zzz;
                tmp1.xyz = _WorldSpaceLightPos0.www * -inp.texcoord1.xyz + _WorldSpaceLightPos0.xyz;
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                tmp2.xyz = _WorldSpaceCameraPos - inp.texcoord1.xyz;
                tmp0.w = dot(tmp2.xyz, tmp2.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp2.xyz * tmp0.www + tmp1.xyz;
                tmp0.w = dot(tmp2.xyz, tmp2.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp2.xyz = tmp0.www * tmp2.xyz;
                tmp0.w = dot(inp.texcoord2.xyz, inp.texcoord2.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp3.xyz = tmp0.www * inp.texcoord2.xyz;
                tmp0.w = dot(tmp2.xyz, tmp3.xyz);
                tmp1.x = dot(tmp3.xyz, tmp1.xyz);
                tmp1.x = max(tmp1.x, 0.0);
                tmp0.w = max(tmp0.w, 0.0);
                tmp0.w = log(tmp0.w);
                tmp0.w = tmp0.w * 128.0;
                tmp0.w = exp(tmp0.w);
                tmp1.yzw = mul(unity_WorldToLight, inp.texcoord1.xyz);
                tmp1.y = dot(tmp1.xyz, tmp1.xyz);
                tmp2 = tex2D(_LightTexture0, tmp1.yy);
                tmp1.yzw = tmp2.xxx * _LightColor0.xyz;
                tmp2.xyz = tmp0.www * tmp1.yzw;
                tmp1.xyz = tmp1.yzw * tmp1.xxx;
                tmp2.xyz = tmp2.xyz + tmp2.xyz;
                o.sv_target.xyz = tmp1.xyz * tmp0.xyz + tmp2.xyz;
                o.sv_target.w = 0.0;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}