Shader "Effects/WeaponFX/IceMobile" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,1)
		_RimColor ("Rim Color", Vector) = (1,1,1,0.5)
		_MainTex ("MainTex (r)", 2D) = "black" {}
		_HeightMap ("_HeightMap (r)", 2D) = "black" {}
		_Height ("_Height", Float) = 0.1
		_FPOW ("FPOW Fresnel", Float) = 5
		_R0 ("R0 Fresnel", Float) = 0.05
		_BumpAmt ("Distortion", Float) = 10
		_IceStrength ("Ice Strength", Float) = 2
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha One, SrcAlpha One
			ZWrite Off
			Offset -1, -1
			GpuProgramID 41635
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 sv_position : SV_Position0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float4 color : COLOR0;
				float3 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float _Height;
			float4 _Height_ST;
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _Color;
			float4 _RimColor;
			float _FPOW;
			float _R0;
			float _IceStrength;
			float4 _LightColor0;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			sampler2D _HeightMap;
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = tex2Dlod(_HeightMap, float4(v.texcoord.xy, 0, 0.0), 0);
                tmp0.yzw = v.normal.xyz * _Height.xxx;
                tmp0.xyz = tmp0.yzw * tmp0.xxx + v.vertex.xyz;
                tmp1 = mul(unity_ObjectToWorld, float4(tmp0.xyz, 1.0));
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                o.sv_position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.texcoord2.xy = v.texcoord.xy * _Height_ST.xy + _Height_ST.zw;
                o.texcoord1.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.color = v.color;
                o.texcoord3.xyz = v.normal.xyz;
                tmp1.xyz = mul(unity_WorldToObject, _WorldSpaceCameraPos);
                tmp0.xyz = tmp1.xyz - tmp0.xyz;
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                o.texcoord4.xyz = tmp0.www * tmp0.xyz;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = dot(inp.texcoord3.xyz, inp.texcoord4.xyz);
                tmp0.x = saturate(0.7 - tmp0.x);
                tmp0.x = log(tmp0.x);
                tmp0.x = tmp0.x * _FPOW;
                tmp0.x = exp(tmp0.x);
                tmp0.y = 1.0 - _R0;
                tmp0.x = tmp0.y * tmp0.x + _R0;
                tmp1 = tex2D(_MainTex, inp.texcoord1.xy);
                tmp0.y = tmp1.y * tmp1.x;
                tmp0.yzw = tmp0.yyy * _Color.xyz;
                tmp0.yzw = tmp0.yzw * _IceStrength.xxx;
                tmp0.xyz = saturate(tmp0.xxx * _RimColor.xyz + tmp0.yzw);
                tmp0.xyz = tmp0.xyz * inp.color.www;
                tmp1.xyz = _LightColor0.www * _LightColor0.xyz;
                tmp1.xyz = saturate(tmp1.xyz + tmp1.xyz);
                o.sv_target.xyz = tmp0.xyz * tmp1.xyz;
                o.sv_target.w = inp.color.w * _Color.w;
                return o;
			}
			ENDCG
		}
	}
}