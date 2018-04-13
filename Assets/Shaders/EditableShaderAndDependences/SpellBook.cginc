#include "UnityCG.cginc"
#include "UnityLightingCommon.cginc"

//variables

		//=================================
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;

		uniform sampler2D _CustomTexture;
		uniform float4 _CustomTexture_ST;
		uniform fixed4 _TextureTint;
		
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform half _NormalMapScale = 1.0f;

		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float _MaxHeightBumpMap = 5.0f;
		uniform float _MaxTexCoordOffset = 3.0f;

		uniform float _CustomAmbientLightForce = 0.75f;
		uniform fixed4 _CustomSpecularColor;
		uniform float _CustomShininess = 1.0f;
		
		uniform float _PhongSpecularPower = 0.5f;
		uniform float _PhongSpecularGlossiness = 0.5f;
		uniform fixed4 _PhongDiffuseColor;
		uniform fixed4 _PhongSpecularColor;

		uniform float _MinnaertRoughness = 0.5f;
		uniform fixed4 _MinnaertDiffuseColor;
		
		uniform float _TextureTileX;
		uniform float _TextureTileY;

		uniform float _OffsetTileX;
		uniform float _OffsetTileY;
		
		uniform int _LightingModel = 2;
		//===========================================
		//=======SPECULAR_HIGHLIGHTS, from: https://en.wikibooks.org/wiki/Cg_Programming/Unity/Specular_Highlights =================
		uniform float4 _Color;		
		uniform float _Shininess;
		uniform float4 _SpecularColor;
		//===========================================

		struct vertexInput 
			{
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
			float3 normal : NORMAL;
			float4 tangent : TANGENT;
			};
		struct vertexOutput 
			{
            float4 pos : SV_POSITION;
			float4 worldPosition : TEXCOORD0;
            float2 tex : TEXCOORD1;
			float3 NormalWorld : TEXCOORD2;
			float3 TangentWorld : TEXCOORD3;
			float3 BitangentWorld : TEXCOORD4;
			float3 viewDirWorld : TEXCOORD5;
			float3 viewDirInScaledSurfaceCoords : TEXCOORD6;

			};	

		struct vertexInput_PosAndGNormal
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};			

		struct vertexOutput_PerVertexLighting
		{
			float4 pos : SV_POSITION;
			float4 col : COLOR;	
		};

		struct vertexOutput_PerPixelLighting
		{
			float4 pos : SV_POSITION;
			float3 posWorld : TEXCOORD0;
			float3 normalDir : TEXCOORD1;
		};

		vertexOutput_PerVertexLighting vert_PerVertexLighting_PhongBase (vertexInput_PosAndGNormal input)
		{
			_Shininess = _CustomShininess;
			_SpecularColor = _CustomSpecularColor;

			vertexOutput_PerVertexLighting output;
			
			float4x4 modelMatrix = unity_ObjectToWorld;
			float3x3 modelMatrixInverse = unity_WorldToObject;
			float3 normalDirection = normalize(mul(input.normal, modelMatrixInverse));
			float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.vertex).xyz);
			
			float3 lightDirection;
			float attenuation;

			if (0.0 == _WorldSpaceLightPos0.w) // directional light
			{
				attenuation = 1.0;
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
			}
			else
			{
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz -	
											 mul(modelMatrix, input.vertex).xyz;
				float3 distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDirection = normalize(vertexToLightSource);
			}

			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
			float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * 
									   max(0.0, dot(normalDirection, lightDirection));

			float3 specularReflection;

			if (dot(normalDirection, lightDirection) < 0.0)
			{	specularReflection = float3 (0.0, 0.0, 0.0);	}
			else
			{		specularReflection = attenuation * _LightColor0.rgb * _SpecularColor.rgb * 
										 pow(max(0.0, dot(reflect(-lightDirection, normalDirection),
														  viewDirection)), _Shininess);
			}

			output.col = float4(ambientLighting + diffuseReflection + specularReflection, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);

			return output;
		}

		vertexOutput_PerVertexLighting vert_PerVertexLighting_Lambert(vertexInput_PosAndGNormal input)
		{
			vertexOutput_PerVertexLighting output;

			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 posWorld = mul(modelMatrix, input.vertex);
			float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

			float3 normalDirection = normalize(normalDir);
			float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz);

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
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDirection = normalize(vertexToLightSource);
			}

			float NDotL = max (0.0, dot(normalDirection, lightDirection));
			float LambertDiffuse = NDotL * _TextureTint.rgb;	
			float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb;

			output.col = float4(finalColor, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
		
			return output;

		}

		vertexOutput_PerVertexLighting vert_PerVertexLighting_HalfLambert(vertexInput_PosAndGNormal input)
		{
			vertexOutput_PerVertexLighting output;

			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 posWorld = mul(modelMatrix, input.vertex);
			float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

			float3 normalDirection = normalize(normalDir);
			float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz);
		
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
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDirection = normalize(vertexToLightSource);
			}

			float3 NDotL = max (0.0, dot(normalDirection, lightDirection));
			float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0) * _TextureTint.rgb;
			float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb;

			output.col = float4(finalColor, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
		
			return output;
		
		}

		vertexOutput_PerVertexLighting vert_PerVertexLighting_PhongAdd (vertexInput_PosAndGNormal input)
		{
			_Shininess = _CustomShininess;
			_SpecularColor = _CustomSpecularColor;

			vertexOutput_PerVertexLighting output;
			
			float4x4 modelMatrix = unity_ObjectToWorld;
			float3x3 modelMatrixInverse = unity_WorldToObject;
			float3 normalDirection = normalize(mul(input.normal, modelMatrixInverse));
			float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.vertex).xyz);
			
			float3 lightDirection;
			float attenuation;

			if (0.0 == _WorldSpaceLightPos0.w) // directional light
			{
				attenuation = 1.0;
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
			}
			else
			{
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz -	
											 mul(modelMatrix, input.vertex).xyz;
				float3 distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDirection = normalize(vertexToLightSource);
			}

			float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * 
									   max(0.0, dot(normalDirection, lightDirection));

			float3 specularReflection;

			if (dot(normalDirection, lightDirection) < 0.0)
			{	specularReflection = float3 (0.0, 0.0, 0.0);	}
			else
			{		specularReflection = attenuation * _LightColor0.rgb * _SpecularColor.rgb * 
										 pow(max(0.0, dot(reflect(-lightDirection, normalDirection),
														  viewDirection)), _Shininess);
			}

			output.col = float4(diffuseReflection + specularReflection, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);

			return output;
		}

		vertexOutput_PerPixelLighting vert_PerPixelLighting (vertexInput_PosAndGNormal input)
		{
			vertexOutput_PerPixelLighting output;
		
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			output.posWorld = mul(modelMatrix, input.vertex);
			output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}

		float4 frag_PerVertexLighting(vertexOutput_PerVertexLighting input) : COLOR
		{	return input.col;	}

		//PhongModel
		float4 frag_PerPixelLighting_Phong (vertexOutput_PerPixelLighting input) : COLOR
		{	
			_Shininess = _CustomShininess;
			_SpecularColor = _CustomSpecularColor;

			float3 normalDirection = normalize(input.normalDir);

			float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);

			float3 lightDirection;
			float attenuation;
		
			if (0.0 == _WorldSpaceLightPos0.w)
			{
				attenuation = 1.0f;
				lightDirection = normalize(_WorldSpaceLightPos0.xyz);
			}
			else
			{
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDirection = normalize(vertexToLightSource);				
			}
			
			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;

			float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * 
										   max(0.0, dot(normalDirection, lightDirection));
				
			float3 specularReflection;
			if (dot(normalDirection, lightDirection) < 0.0)
			{	specularReflection = float3(0.0, 0.0, 0.0);		}
			else 
			{
				specularReflection = attenuation * _LightColor0.rgb * _SpecColor.rgb * 
									 pow(max(0.0, dot(reflect(-lightDirection, normalDirection),
													  viewDirection)), _Shininess);				
			}	

			return float4(ambientLighting + diffuseReflection + specularReflection, 1.0);		
		}

		float4 frag_PerPixelLighting_Lambert(vertexOutput_PerPixelLighting input) : COLOR
		{

			float3 normalDirection = normalize(input.normalDir);
			float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);

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
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDirection = normalize(vertexToLightSource);
			}

			float NDotL = max (0.0, dot(normalDirection, lightDirection));
			float LambertDiffuse = NDotL * _TextureTint.rgb;	
			float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb;

			return float4 (finalColor, 1.0f);		
		}

		float4 frag_PerPixelLighting_HalfLambert(vertexOutput_PerPixelLighting input) : COLOR
		{
			float3 normalDirection = normalize(input.normalDir);
			float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
		
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
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
				float distance = length(vertexToLightSource);
				attenuation = 1.0 / distance;
				lightDirection = normalize(vertexToLightSource);
			}

			float3 NDotL = max (0.0, dot(normalDirection, lightDirection));
			float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0) * _TextureTint.rgb;
			float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb;

			return float4 (finalColor, 1.0);		
		}

		float4 frag_PerPixelLighting_NoLight(vertexOutput_PerPixelLighting input) : COLOR
		{
			_SpecularColor = _CustomSpecularColor;
			return (_SpecularColor);
		
		}
		
		vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
			
			fixed2 tileVector = fixed2 (_TextureTileX, _TextureTileY);
			fixed2 offsetVector = fixed2 (_OffsetTileX, _OffsetTileY);
			_NormalMap_ST = float4 (tileVector, offsetVector ); 
			_MainTex_ST = float4 (tileVector, offsetVector );
			_BumpMap_ST = float4 (tileVector, offsetVector );
			_CustomTexture_ST = float4 (tileVector, offsetVector );

		
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

			//output.tex = TRANSFORM_TEX(input.texcoord, _CustomTexture);
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
				else if (LightModel == 4) //Phong Lighting Model, based on code from: http://www.jordanstevenstechart.com/lighting-models
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
						attenuation = 1.0 / pow(distance + 1, 2);
						lightDirection = normalize(vertexToLightSource);
					}
					
					float3 lightReflectDirection = reflect (-lightDirection, normalDirection);
					float NDotL = max (0.0, dot (normalDirection, lightDirection));
					float RDotV = max (0.0, dot (lightReflectDirection, viewDirection));
					//Specular Calculations
					float3 specularity = pow (RDotV, _PhongSpecularGlossiness / 4) 
										 * _PhongSpecularPower * _PhongSpecularColor.rgb;

					float3 lightingModel = NDotL * _PhongDiffuseColor + specularity;
					float3 attenColor = attenuation * _LightColor0.rgb;
					float4 finalDiffuse = float4 (lightingModel * attenColor, 1.0f);
					
					return finalDiffuse;

				}
				else
				{	return float4(1.0f, 1.0f, 1.0f, 1.0f);	}
		}



		 
		 float4 frag(vertexOutput input) : COLOR
         {
			//Overwritting deffault Values with custom Values
			//Also, depending on the lighting model, we need to update the necessary variables...					
			_MainTex = _CustomTexture;		
			
			//Recuerda, los floats de cada textura apodados textura_ST significa (Scale Translate)
			//para procesar el tileado y offset de las texturas.

			//Parallax time!
			
			float height = _MaxHeightBumpMap * (-0.5 + tex2D(_BumpMap, _BumpMap_ST.xy * input.tex.xy + _BumpMap_ST.zw).x);
			float2 texCoordOffsets = clamp (height * input.viewDirInScaledSurfaceCoords.xy / 
											input.viewDirInScaledSurfaceCoords.z, - _MaxTexCoordOffset, +_MaxTexCoordOffset);

			// doing actual work (to remove bump map: remove texCoordOffsets from encodedNormal)
			float4 encodedNormal = tex2D(_NormalMap, _NormalMap_ST.xy * (input.tex.xy + texCoordOffsets) + _NormalMap_ST.zw);
			float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0, 1.0), 0.0);
			localCoords.z = 1.0 - 0.5 * dot (localCoords, localCoords);

			float3x3 local2WorldTranspose = float3x3(input.TangentWorld, input.BitangentWorld, input.NormalWorld);
			float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));


			
			 float4 lightingModelCalculation = LightingModelsResult(_LightingModel, input, normalDirection);

			 float4 finalColor = tex2D(_CustomTexture, ((input.tex.xy + texCoordOffsets) * _CustomTexture_ST.xy + _CustomTexture_ST.zw))
								 * lightingModelCalculation;
			 return finalColor;

         }
		
		
		//=====================================================================================================
		//=====================================================================================================
		//=====================================================================================================
		//=================obsolete functions, review code for total destruction without making errors ===========
		//=======================================================================================================
		//=======================================================================================================
		//=====================================================================================================

		// LIGHTINGMODELSRESULT
		
		

        