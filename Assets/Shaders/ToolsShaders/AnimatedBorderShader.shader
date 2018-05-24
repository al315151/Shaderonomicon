Shader "Tools/AnimatedBorderShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BorderColor("Border Color", Color) = (1.0, 1.0, 1.0, 1.0)
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _BorderColor;
			
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
				fixed4 col = tex2D(_MainTex, i.uv);
				
				if (frac(i.uv.x) < 0.1f || frac(i.uv.x) > 0.9f ||
					frac (i.uv.y) < 0.1f || frac(i.uv.y) > 0.9f)
					{
						col.rgb = _BorderColor.rgb;
						col.a = 0.5 * (sin(_Time.y) + 1);
					}

				return col;
			}
			ENDCG
		}
	}
}
