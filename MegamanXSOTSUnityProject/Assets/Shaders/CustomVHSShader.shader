Shader "Custom/VHSShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_VhsVSpeed ("VHS V Speed", Float) = 1
		_VhsHSpeed ("VHS H Speed", Float) = 1
		_VhsVOffect ("VHS V Offset", Float) = 64
		[MaterialToggle] _VHSOn ("VHS Effect On", Float) = 1
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_NoiseXSpeed ("Noise X Speed", Float) = 100
		_NoiseYSpeed ("Noise Y Speed", Float) = 100
		_NoiseCutoff ("Noise Cutoff", Range(0, 1)) = 0
		_DistortionSrength ("Distortion Strength", Float) = 1
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 10496
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR0;
				float4 position : SV_POSITION0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _Color;
			// $Globals ConstantBuffers for Fragment Shader
			float _VhsVSpeed;
			float _VhsHSpeed;
			float _VhsVOffect;
			float _VHSOn;
			float _NoiseXSpeed;
			float _NoiseYSpeed;
			float _NoiseCutoff;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                o.texcoord.xy = v.texcoord.xy;
                o.color = v.color * _Color;
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
                float4 tmp2;
                tmp0.x = _VHSOn == 1.0;
                if (tmp0.x) {
                    tmp0.yz = inp.texcoord.xy / float2(_VhsVSpeed.x, _VhsHSpeed.x);
                    tmp1.y = _Time.y;
                    tmp1.xz = inp.texcoord.yy;
                    tmp0.w = dot(tmp1.xy, float2(12.9898, 78.233));
                    tmp0.w = sin(tmp0.w);
                    tmp0.w = tmp0.w * 43758.55;
                    tmp0.w = frac(tmp0.w);
                    tmp0.w = tmp0.w - 0.5;
                    tmp0.w = tmp0.w / _VhsVOffect;
                    tmp0.x = tmp0.w + tmp0.y;
                    tmp0.y = dot(tmp1.xy, float2(12.9898, 78.233));
                    tmp0.y = sin(tmp0.y);
                    tmp0.y = tmp0.y * 43758.55;
                    tmp2.x = frac(tmp0.y);
                    tmp1.yw = _Time.yy + float2(2.0, 1.0);
                    tmp0.y = dot(tmp1.xy, float2(12.9898, 78.233));
                    tmp0.y = sin(tmp0.y);
                    tmp0.y = tmp0.y * 43758.55;
                    tmp2.y = frac(tmp0.y);
                    tmp1.x = inp.texcoord.y;
                    tmp0.y = dot(tmp1.xy, float2(12.9898, 78.233));
                    tmp0.y = sin(tmp0.y);
                    tmp0.y = tmp0.y * 43758.55;
                    tmp2.z = frac(tmp0.y);
                    tmp2.w = 0.0;
                    tmp1 = tmp2 - float4(0.5, 0.5, 0.5, 0.5);
                    tmp0 = tex2D(_MainTex, tmp0.xz);
                    tmp0 = tmp1 * float4(0.1, 0.1, 0.1, 0.1) + tmp0;
                    tmp1.xy = float2(_NoiseXSpeed.x, _NoiseYSpeed.x) * _Time.zz + inp.texcoord.xy;
                    tmp1 = tex2D(_NoiseTex, tmp1.xy);
                    tmp1.x = _NoiseCutoff < tmp1.x;
                    tmp1.x = tmp1.x ? 0.2 : 0.0;
                    o.sv_target = tmp0 + tmp1.xxxx;
                } else {
                    tmp0 = tex2D(_MainTex, inp.texcoord.xy);
                    tmp1.xy = float2(_NoiseXSpeed.x, _NoiseYSpeed.x) * _Time.zz + inp.texcoord.xy;
                    tmp1 = tex2D(_NoiseTex, tmp1.xy);
                    tmp1.x = _NoiseCutoff < tmp1.x;
                    tmp1.x = tmp1.x ? 0.2 : 0.0;
                    o.sv_target = tmp0 + tmp1.xxxx;
                }
                return o;
			}
			ENDCG
		}
	}
}