Shader "RainDrop/Internal/RainDistortion (Mobile)" {
	Properties {
		_Strength ("Distortion Strength", Range(0, 1000)) = 50
		_Distortion ("Normalmap", 2D) = "bump" {}
	}
	SubShader {
		LOD 100
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass {
			"_BackgroundTexture"
		}
		Pass {
			LOD 100
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			ColorMask RGB -1
			ZTest Always
			GpuProgramID 46902
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 sv_position : SV_Position0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD2;
				float4 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _BackgroundTexture_TexelSize;
			float _Strength;
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _Distortion;
			sampler2D _BackgroundTexture;
			
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
                o.sv_position = tmp0;
                o.texcoord2.xy = _BackgroundTexture_TexelSize.xy * _Strength.xx;
                o.texcoord.xy = v.texcoord.xy;
                tmp1.xyz = tmp0.xwy * float3(0.5, 0.5, -0.5);
                o.texcoord1.zw = tmp0.zw;
                o.texcoord1.xy = tmp1.yy + tmp1.xz;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                tmp0 = tex2D(_Distortion, inp.texcoord.xy);
                tmp0.x = tmp0.w * tmp0.x;
                tmp0.xy = tmp0.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.xy = -inp.texcoord2.xy * tmp0.xy + inp.texcoord1.xy;
                tmp0.xy = tmp0.xy / inp.texcoord1.ww;
                o.sv_target = tex2D(_BackgroundTexture, tmp0.xy);
                return o;
			}
			ENDCG
		}
	}
}