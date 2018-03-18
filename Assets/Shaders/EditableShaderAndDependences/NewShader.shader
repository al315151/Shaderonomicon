// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "LightMode" = "ForwardBase" }
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
			float4 tangent : TANGENT;
			};
			struct vertexOutput 
			{
            float4 pos : SV_POSITION;
			float4 worldPosition : TEXCOORD0;
            float4 tex : TEXCOORD1;
			float3 NormalWorld : TEXCOORD2;
			float3 TangentWorld : TEXCOORD3;
			float3 BitangentWorld : TEXCOORD4;


			};
 
		//variables

		uniform sampler2D _MainTex;
		
		sampler2D _CustomTexture;
		fixed4 _TextureTint = fixed4(1.0, 1.0, 1.0, 1.0);
		
		sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		half _NormalMapScale = 1.0f;

		uniform float _CustomAmbientLightForce = 0.75f;
		uniform fixed4 _CustomSpecularColor = float4(1.0f, 1.0f, 1.0f, 1.0f);
		uniform float _CustomShininess = 1.0f;
		


		//Quick normal maths
		

         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			output.TangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz);
			output.NormalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			output.BitangentWorld = normalize(cross(output.NormalWorld, output.TangentWorld) * input.tangent.w);

			output.worldPosition = mul(modelMatrix, input.vertex);
			output.tex = input.texcoord;
			output.pos = UnityObjectToClipPos(input.vertex);

            return output;
         }
		 

         float4 frag(vertexOutput input) : COLOR
         {
			//Overwritting deffault Values with custom Values
			_MainTex = _CustomTexture;
			


			// doing actual work
			float4 encodedNormal = tex2D(_NormalMap, _NormalMap_ST.xy * input.tex.xy + _NormalMap_ST.zw);
			float3 localCoords = float3(2.0 * encodedNormal.a - 1.0, 2.0 * encodedNormal.g - 1.0, 0.0);
			
			//Con uso de sqrt: mas preciso pero mas costoso
			//localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));
			//Sin uso de sqrt: aproximacion de valor, pero mas barata
			localCoords.z = 1-0 - 0.5 * dot (localCoords, localCoords);

			float3x3 local2WorldTranspose = float3x3(input.TangentWorld, input.BitangentWorld, input.NormalWorld);
			float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));
			float3 viewDirection = normalize(_WorldSpaceCameraPos - input.worldPosition.xyz);
			
			float3 lightDirection;
			float attenuation;
			
			//light type consideration

			if (0.0 == _WorldSpaceLightPos0.w)
			{
				attenuation = 1.0f;
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);			
			}
			else
			{
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.worldPosition.xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDirection = normalize(vertexToLightSource);
			}

			// More calculations
			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _TextureTint.rgb;
			float3 diffuseReflection = attenuation * _LightColor0.rgb * _TextureTint.rgb
									   * max (0.0f, dot (normalDirection, lightDirection));

			//Is light on the right side?
			float3 specularReflection; 
			if (dot (normalDirection, lightDirection) < 0.0) {specularReflection = float3 (0.0, 0.0, 0.0);}
			else
			{
				specularReflection = attenuation * _LightColor0.rgb * _CustomSpecularColor.rgb * 
									 pow(max(0.0, dot(reflect(-lightDirection, normalDirection),
									 viewDirection)), _CustomShininess);
			
			}
			 

			 float4 finalColor = tex2D(_MainTex, input.tex.xy) * float4(ambientLighting + diffuseReflection + specularReflection, 1.0f);
			 return finalColor;

         }
 
         ENDCG
		}
	}
}
