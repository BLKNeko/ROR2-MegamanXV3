Shader "FlowEmission/Particle_AlphaBlend_FlowEmission(Mask)_Mask02_NoFog" {
	Properties {
		_MainTex ("FlowEmission(Mask)", 2D) = "white" {}
		_Mask_02 ("Mask_02", 2D) = "white" {}
		_Angle ("Angle", Range(0, 2)) = 0
		_FlowSpeed ("FlowSpeed", Float) = 1
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask RGB -1
			ZWrite Off
			GpuProgramID 23918
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float4 _TimeEditor;
			float4 _MainTex_ST;
			float4 _Mask_02_ST;
			float _Angle;
			float _FlowSpeed;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _Mask_02;
			
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
                o.color = v.color;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.x = _Angle * 3.141593;
                tmp0.x = sin(tmp0.x);
                tmp1.x = cos(tmp0.x);
                tmp2.z = tmp0.x;
                tmp0.yz = inp.texcoord.xy - float2(0.5, 0.5);
                tmp2.y = tmp1.x;
                tmp2.x = -tmp0.x;
                tmp1.y = dot(tmp0.xy, tmp2.xy);
                tmp1.x = dot(tmp0.xy, tmp2.xy);
                tmp0.xy = tmp1.xy + float2(0.5, 0.5);
                tmp0.z = _TimeEditor.y + _Time.y;
                tmp0.z = tmp0.z * _FlowSpeed;
                tmp0.xy = tmp0.zz * float2(0.0, 1.0) + tmp0.xy;
                tmp0.xy = tmp0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp0.w = tmp0.w * inp.color.w;
                o.sv_target.xyz = tmp0.xyz * inp.color.xyz;
                tmp0.xy = inp.texcoord.xy * _Mask_02_ST.xy + _Mask_02_ST.zw;
                tmp1 = tex2D(_Mask_02, tmp0.xy);
                o.sv_target.w = tmp0.w * tmp1.x;
                return o;
			}
			ENDCG
		}
	}
	CustomEditor "ShaderForgeMaterialInspector"
}