// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewShader"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		//_CustomTexture ("Custom Texture", 2D) = "white"{}
		//_TextureTint("Custom Texture Tint", Color) = (1.0, 1.0, 1.0, 1.0)
		//_NormalMap("Normal Map", 2D) = "bump"{}
		//_BumpMap("Bump Map", 2D) = "bump"{}
		_NormalMapScale("Normal Map Scale", float) = 1.0
		_MaxHeightBumpMap("Bump Map Max Height", float) = 0.5
		_MaxTexCoordOffset ("Bump Map Max Texture Coordinate offset", float) = 0.5
		_CustomAmbientLightForce("Ambient Light Force", float) = 0.75
		_CustomSpecularColor ("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_CustomShininess("Shininess", Range(0.0, 1.0)) = 0.5
		_PhongDiffuseColor("Phong Diffuse Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_PhongSpecularColor("Phong Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_PhongSpecularGlossiness("Phong Specular Glossiness", Range(0.0, 1.0)) = 0.5
		_PhongSpecularPower("Phong Specular Power", float) = 1.0
		//_TextureTileX("Texture Tiling X", float) = 1.0
		//_TextureTileY("Texture Tiling Y", float) = 1.0
		//_OffsetTileX("Offset Tiling X", float) = 0.0
		//_OffsetTileY("Offset Tiling Y", float) = 0.0
		//_LightingModel("Lighting Model", int) = 0
	}
	SubShader
	{
		Tags { "LightMode" = "ForwardBase" "RenderType" = "Opaque" }
		LOD 100

		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "SpellBook.cginc"
			
 
         ENDCG
		}
	}
}
