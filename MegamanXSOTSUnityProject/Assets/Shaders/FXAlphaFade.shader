Shader "FX/AlphaFade" {
	Properties {
		_Color ("Color", Color) = (0,1,1,1)
		_AlphaTexture ("Alpha Texture", 2D) = "white" {}
		_Tiling ("Tiling", Float) = 3
		_Edge ("Edge", Float) = 0.1
		_Threshold ("Threshold", Float) = 3
		_ScrollSpeed ("Scroll Speed", Range(0, 5)) = 1
		_Intensity ("Intensity", Float) = 0.5
		_GlitchSpeed ("Glitch Speed", Range(0, 50)) = 50
		_GlitchIntensity ("Glitch Intensity", Range(0, 0.1)) = 0
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Overlay" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Overlay" "RenderType" = "Transparent" }
			Blend SrcAlpha One, SrcAlpha One
			GpuProgramID 30189
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float1 texcoord2 : TEXCOORD2;
				float3 texcoord1 : TEXCOORD1;
				float3 normal : NORMAL0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _ScrollSpeed;
			float _GlitchSpeed;
			float _GlitchIntensity;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Color;
			float _Tiling;
			float _Edge;
			float _Intensity;
			float _Threshold;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _AlphaTexture;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.x = _GlitchSpeed * _Time.y;
                tmp0.x = tmp0.x * v.vertex.y;
                tmp0.x = tmp0.x * 5.0;
                tmp0.x = sin(tmp0.x);
                tmp0.z = tmp0.x * _GlitchIntensity + v.vertex.z;
                tmp1 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                tmp2.xyz = tmp1.yyy * unity_MatrixV._m01_m11_m21;
                tmp2.xyz = unity_MatrixV._m00_m10_m20 * tmp1.xxx + tmp2.xyz;
                tmp1.xyz = unity_MatrixV._m02_m12_m22 * tmp1.zzz + tmp2.xyz;
                tmp1.xyz = unity_MatrixV._m03_m13_m23 * tmp1.www + tmp1.xyz;
                tmp1.w = _Time.x * _ScrollSpeed + tmp1.y;
                o.texcoord1.xyz = tmp1.xwz;
                tmp0.xy = v.vertex.xy;
                tmp0.x = dot(tmp0.xyz, tmp0.xyz);
                o.texcoord2.x = sqrt(tmp0.x);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = _Threshold < inp.texcoord2.x;
                if (tmp0.x) {
                    discard;
                }
                tmp0.x = _Threshold - _Edge;
                tmp0.x = inp.texcoord2.x - tmp0.x;
                tmp0.x = saturate(tmp0.x * 100.0);
                tmp0.yz = inp.texcoord1.xy * _Tiling.xx;
                tmp1 = tex2D(_AlphaTexture, tmp0.yz);
                o.sv_target.xyz = tmp0.xxx + _Color.xyz;
                tmp0.x = tmp0.x * 100.0 + _Intensity;
                o.sv_target.w = tmp0.x * tmp1.w;
                return o;
			}
			ENDCG
		}
	}
}