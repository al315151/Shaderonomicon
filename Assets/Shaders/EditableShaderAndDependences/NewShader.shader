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
		Tags { "LightMode" = "ForwardBase" "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			//#include "ShaderonomiconLibrary.cginc"
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			//variables

		uniform sampler2D _MainTex;


		uniform sampler2D _CustomTexture;
		uniform fixed4 _TextureTint = fixed4(1.0, 1.0, 1.0, 1.0);
		
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform half _NormalMapScale = 1.0f;

		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float _MaxHeightBumpMap = 5.0f;
		uniform float _MaxTexCoordOffset = 3.0f;

		uniform float _CustomAmbientLightForce = 0.75f;
		uniform fixed4 _CustomSpecularColor = float4(1.0f, 1.0f, 1.0f, 1.0f);
		uniform float _CustomShininess = 1.0f;
		
		uniform float _PhongSpecularPower = 0.5f;
		uniform float _PhongSpecularGlossiness = 0.5f;
		uniform fixed4 _PhongDiffuseColor = fixed4(1.0f, 1.0f, 1.0f, 1.0f);
		uniform fixed4 _PhongSpecularColor = fixed4(1.0f, 1.0f, 1.0f, 1.0f);

		uniform float _MinnaertRoughness = 0.5f;
		uniform fixed4 _MinnaertDiffuseColor = fixed4(1.0f, 1.0f, 1.0f, 1.0f);


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
			float3 viewDirWorld : TEXCOORD5;
			float3 viewDirInScaledSurfaceCoords : TEXCOORD6;

			};	


		//Quick normal maths
		
		

         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			output.TangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz);
			output.NormalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			output.BitangentWorld = normalize(cross(output.NormalWorld, output.TangentWorld) * input.tangent.w);

			float3 biNormal = cross (input.normal, input.tangent.xyz) * input.tangent.w;
			//scaled tangent and biNormal aprroximations
			//to map distances from object space to Texture space.

			float3 viewDirInObjectCoords = mul (modelMatrixInverse, float4(_WorldSpaceCameraPos, 1.0).xyz) - 
												input.vertex.xyz;
			float3x3 localSurface2ScaledObjectT = float3x3(input.tangent.xyz, biNormal, input.normal);
			//VECTORS ARE ORTHOGONAL.

			output.viewDirInScaledSurfaceCoords = mul (localSurface2ScaledObjectT, viewDirInObjectCoords);
			//we multiply with the ptranspose to multiply with the "inverse"
			
			output.worldPosition = mul(modelMatrix, input.vertex);
			output.viewDirWorld = normalize(_WorldSpaceCameraPos - output.worldPosition.xyz);

			output.tex = input.texcoord;
			output.pos = UnityObjectToClipPos(input.vertex);

            return output;
         }
		 
		 float4 LightingModelsResult(int LightModel, vertexOutput input, float3 normalDirection)
		{
				if (LightModel == 1) // Phong Lighting Model
				{
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
					
					return float4(ambientLighting + diffuseReflection + specularReflection, 1.0f);				
					}
				else if (LightModel == 2) // Lambert Lighting Model, based on code from: http://www.jordanstevenstechart.com/lighting-models
				{
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

					float NDotL = max (0.0, dot(normalDirection, lightDirection));
					float LambertDiffuse = NDotL * _TextureTint.rgb;	
					float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb;

					return float4 (finalColor, 1.0f);
				}
				else if (LightModel == 3) // Half-Lambert Lighting Model, based on code from: http://www.jordanstevenstechart.com/lighting-models
				{
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

					float3 NDotL = max (0.0, dot(normalDirection, lightDirection));
					float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0) * _TextureTint.rgb;
					float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb;

					return float4 (finalColor, 1.0);
					
				}
				else
				{	return float4(1.0f, 1.0f, 1.0f, 1.0f);	}
		}
		
         float4 frag(vertexOutput input) : COLOR
         {
			//Overwritting deffault Values with custom Values
			//Also, depending on the lighting model, we need to update the necessary variables...
			_MainTex = _CustomTexture;
			
			//Parallax time!
			
			float height = _MaxHeightBumpMap * (-0.5 + tex2D(_BumpMap, _BumpMap_ST.xy * input.tex.xy + _BumpMap_ST.zw).x);
			float2 texCoordOffsets = clamp (height * input.viewDirInScaledSurfaceCoords.xy / 
											input.viewDirInScaledSurfaceCoords.z, - _MaxTexCoordOffset, +_MaxTexCoordOffset);

			// doing actual work (to remove bump map: remove texCoordOffsets from encodedNormal)
			float4 encodedNormal = tex2D(_NormalMap, _NormalMap_ST.xy * (input.tex.xy + texCoordOffsets) + _NormalMap_ST.zw);
			float3 localCoords = float3(2.0 * encodedNormal.a - 1.0, 2.0 * encodedNormal.g - 1.0, 0.0);
			
			//Con uso de sqrt: mas preciso pero mas costoso
			localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));
			//Sin uso de sqrt: aproximacion de valor, pero mas barata
			//localCoords.z = 1-0 - 0.5 * dot (localCoords, localCoords);

			float3x3 local2WorldTranspose = float3x3(input.TangentWorld, input.BitangentWorld, input.NormalWorld);
			float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));


			
			 float4 lightingModelCalculation = LightingModelsResult(2, input, normalDirection);

			 float4 finalColor = tex2D(_MainTex, input.tex.xy) * lightingModelCalculation;
			 return finalColor;

         }
 
         ENDCG
		}
	}
}
