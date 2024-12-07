Shader "Shader Forge/SF_Layzer_01_B" {
	Properties {
		_LayzerMap ("Layzer Map", 2D) = "white" {}
		_Color_A ("Color_A", Color) = (1,1,0,1)
		_Color_B ("Color_B", Color) = (1,0.5019608,0,1)
		_Direction ("Direction", Range(0, 1)) = 0
		_UV ("UV", Range(0, 10)) = 0
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 44084
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float _Direction;
			float4 _Color_A;
			float4 _LayzerMap_ST;
			float4 _Color_B;
			float _UV;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _CameraDepthTexture;
			sampler2D _LayzerMap;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp1 = mul(unity_MatrixVP, tmp0);
                o.position = tmp1;
                o.texcoord.xy = v.texcoord.xy;
                tmp0.y = tmp0.y * unity_MatrixV._m21;
                tmp0.x = unity_MatrixV._m20 * tmp0.x + tmp0.y;
                tmp0.x = unity_MatrixV._m22 * tmp0.z + tmp0.x;
                tmp0.x = unity_MatrixV._m23 * tmp0.w + tmp0.x;
                o.texcoord1.z = -tmp0.x;
                tmp0.x = tmp1.y * _ProjectionParams.x;
                tmp0.w = tmp0.x * 0.5;
                tmp0.xz = tmp1.xw * float2(0.5, 0.5);
                o.texcoord1.w = tmp1.w;
                o.texcoord1.xy = tmp0.zz + tmp0.xw;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.x = _UV * 0.5 + 0.5;
                tmp0.y = tmp0.x * 0.5;
                tmp1 = inp.texcoord.xyxy * float4(2.0, 1.0, 2.0, 1.0);
                tmp2 = _Time * float4(3.0, 0.3, -3.0, -0.3);
                tmp3 = tmp1 * tmp0.yyyy + tmp2;
                tmp0 = tmp1 * tmp0.xxxx + tmp2;
                tmp1.xy = tmp3.zw - tmp3.xy;
                tmp1.xy = _Direction.xx * tmp1.xy + tmp3.xy;
                tmp1.xy = tmp1.xy * _LayzerMap_ST.xy + _LayzerMap_ST.zw;
                tmp1 = tex2D(_LayzerMap, tmp1.xy);
                tmp0.zw = tmp0.zw - tmp0.xy;
                tmp0.xy = _Direction.xx * tmp0.zw + tmp0.xy;
                tmp0.xy = tmp0.xy * _LayzerMap_ST.xy + _LayzerMap_ST.zw;
                tmp0 = tex2D(_LayzerMap, tmp0.xy);
                tmp0.xyz = tmp1.xyz * tmp0.xyz;
                tmp1.xyz = -tmp0.xyz * float3(3.0, 3.0, 3.0) + float3(1.0, 1.0, 1.0);
                tmp0.xyz = tmp0.xyz * float3(3.0, 3.0, 3.0);
                tmp1.xyz = tmp1.xyz * _Color_B.xyz;
                tmp1.xyz = tmp0.xyz * _Color_A.xyz + tmp1.xyz;
                tmp2.xy = float2(1.0, 1.0) - inp.texcoord.yx;
                tmp2.xy = tmp2.xy * inp.texcoord.yx;
                tmp2.xy = tmp2.yx * float2(24.0, 4.0);
                tmp0.w = tmp2.y * tmp2.y;
                tmp0.w = tmp0.w * tmp0.w;
                tmp0.w = tmp0.w * tmp0.w;
                tmp0.xyz = min(tmp0.www, tmp0.xyz);
                o.sv_target.xyz = tmp0.xyz + tmp1.xyz;
                tmp2.x = saturate(tmp2.x);
                tmp0.x = tmp2.x * tmp2.y;
                tmp0.yz = inp.texcoord1.xy / inp.texcoord1.ww;
                tmp1 = tex2D(_CameraDepthTexture, tmp0.yz);
                tmp0.y = _ZBufferParams.z * tmp1.x + _ZBufferParams.w;
                tmp0.y = 1.0 / tmp0.y;
                tmp0.y = tmp0.y - _ProjectionParams.y;
                tmp0.z = inp.texcoord1.z - _ProjectionParams.y;
                tmp0.yz = max(tmp0.yz, float2(0.0, 0.0));
                tmp0.y = saturate(tmp0.y - tmp0.z);
                o.sv_target.w = tmp0.y * tmp0.x;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}