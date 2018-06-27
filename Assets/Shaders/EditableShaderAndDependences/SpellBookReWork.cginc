/*
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
uniform float3 vert_NormalMap_NormalDir = float3 (0, 0, 0);

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


struct vertexInput_NoNormalMap
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
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


float4 Texture_Handling_Vertex(vertexOutput_PerVertexLighting input)
{
	float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY);
	texCoordsScale *= input.tex.xy;
	texCoordsScale += float2(_OffsetTileX, _OffsetTileY);
	float4 textureColor = tex2Dlod(_CustomTexture, float4(texCoordsScale.xy, 0, 0));
	textureColor = textureColor * _TextureTint;
	return textureColor;
}
*/


