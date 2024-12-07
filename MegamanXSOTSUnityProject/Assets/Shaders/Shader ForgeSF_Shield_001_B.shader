Shader "Shader Forge/SF_Shield_001_B" {
	Properties {
		_Shield ("Shield", 2D) = "white" {}
		_Smoke ("Smoke", 2D) = "white" {}
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 48682
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
				float4 color : COLOR0;
				float4 texcoord3 : TEXCOORD3;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Shield_ST;
			float4 _Smoke_ST;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _CameraDepthTexture;
			sampler2D _Smoke;
			sampler2D _Shield;
			
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
                o.texcoord1 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp0 = mul(unity_MatrixVP, tmp1);
                o.position = tmp0;
                o.texcoord.xy = v.texcoord.xy;
                tmp2.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp2.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp2.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.z = dot(tmp2.xyz, tmp2.xyz);
                tmp0.z = rsqrt(tmp0.z);
                o.texcoord2.xyz = tmp0.zzz * tmp2.xyz;
                o.color = v.color;
                tmp0.z = tmp1.y * unity_MatrixV._m21;
                tmp0.z = unity_MatrixV._m20 * tmp1.x + tmp0.z;
                tmp0.z = unity_MatrixV._m22 * tmp1.z + tmp0.z;
                tmp0.z = unity_MatrixV._m23 * tmp1.w + tmp0.z;
                o.texcoord3.z = -tmp0.z;
                tmp0.y = tmp0.y * _ProjectionParams.x;
                tmp1.xzw = tmp0.xwy * float3(0.5, 0.5, 0.5);
                o.texcoord3.w = tmp0.w;
                o.texcoord3.xy = tmp1.zz + tmp1.xw;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.x = sin(_Time.y);
                tmp0.yz = _Time.yy + float2(2.0, 4.0);
                tmp0.yz = sin(tmp0.yz);
                tmp0.xyz = max(tmp0.xyz, float3(0.0, 0.0, 0.0));
                tmp1.xy = _Time.yy * float2(0.5, 0.0);
                tmp1.xy = inp.texcoord.xy * float2(5.0, 2.0) + tmp1.xy;
                tmp1.xy = tmp1.xy * _Shield_ST.xy + _Shield_ST.zw;
                tmp1 = tex2D(_Shield, tmp1.xy);
                tmp0.y = tmp0.y * tmp1.y;
                tmp0.x = tmp1.x * tmp0.x + tmp0.y;
                tmp0.x = tmp1.z * tmp0.z + tmp0.x;
                tmp2 = _Time * float4(0.3, 0.3, 0.1, 0.1) + inp.texcoord.xyxy;
                tmp2 = tmp2 * _Smoke_ST + _Smoke_ST;
                tmp3 = tex2D(_Smoke, tmp2.zw);
                tmp2 = tex2D(_Smoke, tmp2.xy);
                tmp0.y = tmp2.x * tmp2.x;
                tmp0.y = dot(tmp0.xy, tmp1.xy);
                tmp0.z = log(tmp3.x);
                tmp0.z = tmp0.z * 1.5;
                tmp0.z = exp(tmp0.z);
                tmp0.z = tmp0.z * 1.5;
                tmp0.x = tmp0.z * tmp0.x + tmp0.y;
                tmp0.y = tmp0.x - 0.5;
                tmp0.y = -tmp0.y * 2.0 + 1.0;
                tmp1.xyz = float3(1.0, 1.0, 1.0) - inp.color.xyz;
                tmp0.yzw = -tmp0.yyy * tmp1.xyz + float3(1.0, 1.0, 1.0);
                tmp1.x = tmp0.x + tmp0.x;
                tmp1.xyz = tmp1.xxx * inp.color.xyz;
                tmp1.w = tmp0.x > 0.5;
                tmp0.yzw = saturate(tmp1.www ? tmp0.yzw : tmp1.xyz);
                o.sv_target.xyz = tmp0.yzw * float3(1.6, 1.6, 1.6);
                tmp0.yzw = _WorldSpaceCameraPos - inp.texcoord1.xyz;
                tmp1.x = dot(tmp0.xyz, tmp0.xyz);
                tmp1.x = rsqrt(tmp1.x);
                tmp0.yzw = tmp0.yzw * tmp1.xxx;
                tmp1.x = dot(inp.texcoord2.xyz, inp.texcoord2.xyz);
                tmp1.x = rsqrt(tmp1.x);
                tmp1.xyz = tmp1.xxx * inp.texcoord2.xyz;
                tmp0.y = dot(tmp1.xyz, tmp0.xyz);
                tmp0.y = max(tmp0.y, 0.0);
                tmp0.y = 1.0 - tmp0.y;
                tmp0.y = tmp0.y * tmp0.y;
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.y = 1.0 - inp.texcoord.y;
                tmp0.y = tmp0.y * inp.texcoord.y;
                tmp0.y = tmp0.y * tmp0.y;
                tmp0.y = tmp0.y * 12.0;
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.x = tmp0.x * 3.0;
                tmp0.yz = inp.texcoord3.xy / inp.texcoord3.ww;
                tmp1 = tex2D(_CameraDepthTexture, tmp0.yz);
                tmp0.y = _ZBufferParams.z * tmp1.x + _ZBufferParams.w;
                tmp0.y = 1.0 / tmp0.y;
                tmp0.y = tmp0.y - _ProjectionParams.y;
                tmp0.z = inp.texcoord3.z - _ProjectionParams.y;
                tmp0.yz = max(tmp0.yz, float2(0.0, 0.0));
                tmp0.y = tmp0.y - tmp0.z;
                tmp0.y = saturate(tmp0.y * 3.333333);
                o.sv_target.w = tmp0.y * tmp0.x;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}