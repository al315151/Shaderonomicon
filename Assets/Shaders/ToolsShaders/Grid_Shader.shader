﻿Shader "Tools/GridShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_GridColor("Grid Color", Color) = (0.6, 0.6, 0.6, 0.0)
		_SquareRows("SquareRows", float) = 50
		_AlphaFactor("Grid Alpha Factor", Range(0.0, 1.0)) = 0.6
		_GridFadeFactor("Grid Fade Factor", float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"= "Opaque" }
		LOD 100

		//ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
			fixed4 _GridColor;
			float _SquareRows;
			float _AlphaFactor;
			float _GridFadeFactor;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				
				_GridColor = fixed4(0.8f, 0.8f, 0.8f, 1.0f);

				half _GridCoordX = i.uv.x * _SquareRows;
				half _GridCoordY = i.uv.y * _SquareRows;

				float distanceFromCenter = max(abs(i.uv.x- 0.5f), abs(i.uv.y - 0.5f));

				if (frac(_GridCoordX) < 0.1f || frac(_GridCoordX) > 0.9f ||
					frac(_GridCoordY) < 0.1f || frac(_GridCoordY) > 0.9f)
					{	_GridColor.a = clamp(0.0f, _AlphaFactor, _AlphaFactor - distanceFromCenter * _GridFadeFactor);	}
				else {_GridColor.a = 0.0f;}

				return _GridColor;
			}
			ENDCG
		}
	}
}