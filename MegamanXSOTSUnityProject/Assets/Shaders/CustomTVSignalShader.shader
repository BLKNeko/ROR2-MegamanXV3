Shader "Custom/TVSignalShader" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		_Rate ("Rate", Range(0, 1)) = 0.05
		_Intensity ("Intensity (Color)", Range(0, 10)) = 1.5
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			GpuProgramID 12253
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float _Rate;
			float _Intensity;
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
                tmp0 = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp1 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp0.wwww + tmp1;
                o.texcoord.xy = v.texcoord.xy;
                o.texcoord1.xy = float2(0.1123047, 0.0);
                return o;
			}
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.xy = _Time.yy * float2(6.0, 16.0);
                tmp0.xy = sin(tmp0.xy);
                tmp0.x = tmp0.x + 1.0;
                tmp0.y = tmp0.y * 0.5 + 1.0;
                tmp0.x = tmp0.x * 0.5;
                tmp0.x = tmp0.y * tmp0.x;
                tmp0.y = tmp0.x * tmp0.x;
                tmp0.x = tmp0.y * tmp0.x;
                tmp0.x = tmp0.x * _Rate;
                tmp0.yz = inp.texcoord.xy - float2(0.5, 0.5);
                tmp0.y = dot(tmp0.xy, tmp0.xy);
                tmp0.y = sqrt(tmp0.y);
                tmp0.x = tmp0.y * tmp0.x;
                tmp0.y = -tmp0.y * 0.5 + 1.0;
                tmp0.z = inp.texcoord1.x < 0.115;
                tmp0.x = tmp0.z ? tmp0.x : 0.0;
                tmp1.x = tmp0.x + inp.texcoord.x;
                tmp1.z = inp.texcoord.x - tmp0.x;
                tmp1.yw = inp.texcoord.yy;
                tmp2 = tex2D(_MainTex, tmp1.xy);
                tmp1 = tex2D(_MainTex, tmp1.zw);
                tmp1.z = tmp1.z * _Intensity;
                tmp1.x = tmp2.x * _Intensity;
                tmp2 = tex2D(_MainTex, inp.texcoord.xy);
                tmp1.y = tmp2.y * _Intensity;
                tmp0.x = inp.texcoord.y * 800.0;
                tmp0.x = sin(tmp0.x);
                tmp0.xzw = -tmp0.xxx * float3(0.04, 0.04, 0.04) + tmp1.xyz;
                o.sv_target.xyz = tmp0.yyy * tmp0.xzw;
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
	}
}