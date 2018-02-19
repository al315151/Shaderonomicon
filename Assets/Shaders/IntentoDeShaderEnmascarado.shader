// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MASKEDColorBSShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			//Code obtained in https://www.shadertoy.com/view/XljGzV
			// I am not propietary of the following functions, 
			// They may be used as pleased, except for monetary purposes.

			float4 _HUESelected;

			float3 FromHSLToRGB ( float3 cec)
			{
				float3 rgb = clamp(
							 abs(
							 mul(cec.x * 6.0f + 
							 float3(0.0f, 4.0f, 2.0f), 6.0f) - 3.0f) -1.0f, 0.0f, 1.0f);
			
				return cec.z + cec.y * (rgb- 0.5)* (1.0 - abs(2.0 * cec.z - 1.0));
			}

			float3 HUEShift ( float3 color, float Shift)
			{
				float3 P = float3(0.55735f, 0.55735f, 0.55735f) * dot(float3(0.55735f, 0.55735f, 0.55735f), color);
				float3 U = color - P;
				float3 V = cross(float3(0.55735f, 0.55735f, 0.55735f), U);
				color = U*cos(Shift * 6.2832f) + V*sin(Shift * 6.2832f) + P;
				return float3(color);
			}

			float3 FromRGBToHSL (float3 cec)
			{
				float h = 0.0f;
				float s = 0.0f;
				float l = 0.0f;
				float r = cec.r;
				float g = cec.g;
				float b = cec.b;

				float cMin = min(r, min(g,b));
				float cMax = max(r, max(g,b));

				l = (cMax + cMin) / 2.0f;
				if (cMax > cMin)
				{
					float cDelta = cMax - cMin;
					s = l < 0.0f ? cDelta / (cMax + cMin) : cDelta / (2.0 - (cMax + cMin) );
					
					if (r == cMax)	{	h = (g - b) / cDelta;	}
					else if (g == cMax){h = 2.0f + (b - r) / cDelta;	}
					else			{	h = 4.0f + (r - g) / cDelta;	}
					
					if (h < 0.0f) {h += 6.0f;}
					h = h / 6.0f;
				}
				return float3(h,s,l);			
			}

			float3 FromRGBToHSV (float3 cec)
			{
				float4 K = float4 (0.0f, -1.0f / 3.0f, 2.0f / 3.0f, -1.0f );
				float4 P = lerp(float4 (cec.bg, K.wz), float4(cec.gb, K.xy), step(cec.b, cec.g));
				float4 Q = lerp(float4 (P.xyw, cec.r), float4(cec.r, P.yzx), step(P.x, cec.r));
				float D = Q.x - min(Q.w, Q.y);
				float E = 1.0e-10;
				return float3(abs(Q.z + (Q.w + Q.y) / (6.0f * D + E)), D / (Q.x + E), Q.x);
			}

			float3 FromHSVToRGB (float3 cec)
			{
				float4 K = float4 (1.0f, 2.0f / 3.0f, 1.0f / 3.0f, 3.0f);
				float3 P = abs(frac(cec.xxx + K.xyz) * 6.0f - K.www);
				return cec.z * lerp(K.xxx, clamp(P - K.xxx, 0.0f, 1.0f), cec.y);		
			}

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float2 newUV = (i.uv);
				fixed4 col = tex2D(_MainTex, i.uv);	
				float3 HUEConverted = FromRGBToHSV (_HUESelected.rgb);
				float3 result = FromHSVToRGB(float3(HUEConverted.x, (newUV.x), (newUV.y)));
				

				return float4(result, 1.0f);
			}
			ENDCG
		}
	}
}
