Shader "UI/SimpleDistortion" {
	Properties {
		[HDR] _Color ("Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_DistTex ("Distort Texture", 2D) = "white" {}
		_Params ("_Params (X=OffsetMultiply, YZW=Unused)", Vector) = (0.05,0,0,0)
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass {
			"_GrabTexture"
		}
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			GpuProgramID 61577
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 texcoord : TEXCOORD0;
				float4 color : COLOR0;
				float4 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Color;
			float4 _Params;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _DistTex;
			sampler2D _MainTex;
			sampler2D _GrabTexture;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                tmp0 = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.position = tmp0;
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.texcoord.zw = float2(0.0, 0.0);
                o.color = v.color;
                tmp0.xyz = tmp0.xwy * float3(0.5, 0.5, -0.5);
                tmp0.xy = tmp0.yy + tmp0.xz;
                o.texcoord1.xy = tmp0.xy / tmp0.ww;
                o.texcoord1.zw = float2(0.0, 0.0);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0 = tex2D(_DistTex, inp.texcoord.xy);
                tmp0.xy = tmp0.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.xy = tmp0.xy * _Params.xx;
                tmp0.xy = tmp0.xy * inp.color.ww + inp.texcoord1.xy;
                tmp0 = tex2D(_GrabTexture, tmp0.xy);
                tmp1 = inp.color + inp.color;
                tmp1 = tmp1 * _Color;
                tmp2 = tex2D(_MainTex, inp.texcoord.xy);
                tmp3.xyz = tmp2.xyz == float3(0.0, 0.0, 0.0);
                tmp3.x = tmp3.y ? tmp3.x : 0.0;
                tmp3.x = tmp3.z ? tmp3.x : 0.0;
                tmp2.w = tmp3.x ? 0.0 : tmp2.w;
                tmp1 = tmp1 * tmp2;
                tmp0 = tmp2 * tmp1 + tmp0;
                o.sv_target.w = saturate(tmp0.w);
                o.sv_target.xyz = tmp0.xyz;
                return o;
			}
			ENDCG
		}
	}
}