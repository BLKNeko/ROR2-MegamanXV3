Shader "Projector/Multiply" {
	Properties {
		_ShadowTex ("Cookie", 2D) = "gray" {}
		_FalloffTex ("FallOff", 2D) = "white" {}
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" }
			Blend DstColor Zero, DstColor Zero
			ColorMask RGB -1
			ZWrite Off
			Offset -1, -1
			GpuProgramID 27507
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float1 texcoord2 : TEXCOORD2;
				float4 position : SV_POSITION0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			// $Globals ConstantBuffers for Fragment Shader
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _ShadowTex;
			sampler2D _FalloffTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                o.texcoord = mul(unity_Projector, v.vertex);
                tmp0 = v.vertex.yyyy * unity_ProjectorClip._m01_m11_m21_m31;
                tmp0 = unity_ProjectorClip._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ProjectorClip._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                o.texcoord1 = unity_ProjectorClip._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp0.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp0.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp0.x = dot(tmp0.xyz, tmp0.xyz);
                tmp0.x = rsqrt(tmp0.x);
                o.texcoord2.x = saturate(tmp0.x * tmp0.y);
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.xy = inp.texcoord.xy / inp.texcoord.ww;
                tmp0 = tex2D(_ShadowTex, tmp0.xy);
                tmp0 = tmp0 * float4(1.0, 1.0, 1.0, -1.0) + float4(-1.0, -1.0, -1.0, 1.0);
                tmp1.xy = inp.texcoord1.xy / inp.texcoord1.ww;
                tmp1 = tex2D(_FalloffTex, tmp1.xy);
                tmp0 = tmp1.wwww * tmp0 + float4(1.0, 1.0, 1.0, 0.0);
                tmp0 = float4(1.0, 1.0, 1.0, 1.0) - tmp0;
                o.sv_target = -tmp0 * inp.texcoord2.xxxx + float4(1.0, 1.0, 1.0, 1.0);
                return o;
			}
			ENDCG
		}
	}
}