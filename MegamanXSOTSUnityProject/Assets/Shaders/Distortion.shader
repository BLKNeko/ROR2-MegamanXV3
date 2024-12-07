Shader "Distortion" {
	Properties {
		_Refraction ("Refraction", Range(0, 10)) = 1
		_Power ("Power", Range(1, 10)) = 1
		_AlphaPower ("Vertex Alpha Power", Range(1, 10)) = 1
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Cull ("Face Culling", Float) = 2
	}
	SubShader {
		Tags { "QUEUE" = "Transparent+1" }
		GrabPass {
			"_GrabTexture"
		}
		Pass {
			Tags { "QUEUE" = "Transparent+1" }
			Cull Off
			GpuProgramID 11021
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 texcoord : TEXCOORD0;
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
				float2 texcoord1 : TEXCOORD1;
				float3 texcoord2 : TEXCOORD2;
				float3 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float _Refraction;
			float _Power;
			float _AlphaPower;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _GrabTexture;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                tmp1.xyz = tmp0.xwy * float3(0.5, 0.5, -0.5);
                o.texcoord.xy = tmp1.yy + tmp1.xz;
                o.texcoord.zw = tmp0.zw;
                o.position = tmp0;
                o.color = v.color;
                o.texcoord1.xy = v.texcoord.xy;
                tmp0.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp0.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp0.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp0.xyz = tmp0.www * tmp0.xyz;
                o.texcoord2.xyz = tmp0.xyz;
                tmp1.xyz = v.tangent.yyy * unity_ObjectToWorld._m01_m11_m21;
                tmp1.xyz = unity_ObjectToWorld._m00_m10_m20 * v.tangent.xxx + tmp1.xyz;
                tmp1.xyz = unity_ObjectToWorld._m02_m12_m22 * v.tangent.zzz + tmp1.xyz;
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                o.texcoord3.xyz = tmp1.xyz;
                tmp2.xyz = tmp0.zxy * tmp1.yzx;
                tmp0.xyz = tmp0.yzx * tmp1.zxy + -tmp2.xyz;
                o.texcoord4.xyz = tmp0.xyz * v.tangent.www;
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
                tmp1.xyz = tmp0.yyy * inp.texcoord4.xyz;
                tmp1.xyz = tmp0.xxx * inp.texcoord3.xyz + tmp1.xyz;
                tmp0.x = dot(tmp0.xy, tmp0.xy);
                tmp0.x = min(tmp0.x, 1.0);
                tmp0.x = 1.0 - tmp0.x;
                tmp0.x = sqrt(tmp0.x);
                tmp0.xyz = tmp0.xxx * inp.texcoord2.xyz + tmp1.xyz;
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp0.xyz = tmp0.www * tmp0.xyz;
                tmp1.xyz = tmp0.yyy * unity_MatrixV._m01_m11_m21;
                tmp0.xyw = unity_MatrixV._m00_m10_m20 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_MatrixV._m02_m12_m22 * tmp0.zzz + tmp0.xyw;
                tmp0.z = dot(tmp0.xyz, tmp0.xyz);
                tmp0.z = rsqrt(tmp0.z);
                tmp0.xy = tmp0.zz * tmp0.xy;
                tmp0.z = dot(tmp0.xy, tmp0.xy);
                tmp0.xy = tmp0.xy * _Refraction.xx;
                tmp0.z = sqrt(tmp0.z);
                tmp0.z = log(tmp0.z);
                tmp0.z = tmp0.z * _Power;
                tmp0.z = exp(tmp0.z);
                tmp0.xy = tmp0.zz * tmp0.xy;
                tmp0.xy = tmp0.xy / _ScreenParams.xy;
                tmp0.xy = tmp0.xy / inp.position.zz;
                tmp0.z = log(inp.color.w);
                tmp0.z = tmp0.z * _AlphaPower;
                tmp0.z = exp(tmp0.z);
                tmp0.xy = tmp0.zz * tmp0.xy;
                tmp0.z = 0.0;
                tmp0.xyz = tmp0.xyz + inp.texcoord.xyw;
                tmp0.xy = tmp0.xy / tmp0.zz;
                o.sv_target = tex2D(_GrabTexture, tmp0.xy);
                return o;
			}
			ENDCG
		}
	}
}