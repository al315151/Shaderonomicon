// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewShader"
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

			struct vertexInput 
			{
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
			};
			struct vertexOutput 
			{
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
			};
 
		uniform sampler2D _MainTex;
		sampler2D _CustomTexture;
		fixed4 _TextureTint;

         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.tex = input.texcoord;
               // Unity provides default longitude-latitude-like 
               // texture coordinates at all vertices of a 
               // sphere mesh as the input parameter 
               // "input.texcoord" with semantic "TEXCOORD0".
            output.pos = UnityObjectToClipPos(input.vertex);
            return output;
         }
         float4 frag(vertexOutput input) : COLOR
         {
			_MainTex = _CustomTexture;
            return tex2D(_MainTex, input.tex.xy) * _TextureTint;	
               // look up the color of the texture image specified by 
               // the uniform "_MainTex" at the position specified by 
               // "input.tex.x" and "input.tex.y" and return it
 
         }
 
         ENDCG
		}
	}
}
