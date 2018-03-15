// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			//#include "ShaderonomiconLibrary.cginc"
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct vertexInput 
			{
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
			float3 normal : NORMAL;
			};
			struct vertexOutput 
			{
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
			float3 worldNormal : TEXCOORD1;
			};
 
		uniform sampler2D _MainTex;
		
		sampler2D _CustomTexture;
		
		sampler2D _NormalMap;
		half _NormalMapScale = 1.0f;

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
			output.worldNormal = UnityObjectToWorldNormal(input.normal);

            return output;
         }
         float4 frag(vertexOutput input) : COLOR
         {
			_MainTex = _CustomTexture;

			float3 normalDirection = normalize(input.worldNormal);		
			
			float nl = max(0.0, dot(normalDirection, _WorldSpaceLightPos0.xyz));
			float4 diffuseTerm = nl * _TextureTint * tex2D(_MainTex, input.tex.xy) * _LightColor0; 

			
            return  diffuseTerm;	
               // look up the color of the texture image specified by 
               // the uniform "_MainTex" at the position specified by 
               // "input.tex.x" and "input.tex.y" and return it
 
         }
 
         ENDCG
		}
	}
}
