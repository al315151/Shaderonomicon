#include "UnityCG.cginc"
#include "UnityLightingCommon.cginc"

		//variables
		//=================================
		
		uniform sampler2D _CustomTexture;
		uniform fixed4 _TextureTint;
		uniform float _TextureTileX;
		uniform float _TextureTileY;
		uniform float _OffsetTileX;
		uniform float _OffsetTileY;
		
		uniform sampler2D _NormalMap;
		uniform float _NormalTileX;
		uniform float _NormalTileY;
		uniform float _NormalOffsetX;
		uniform float _NormalOffsetY;

		uniform half _NormalMapScale = 1.0f;

		uniform float _CustomShininess;		
		uniform float4 _PhongAmbientColor;
		uniform float _PhongAmbientForce;
		uniform float4 _PhongSpecularColor;
		uniform float _PhongSpecularForce;
		uniform float4 _PhongDiffuseColor;
		uniform float _PhongDiffuseForce;

		uniform float _LambertTintForce;
		uniform float4 _LambertTintColor;

		//===========================================
		//=======SPECULAR_HIGHLIGHTS, from: https://en.wikibooks.org/wiki/Cg_Programming/Unity/Specular_Highlights =================

		//====STRUCTS AND VARIABLES ONLY USED IN SHADER EDITING, THEY SHALL NOT BE USED AT THE FINAL SHADER ===================

		uniform float _LightingModel;
		uniform float _IsPixelLighting;
		uniform float _IsNormalMapApplied;
		uniform float _IsTextureApplied;

		//==============================================================================================================



		struct vertexInput_AllVariables
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
			float4 tangent : TANGENT;
		};			

		struct vertexOutput_PerVertexLighting
		{
			float4 pos : SV_POSITION;
			float4 col : COLOR;	
			float2 tex : TEXCOORD1;
		};

		struct vertexOutput_PerPixelLighting
		{
			float4 pos : SV_POSITION;
			float3 posWorld : TEXCOORD0;
			float3 normalDir : TEXCOORD1;
			float2 tex : TEXCOORD2;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
		};

		struct vertexInput_NoTextureNoNormalMap
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;			
		};

		struct vertexOutput_NoTextureNoNormalMap_PerVertexLighting
		{
			float4 pos : SV_POSITION;
			float4 col : COLOR;
		};

		struct vertexOutput_NoTextureNoNormalMap_PerPixelLighting
		{
			float4 pos : SV_POSITION;
			float3 posWorld : TEXCOORD0;
			float3 normalDir : TEXCOORD1;
			float3 normal : NORMAL;
		};

		struct vertexInput_NoNormalMap
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		struct vertexOutput_NoNormalMap_PerPixelLighting
		{
			float4 pos : SV_POSITION;
			float3 posWorld : TEXCOORD0;
			float3 normalDir : TEXCOORD1;
			float3 normal : NORMAL;
			float2 tex : TEXCOORD2;
		};

		struct  vertexInput_NoLight
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
			float4 tangent : TANGENT;
		};

		struct vertexInput_NoLight_NoTextureNoNormalMap
		{
			float4 vertex : POSITION;
		};

		struct vertexOutput_NoLight_NoTextureNoNormalMap
		{
			float4 pos : SV_POSITION;
			float4 col : COLOR;
		};

		struct vertexOutput_AllVariables
		{
			float4 pos : SV_POSITION;
			float4 col : COLOR;	
			float2 tex : TEXCOORD3;
			float3 posWorld : TEXCOORD4;
			float3 normalDir : TEXCOORD5;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;		
		};
		//=======================================================================================================
		

		// =============SUB-FUNCTIONS FOR TEXTURE HANDLING, NORMAL MAP HANDLING ==================
		
		float4 Texture_Handling_Vertex(vertexOutput_PerVertexLighting input)
		{
			float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY);
			texCoordsScale *= input.tex.xy;
			texCoordsScale += float2(_OffsetTileX, _OffsetTileY);
			float4 textureColor = tex2Dlod(_CustomTexture, float4(texCoordsScale, 0, 0));
			textureColor = textureColor * _TextureTint;
			return textureColor;
		}

		float4 Texture_Handling_Pixel(vertexOutput_PerPixelLighting input)
		{
			float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY);
			texCoordsScale *= input.tex.xy;

			float4 textureColor  = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY));
			textureColor = textureColor * _TextureTint;
			return textureColor;
		}

		float4 Texture_Handling_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input)
		{
			float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY);
			texCoordsScale *= input.tex.xy;

			float4 textureColor  = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY));
			textureColor = textureColor * _TextureTint;
			return textureColor;
		}

		
		float3 Normal_Direction_With_Normal_Map_Handling_Vertex(vertexInput_AllVariables input)
		{
			
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 tangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz);
			float3 normalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			float3 BitangentWorld = normalize(cross(normalWorld, tangentWorld) * input.tangent.w);
			
			float3 biNormal = cross (input.normal, input.tangent.xyz) * input.tangent.w;
			//scaled tangent and biNormal aprroximations
			//to map distances from object space to Texture space.

			float2 normalCoordsScaled = float2 (_NormalTileX, _NormalTileY);
			normalCoordsScaled *= input.texcoord.xy;
			normalCoordsScaled += float2(_NormalOffsetX, _NormalOffsetY);
			float4 encodedNormal = tex2Dlod(_NormalMap, float4(normalCoordsScaled.xy,0, 0));

			float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0, 1.0), 0.0);
			localCoords.z = 1.0 - 0.5 * dot (localCoords, localCoords);

			float3x3 local2WorldTranspose = float3x3(tangentWorld, BitangentWorld, normalWorld);
			float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));

			normalDirection = float3(_NormalMapScale, _NormalMapScale, 1.0f) * normalDirection;

			return normalDirection;
		
		}

		float3 Normal_Direction_With_Normal_Map_Handling_Pixel(vertexOutput_PerPixelLighting input)
		{
			
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 tangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz);
			float3 normalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			float3 BitangentWorld = normalize(cross(normalWorld, tangentWorld) * input.tangent.w);
			
			float3 biNormal = cross (input.normal, input.tangent.xyz) * input.tangent.w;
			//scaled tangent and biNormal aprroximations
			//to map distances from object space to Texture space.

			float2 normalCoordsScaled = float2 (_NormalTileX, _NormalTileY);
			normalCoordsScaled *= input.tex.xy;
			float4 encodedNormal = tex2D(_NormalMap, (normalCoordsScaled + float2(_NormalOffsetX, _NormalOffsetY)).xy);

			float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0, 1.0), 0.0);
			localCoords.z = 1.0 - 0.5 * dot (localCoords, localCoords);

			float3x3 local2WorldTranspose = float3x3(tangentWorld, BitangentWorld, normalWorld);
			float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));
			
			normalDirection = float3(_NormalMapScale, _NormalMapScale, 1.0f) * normalDirection;

			return normalDirection;
		
		}

		//========================================================================================================

		float3 Phong_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection)
		{
			float4x4 modelMatrix = unity_ObjectToWorld;
			float3x3 modelMatrixInverse = unity_WorldToObject;
			// normalDirection += normalize(mul(input.normal, modelMatrixInverse));
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

			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb;
			float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * 
									   max(0.0, dot(normalDirection, lightDirection));

			float3 specularReflection;

			if (dot(normalDirection, lightDirection) < 0.0)
			{	specularReflection = float3 (0.00001, 0.00001, 0.00001);	}
			else
			{
					specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb *
											max(0.0, dot(reflect(-lightDirection, normalDirection),
																				viewDirection));					
					specularReflection = specularReflection * _CustomShininess;
			}

			  return (ambientLighting * _PhongAmbientForce) + (diffuseReflection * _PhongDiffuseForce) 
						  + (specularReflection * _PhongSpecularForce);
		}

		float3 Phong_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input)
		{
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

			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb;
			float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * 
									   max(0.0, dot(normalDirection, lightDirection));

			float3 specularReflection;

			if (dot(normalDirection, lightDirection) < 0.0)
			{	specularReflection = float3 (0.00001, 0.00001, 0.00001);	}
			else
			{
				specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb *
									max(0.0, dot(reflect(-lightDirection, normalDirection),
									viewDirection));
				specularReflection = specularReflection * _CustomShininess;
			}

			  return (ambientLighting * _PhongAmbientForce) + (diffuseReflection * _PhongDiffuseForce) 
						  + (specularReflection * _PhongSpecularForce);
		}
		
		float4 Lambert_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection)
		{
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 posWorld = mul(modelMatrix, input.vertex);
			float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

			normalDirection += normalize(normalDir);
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
			float LambertDiffuse = NDotL;	
			float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb;
			return float4 (finalColor, 1.0f);
		}

		float4 Lambert_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input)
		{
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 posWorld = mul(modelMatrix, input.vertex);
			float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

			//float3 normalDirection = normalize(normalDir);
			float3 normalDirection = normalize(mul(input.normal, modelMatrixInverse));
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
			float LambertDiffuse = NDotL;	
			float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb;
			return float4 (finalColor, 1.0f);
		}

		float4 HalfLambert_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection)
		{
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 posWorld = mul(modelMatrix, input.vertex);
			float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

			normalDirection += normalize(normalDir);
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
			float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0);
			float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb;
			return float4 (finalColor, 1.0f);
		}

		float4 HalfLambert_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input)
		{
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 posWorld = mul(modelMatrix, input.vertex);
			float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

			//float3 normalDirection = normalize(normalDir);
			float3 normalDirection = normalize(mul(input.normal, modelMatrixInverse));
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
			float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0);
			float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb;
			return float4 (finalColor, 1.0f);
		}

		//=======================================================================================================

		float3 Phong_Lighting_Pixel (vertexOutput_PerPixelLighting input, float3 normalDirection)
		{
			normalDirection += normalize(input.normalDir);

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
			
			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb;

			float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * 
										   max(0.0, dot(normalDirection, lightDirection));
				
			float3 specularReflection;
			if (dot(normalDirection, lightDirection) < 0.0)
			{	specularReflection = float3(0.00001, 0.00001, 0.00001);		}
			else 
			{
				specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * 
									 max(0.0, dot(reflect(-lightDirection, normalDirection),
													  viewDirection));
				specularReflection = specularReflection * _CustomShininess;
			}	

			return float3(ambientLighting * _PhongAmbientForce + diffuseReflection * _PhongDiffuseForce 
						  + specularReflection * _PhongSpecularForce);
		}

		float3 Phong_Lighting_Pixel_NoNormalMap (vertexOutput_NoNormalMap_PerPixelLighting input)
		{
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
			
			float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb;

			float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * 
										   max(0.0, dot(normalDirection, lightDirection));
				
			float3 specularReflection;
			if (dot(normalDirection, lightDirection) < 0.0)
			{	specularReflection = float3(0.00001, 0.00001, 0.00001);		}
			else 
			{
				specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * 
									 max(0.0, dot(reflect(-lightDirection, normalDirection),
													  viewDirection));
				specularReflection = specularReflection * _CustomShininess;
			}	

			return float3(ambientLighting * _PhongAmbientForce + diffuseReflection * _PhongDiffuseForce 
						  + specularReflection * _PhongSpecularForce);
		}

		float3 Lambert_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection)
		{

			normalDirection += normalize(input.normalDir);
			
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
			float LambertDiffuse = NDotL;	
			float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb;
			return finalColor;
		}

		float3 Lambert_Lighting_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input)
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
			float LambertDiffuse = NDotL;	
			float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb;
			return finalColor;
		}

		float3 HalfLambert_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection)
		{
			
			normalDirection += normalize(input.normalDir);
		
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
			float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0);
			float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb;
			return finalColor;

		}

		float3 HalfLambert_Lighting_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input)
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
			float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0);
			float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb;
			return finalColor;

		}

		//======================================================================================================

		vertexOutput_PerVertexLighting vert_PerVertexLighting_Phong (vertexInput_AllVariables input) : COLOR
		{
			vertexOutput_PerVertexLighting output;			
			
			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input);

			output.col = float4(Phong_Lighting_Vertex(input, normalDirection), 1.0f);
			output.pos = UnityObjectToClipPos(input.vertex);
			output.tex = input.texcoord;

			/*float4 textureOutput = Texture_Handling_Vertex(output);
			output.col *= textureOutput;
			*/
			return output;
		}

		vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_Phong_NoNormalMap (vertexInput_NoTextureNoNormalMap input)
		{
			vertexOutput_NoTextureNoNormalMap_PerVertexLighting output;

			output.col = float4(Phong_Lighting_Vertex_NoNormalMap(input), 1.0f);
			output.pos = UnityObjectToClipPos(input.vertex);

			return output;
		}

		vertexOutput_PerVertexLighting vert_PerVertexLighting_Lambert(vertexInput_AllVariables input)
		{
			vertexOutput_PerVertexLighting output;

			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input);

			output.col = float4(Lambert_Lighting_Vertex(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
			output.tex = input.texcoord;

			/*float4 textureOutput = Texture_Handling_Vertex(output);
			output.col *= textureOutput;
			*/
			return output;

		}

		vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_Lambert_NoNormalMap(vertexInput_NoTextureNoNormalMap input)
		{
			vertexOutput_NoTextureNoNormalMap_PerVertexLighting output;

			output.col = float4(Lambert_Lighting_Vertex_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;

		}

		vertexOutput_PerVertexLighting vert_PerVertexLighting_HalfLambert(vertexInput_AllVariables input)
		{
			vertexOutput_PerVertexLighting output;
			
			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input);

			output.col = float4(HalfLambert_Lighting_Vertex(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
			output.tex = input.texcoord;

			/*float4 textureOutput = Texture_Handling_Vertex(output);
			output.col *= textureOutput;
			*/
			return output;
		
		}

		vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_HalfLambert_NoNormalMap(vertexInput_NoTextureNoNormalMap input)
		{
			vertexOutput_NoTextureNoNormalMap_PerVertexLighting output;

			output.col = float4(HalfLambert_Lighting_Vertex_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;

		}

		vertexOutput_PerVertexLighting vert_PerVertexLighting_NoLight (vertexInput_AllVariables input)
		{
			vertexOutput_PerVertexLighting output;

			output.col = float4 (1.0f, 1.0f, 1.0f, 1.0f);
			output.pos = UnityObjectToClipPos(input.vertex);
			output.tex = input.texcoord;

			/*float4 textureOutput = Texture_Handling_Vertex(output);
			output.col *= textureOutput;
			*/
			return output;
		}

		vertexOutput_NoLight_NoTextureNoNormalMap vert_PerVertexLighting_NoLight_NoTextureNoNormalMap (vertexInput_NoLight_NoTextureNoNormalMap input)
		{
			vertexOutput_NoLight_NoTextureNoNormalMap output;
			
			output.pos = UnityObjectToClipPos(input.vertex);
			output.col = float4 (1.0f, 1.0f, 1.0f, 1.0f);
			return output;
		}

		float4 frag_PerVertexLighting(vertexOutput_PerVertexLighting input) : COLOR
		{	
			float4 TextureColor = Texture_Handling_Vertex(input);
			return float4(input.col.xyz * TextureColor.xyz , 1.0f);
		}

		float4 frag_PerVertexLighting_NoTextureMap (vertexOutput_NoTextureNoNormalMap_PerVertexLighting input) : COLOR
		{	return float4(input.col.xyz * _TextureTint.xyz, 1.0f);		}


		//=============================================================================================================

		vertexOutput_PerPixelLighting vert_PerPixelLighting (vertexInput_AllVariables input) : COLOR
		{
			vertexOutput_PerPixelLighting output;
		
			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			output.posWorld = mul(modelMatrix, input.vertex);
			output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			output.tex = input.texcoord;
			output.tangent = input.tangent;
			output.normal = input.normal;
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}	

		vertexOutput_NoTextureNoNormalMap_PerPixelLighting vert_PerPixelLighting_NoTextureNoNormalMap(vertexInput_NoTextureNoNormalMap input) : COLOR
		{
			vertexOutput_NoTextureNoNormalMap_PerPixelLighting output;

			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			output.posWorld = mul(modelMatrix, input.vertex);
			output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			output.normal = input.normal;
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}	

		vertexOutput_NoNormalMap_PerPixelLighting vert_PerPixelLighting_NoNormalMap (vertexInput_NoNormalMap input) : COLOR
		{
			vertexOutput_NoNormalMap_PerPixelLighting output;

			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			output.posWorld = mul(modelMatrix, input.vertex);
			output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
			output.tex = input.texcoord;
			output.normal = input.normal;
			output.pos = UnityObjectToClipPos(input.vertex);
			return output;
		}

		//PhongModel
		float4 frag_PerPixelLighting_Phong (vertexOutput_PerPixelLighting input) : COLOR
		{	
			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input);

			return float4(Texture_Handling_Pixel(input).xyz * Phong_Lighting_Pixel(input, normalDirection), 1.0f);		
		}

		float4 frag_PerPixelLighting_Phong_NoNormalMap (vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR
		{
			return float4(Texture_Handling_Pixel_NoNormalMap(input).xyz * Phong_Lighting_Pixel_NoNormalMap(input), 1.0f);
		}

		float4 frag_PerPixelLighting_Phong_NoTexture(vertexOutput_PerPixelLighting input) : COLOR
		{
			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input);

			return float4(Phong_Lighting_Pixel(input, normalDirection) * _TextureTint.xyz, 1.0f);
		}

		float4 frag_PerPixelLighting_Phong_NoTextureNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR
		{
 			vertexOutput_NoNormalMap_PerPixelLighting dummyOutput;

 			dummyOutput.posWorld = input.posWorld;
			dummyOutput.normalDir = input.normalDir;
			dummyOutput.normal = input.normal;
			dummyOutput.pos = input.pos;

			return float4 (Phong_Lighting_Pixel_NoNormalMap(dummyOutput)* _TextureTint.xyz, 1.0f);
		}

		float4 frag_PerPixelLighting_Lambert(vertexOutput_PerPixelLighting input) : COLOR
		{
			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input);

			return float4 (Texture_Handling_Pixel(input) * Lambert_Lighting_Pixel(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);		
		}

		float4 frag_PerPixelLighting_Lambert_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR
		{
			return float4 (Texture_Handling_Pixel_NoNormalMap(input) * Lambert_Lighting_Pixel_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);	
		}

		float4 frag_PerPixelLighting_Lambert_NoTextureMap(vertexOutput_PerPixelLighting input) : COLOR
		{
			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input);

			return float4(Lambert_Lighting_Pixel(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz * _TextureTint.xyz, 1.0);
		}

		float4 frag_PerPixelLighting_Lambert_NoTextureNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR
		{
			vertexOutput_NoNormalMap_PerPixelLighting dummyOutput;

 			dummyOutput.posWorld = input.posWorld;
			dummyOutput.normalDir = input.normalDir;
			dummyOutput.normal = input.normal;
			dummyOutput.pos = input.pos;

			return float4 (Lambert_Lighting_Pixel_NoNormalMap(dummyOutput).xyz * (_LambertTintColor * _LambertTintForce).xyz * _TextureTint.xyz, 1.0);
		}

		float4 frag_PerPixelLighting_HalfLambert(vertexOutput_PerPixelLighting input) : COLOR
		{
			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input);

			return float4 (Texture_Handling_Pixel(input) * HalfLambert_Lighting_Pixel(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);		
		}

		float4 frag_PerPixelLighting_HalfLambert_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR
		{
			return float4 (Texture_Handling_Pixel_NoNormalMap(input) * HalfLambert_Lighting_Pixel_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);
		}

		float4 frag_PerPixelLighting_HalfLambert_NoTextureMap(vertexOutput_PerPixelLighting input) : COLOR
		{
			float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input);

			return float4(HalfLambert_Lighting_Pixel(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz * _TextureTint.xyz, 1.0);
		}

		float4 frag_PerPixelLighting_HalfLambert_NoTextureMapNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR
		{
			vertexOutput_NoNormalMap_PerPixelLighting dummyOutput;

 			dummyOutput.posWorld = input.posWorld;
			dummyOutput.normalDir = input.normalDir;
			dummyOutput.normal = input.normal;
			dummyOutput.pos = input.pos;

			return float4 (HalfLambert_Lighting_Pixel_NoNormalMap(dummyOutput).xyz * (_LambertTintColor * _LambertTintForce).xyz * _TextureTint.xyz, 1.0);
		}

		float4 frag_PerPixelLighting_NoLight(vertexOutput_PerPixelLighting input) : COLOR
		{
			float4 TextureColor = Texture_Handling_Pixel(input);
			return float4(TextureColor.xyz, 1.0f);		
		}

		float4 frag_PerPixelLighting_NoLight_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR
		{
			float4 TextureColor = Texture_Handling_Pixel_NoNormalMap(input);
			return float4(TextureColor.xyz, 1.0f);		
		}

		float4 frag_PerPixelLighting_NoLight_NoTextureMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input)
		{
			return float4(_TextureTint.xyz, 1.0f);
		}
		
		//===========================================================================================================

		//==== FUNCTIONS ONLY USED FOR SHADER EDITING, THEY SHALL NOT BE USED IN THE FINAL SHADER ====================

		vertexOutput_AllVariables vert_AllPosibilities(vertexInput_AllVariables input)
		{
			vertexOutput_AllVariables output;
		
			if (_IsPixelLighting == 1.0f) // Pixel
			{				

				if (_IsNormalMapApplied == 0.0f) // No normal map
				{
					if (_IsTextureApplied == 0.0f) // No texture
					{
						vertexOutput_NoTextureNoNormalMap_PerPixelLighting outputDummy;
						vertexInput_NoTextureNoNormalMap inputDummy;
						inputDummy.vertex = input.vertex;	
						inputDummy.normal = input.normal;
						outputDummy = vert_PerPixelLighting_NoTextureNoNormalMap(inputDummy);
						output.posWorld = outputDummy.posWorld;
						output.pos = outputDummy.pos;
						output.normalDir = outputDummy.normalDir;
						output.normal = outputDummy.normal;
					}
					else
					{
						vertexOutput_NoNormalMap_PerPixelLighting outputDummy;
						vertexInput_NoNormalMap inputDummy;
						inputDummy.vertex = input.vertex;
						inputDummy.normal = input.normal;
						inputDummy.texcoord = input.texcoord;
						outputDummy = vert_PerPixelLighting_NoNormalMap(inputDummy);
						output.posWorld = outputDummy.posWorld;
						output.pos = outputDummy.pos;
						output.normalDir = outputDummy.normalDir;
						output.tex = outputDummy.tex;
						output.normal = outputDummy.normal;
					}
				}
				else
				{

					vertexOutput_PerPixelLighting outputDummy;
					outputDummy = vert_PerPixelLighting(input);
					output.posWorld = outputDummy.posWorld;
					output.pos = outputDummy.pos;
					output.normalDir = outputDummy.normalDir;
					output.tex = outputDummy.tex;
					output.tangent = outputDummy.tangent;
					output.normal = outputDummy.normal;
				
					//If i do not put this one, unity cries, so xd
					output.col = float4(1.0f, 1.0f, 1.0f, 1.0f);
				}

			}
			else // Vertex
			{
				vertexOutput_PerVertexLighting outputDummy;
				
				if (_LightingModel == 1.0f) // Phong
				{
					if(_IsNormalMapApplied == 0.0f) // no normal map
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexInput_NoTextureNoNormalMap inputDummy;
							inputDummy.vertex = input.vertex;
							inputDummy.normal = input.normal;
							vertexOutput_NoTextureNoNormalMap_PerVertexLighting outputTemp;
							outputTemp = vert_PerVertexLighting_Phong_NoNormalMap(inputDummy);
							output.col = outputTemp.col;
							output.pos = outputTemp.pos;
						}
						else
						{
							outputDummy = vert_PerVertexLighting_Phong(input);
							output.pos = outputDummy.pos;
							output.col = outputDummy.col;
							output.tex = outputDummy.tex;
						}						
					}
					else
					{
						outputDummy = vert_PerVertexLighting_Phong(input);
						output.pos = outputDummy.pos;
						output.col = outputDummy.col;
						output.tex = outputDummy.tex;
					}												
				}			
				if (_LightingModel == 2.0f) // Lambert
				{
					if(_IsNormalMapApplied == 0.0f) // no normal map
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexInput_NoTextureNoNormalMap inputDummy;
							inputDummy.vertex = input.vertex;
							inputDummy.normal = input.normal;
							vertexOutput_NoTextureNoNormalMap_PerVertexLighting outputTemp;
							outputTemp = vert_PerVertexLighting_Lambert_NoNormalMap(inputDummy);
							output.col = outputTemp.col;
							output.pos = outputTemp.pos;
						}
						else
						{
							outputDummy = vert_PerVertexLighting_Lambert(input);
							output.pos = outputDummy.pos;
							output.col = outputDummy.col;
							output.tex = outputDummy.tex;
						}						
					}
					else
					{
						outputDummy = vert_PerVertexLighting_Lambert(input);
						output.pos = outputDummy.pos;
						output.col = outputDummy.col;
						output.tex = outputDummy.tex;
					}									
				}
				if (_LightingModel == 3.0f) // HalfLambert
				{
					if(_IsNormalMapApplied == 0.0f) // no normal map
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexInput_NoTextureNoNormalMap inputDummy;
							inputDummy.vertex = input.vertex;
							inputDummy.normal = input.normal;
							vertexOutput_NoTextureNoNormalMap_PerVertexLighting outputTemp;
							outputTemp = vert_PerVertexLighting_HalfLambert_NoNormalMap(inputDummy);
							output.col = outputTemp.col;
							output.pos = outputTemp.pos;
						}
						else
						{
							outputDummy = vert_PerVertexLighting_HalfLambert(input);
							output.pos = outputDummy.pos;
							output.col = outputDummy.col;
							output.tex = outputDummy.tex;
						}						
					}
					else
					{
						outputDummy = vert_PerVertexLighting_HalfLambert(input);
						output.pos = outputDummy.pos;
						output.col = outputDummy.col;
						output.tex = outputDummy.tex;
					}
								
				}
				if (_LightingModel == 0.0f) // No_Light
				{
					if (_IsNormalMapApplied == 0.0f)
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexInput_NoLight_NoTextureNoNormalMap inputDummy;
							inputDummy.vertex = input.vertex;	
							vertexOutput_NoLight_NoTextureNoNormalMap outputTemp;
							outputTemp = vert_PerVertexLighting_NoLight_NoTextureNoNormalMap(inputDummy);
							output.col = outputTemp.col;
							output.pos = outputTemp.pos;
						}
						else
						{
							outputDummy = vert_PerVertexLighting_NoLight(input);	
							output.pos = outputDummy.pos;
							output.col = outputDummy.col;
							output.tex = outputDummy.tex;
						}
					}
					else
					{
						outputDummy = vert_PerVertexLighting_NoLight(input);	
						output.pos = outputDummy.pos;
						output.col = outputDummy.col;
						output.tex = outputDummy.tex;
					}											
				}			

				//If i do not put this one, unity cries, so i will just insert null values
				output.posWorld = float3(0.0f, 0.0f, 0.0f);
				output.normalDir = float3(0.0f, 0.0f, 0.0f);
				output.tangent = float4(0.0f, 0.0f, 0.0f, 0.0f);
				output.normal = float3(0.0f, 0.0f, 0.0f);
			}
			
			return output;
		}

		float4 frag_AllPosibilities(vertexOutput_AllVariables input) : COLOR
		{
			float4 finalColor;

			if (_IsPixelLighting == 0.0f) //Vertex Lighting
			{
				
					vertexOutput_PerVertexLighting inputDummy;
					inputDummy.tex = input.tex;
					inputDummy.col = input.col;
					inputDummy.pos = input.pos;
					finalColor = frag_PerVertexLighting(inputDummy);

				return finalColor;
			}
			else // Pixel Lighting
			{
				vertexOutput_PerPixelLighting inputDummy;
				inputDummy.posWorld = input.posWorld;
				inputDummy.pos = input.pos;
				inputDummy.normalDir = input.normalDir;
				inputDummy.tex = input.tex;
				inputDummy.tangent = input.tangent;
				inputDummy.normal = input.normal;

				if (_LightingModel == 1.0f) // Phong
				{
					if (_IsNormalMapApplied == 0.0f)
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexOutput_NoTextureNoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_Phong_NoTextureNoNormalMap(inputDummy);
						}
						else
						{
							vertexOutput_NoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_Phong_NoNormalMap(inputDummy);
						}
					}
					else
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexOutput_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.tangent = input.tangent;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_Phong_NoTexture(inputDummy);
						}
						else
						{
							vertexOutput_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.tangent = input.tangent;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_Phong(inputDummy);
						}
					}
				}	
				if (_LightingModel == 2.0f) // Lambert
				{
					if (_IsNormalMapApplied == 0.0f)
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexOutput_NoTextureNoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_Lambert_NoTextureNoNormalMap(inputDummy);
						}
						else
						{
							vertexOutput_NoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_Lambert_NoNormalMap(inputDummy);
						}
					}
					else
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexOutput_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.tangent = input.tangent;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_Lambert_NoTextureMap(inputDummy);
						}
						else
						{
							vertexOutput_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.tangent = input.tangent;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_Lambert(inputDummy);
						}
					}
				}
				if (_LightingModel == 3.0f) // HalfLambert
				{
					if (_IsNormalMapApplied == 0.0f)
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexOutput_NoTextureNoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_HalfLambert_NoTextureMapNoNormalMap(inputDummy);
						}
						else
						{
							vertexOutput_NoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_HalfLambert_NoNormalMap(inputDummy);
						}
					}
					else
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexOutput_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.tangent = input.tangent;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_HalfLambert_NoTextureMap(inputDummy);
						}
						else
						{
							vertexOutput_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.tangent = input.tangent;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_HalfLambert(inputDummy);
						}
					}
				}
				if (_LightingModel == 0.0f) // NoLight
				{
					if (_IsNormalMapApplied == 0.0f)
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexOutput_NoTextureNoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_NoLight_NoTextureMap(inputDummy);
						}
						else
						{
							vertexOutput_NoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_NoLight_NoNormalMap(inputDummy);
						}
					}
					else
					{
						if (_IsTextureApplied == 0.0f)
						{
							vertexOutput_NoTextureNoNormalMap_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_NoLight_NoTextureMap(inputDummy);
						}
						else
						{
							vertexOutput_PerPixelLighting inputDummy;
							inputDummy.posWorld = input.posWorld;
							inputDummy.pos = input.pos;
							inputDummy.normalDir = input.normalDir;
							inputDummy.tex = input.tex;
							inputDummy.tangent = input.tangent;
							inputDummy.normal = input.normal;
							finalColor = frag_PerPixelLighting_NoLight(inputDummy);
						}
					}
				}

				return finalColor;
			}

			return finalColor;		
		}


		

        