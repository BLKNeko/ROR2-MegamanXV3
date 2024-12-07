Shader "orange/panning" {
	Properties {
		_MainTex_Color ("MainTex_Color", Color) = (1,0.1530337,0.004716992,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_x_speed_maintex ("x_speed_maintex", Float) = -0.8
		_y_speed_maintex ("y_speed_maintex", Float) = 0
		_Mask ("Mask", 2D) = "white" {}
		_x_speed_noise ("x_speed_noise", Float) = -0.5
		_y_speed_noise ("y_speed_noise", Float) = 0
		_claer_mask ("claer_mask", Float) = 5
		_Dissolver_amount ("Dissolver_amount", Range(0, 10)) = 10
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "QUEUE" = "AlphaTest" "RenderType" = "TransparentCutout" }
		Pass {
			Name "FORWARD"
			Tags { "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "AlphaTest" "RenderType" = "TransparentCutout" "SHADOWSUPPORT" = "true" }
			Cull Off
			Stencil {
				Ref 128
			}
			GpuProgramID 6773
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float4 _MainTex_ST;
			float _x_speed_maintex;
			float _y_speed_maintex;
			float4 _Mask_ST;
			float _x_speed_noise;
			float _y_speed_noise;
			float4 _MainTex_Color;
			float _claer_mask;
			float _Dissolver_amount;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _Mask;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.texcoord.xy = v.texcoord.xy;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = float4(_x_speed_noise.xx, _y_speed_noise.xx) * _Time;
                tmp0.xy = tmp0.xy * float2(1.0, 0.0) + inp.texcoord.xy;
                tmp0.xy = tmp0.zw * float2(0.0, 1.0) + tmp0.xy;
                tmp0.xy = tmp0.xy * _Mask_ST.xy + _Mask_ST.zw;
                tmp0 = tex2D(_Mask, tmp0.xy);
                tmp0.y = inp.texcoord.x * _claer_mask;
                tmp0.z = 1.0 - inp.texcoord.x;
                tmp0.y = min(tmp0.z, tmp0.y);
                tmp0.x = tmp0.x * tmp0.y;
                tmp0.x = dot(tmp0.xy, float2(_claer_mask.x, _Dissolver_amount.x));
                tmp0.x = tmp0.x - 1.0;
                tmp1 = float4(_x_speed_maintex.xx, _y_speed_maintex.xx) * _Time;
                tmp0.yz = tmp1.xy * float2(1.0, 0.0) + inp.texcoord.xy;
                tmp0.yz = tmp1.zw * float2(0.0, 1.0) + tmp0.yz;
                tmp0.yz = tmp0.yz * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp1 = tex2D(_MainTex, tmp0.yz);
                tmp0.y = tmp1.x * tmp0.x + -0.5;
                tmp0.x = tmp0.x * tmp1.x;
                tmp1.xyz = tmp1.xyz * _MainTex_Color.xyz;
                o.sv_target.xyz = tmp0.xxx * tmp1.xyz;
                tmp0.x = tmp0.y < 0.0;
                if (tmp0.x) {
                    discard;
                }
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}