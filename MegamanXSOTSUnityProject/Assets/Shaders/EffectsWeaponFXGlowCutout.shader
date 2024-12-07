Shader "Effects/WeaponFX/GlowCutout" {
	Properties {
		[HDR] _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
		_TimeScale ("Time Scale", Vector) = (1,1,1,1)
		_MainTex ("Noise Texture", 2D) = "white" {}
		_BorderScale ("Border Scale (XY) Offset (Z)", Vector) = (0.5,0.05,1,0)
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 46312
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 sv_position : SV_Position0;
				float4 color : COLOR0;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL0;
				float3 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4 _BorderScale;
			float4 _MainTex_ST;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _TintColor;
			float4 _TimeScale;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0.xyz = v.normal.xyz * _BorderScale.zzz;
                tmp0.xyz = tmp0.xyz * float3(0.01, 0.01, 0.01) + v.vertex.xyz;
                tmp1 = tmp0.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp1 = unity_ObjectToWorld._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_ObjectToWorld._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.texcoord1.xyz = tmp0.xyz;
                tmp0 = tmp1 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.sv_position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.color = v.color;
                o.texcoord.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                o.normal.xyz = v.normal.xyz;
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0.xy = _TimeScale.xy * _Time.xx;
                tmp0.zw = inp.texcoord1.xy * _MainTex_ST.xx + tmp0.xy;
                tmp1 = inp.texcoord1.xzzy * _MainTex_ST + tmp0.xyxy;
                tmp0 = tex2D(_MainTex, tmp0.zw);
                tmp2 = tex2D(_MainTex, tmp1.zw);
                tmp1 = tex2D(_MainTex, tmp1.xy);
                tmp0.z = abs(inp.normal.y) + abs(inp.normal.x);
                tmp0.z = tmp0.z + abs(inp.normal.z);
                tmp3.xyz = abs(inp.normal.xyz) / tmp0.zzz;
                tmp0.zw = tmp1.xy * tmp3.yy;
                tmp0.zw = tmp2.xy * tmp3.xx + tmp0.zw;
                tmp0.xy = tmp0.xy * tmp3.zz + tmp0.zw;
                tmp0.xy = _Time.xx * _TimeScale.zw + tmp0.xy;
                tmp1 = inp.texcoord1.xzzy * _MainTex_ST + tmp0.xyxy;
                tmp0.xy = inp.texcoord1.xy * _MainTex_ST.xx + tmp0.xy;
                tmp0 = tex2D(_MainTex, tmp0.xy);
                tmp2 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tex2D(_MainTex, tmp1.zw);
                tmp0.zw = tmp3.yy * tmp2.xy;
                tmp0.zw = tmp1.xy * tmp3.xx + tmp0.zw;
                tmp0.xy = tmp0.xy * tmp3.zz + tmp0.zw;
                tmp0.z = _BorderScale.x - _BorderScale.y;
                tmp0.z = tmp0.z >= tmp0.x;
                tmp0.z = tmp0.z ? -1.0 : -0.0;
                tmp0.x = _BorderScale.x >= tmp0.x;
                tmp0.x = tmp0.x ? 1.0 : 0.0;
                tmp0.x = tmp0.z + tmp0.x;
                tmp0.x = tmp0.y * tmp0.x;
                tmp0 = tmp0.xxxx * inp.color;
                tmp0 = tmp0 * _TintColor;
                o.sv_target.w = saturate(tmp0.w);
                o.sv_target.xyz = tmp0.xyz;
                return o;
			}
			ENDCG
		}
	}
}