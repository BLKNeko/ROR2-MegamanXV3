Shader "Legacy Particles/Particle Distortion" {
	Properties {
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_Distortionpower ("Distortion power", Float) = 0.05
		_InvFade ("Soft Particles Factor", Range(0.01, 3)) = 1
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass {
		}
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 21206
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
				float4 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _NormalMap_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float _Distortionpower;
			float4 _GrabTexture_TexelSize;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _NormalMap;
			sampler2D _GrabTexture;
			
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
                tmp0.xyz = unity_ObjectToWorld._m03_m13_m23 * v.vertex.www + tmp0.xyz;
                tmp0.xyz = _WorldSpaceCameraPos - tmp0.xyz;
                tmp0.x = dot(tmp0.xyz, tmp0.xyz);
                tmp0.x = sqrt(tmp0.x);
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                tmp1 = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.position = tmp1;
                o.color = v.color;
                o.texcoord1.z = tmp1.w / tmp0.x;
                tmp0.xy = tmp1.xy * float2(1.0, -1.0) + tmp1.ww;
                o.texcoord1.w = tmp1.w;
                o.texcoord1.xy = tmp0.xy * float2(0.5, 0.5);
                o.texcoord2.xy = v.texcoord.xy * _NormalMap_ST.xy + _NormalMap_ST.zw;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0 = tex2D(_NormalMap, inp.texcoord2.xy);
                tmp0.x = tmp0.w * tmp0.x;
                tmp0.xy = tmp0.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp0.zw = tmp0.xy * _GrabTexture_TexelSize.xy;
                tmp0.x = abs(tmp0.y) * 30.0 + abs(tmp0.x);
                tmp0.x = tmp0.x - 0.03;
                tmp0.yz = tmp0.zw * _Distortionpower.xx;
                tmp0.yz = tmp0.yz * inp.color.ww;
                tmp0.yz = tmp0.yz * inp.texcoord1.zz + inp.texcoord1.xy;
                tmp0.yz = tmp0.yz / inp.texcoord1.ww;
                tmp1 = tex2D(_GrabTexture, tmp0.yz);
                o.sv_target.w = saturate(tmp0.x * tmp1.w);
                o.sv_target.xyz = tmp1.xyz;
                return o;
			}
			ENDCG
		}
	}
}