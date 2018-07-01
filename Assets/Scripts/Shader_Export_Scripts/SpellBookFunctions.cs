using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookFunctions : MonoBehaviour
{

    //This is a library of the functions that are available in SpellBook.
    // Ultima fecha de actualización: 05/05/2018

    public static string necessaryIncludes =
     " #include " + '"' + "UnityCG.cginc" + '"' + "   " + '\n' +
     " #include " + '"' + "UnityLightingCommon.cginc" + '"' + '\n' + "    ";


    public static string Texture_Handling_Variables =
        " uniform sampler2D _CustomTexture; " + '\n' +
        " uniform fixed4 _TextureTint; " + '\n' +
        " uniform float _TextureTileX; " + '\n' +
        " uniform float _TextureTileY; " + '\n' +
        " uniform float _OffsetTileX; " + '\n' +
        " uniform float _OffsetTileY; " + '\n'
        ;

    public static string Normal_Handling_Variables =
        " uniform sampler2D _NormalMap; " + '\n' +
        " uniform float _NormalTileX; " + '\n' +
        " uniform float _NormalTileY; " + '\n' +
        " uniform float _NormalOffsetX; " + '\n' +
        " uniform float _NormalOffsetY; " + '\n' +
        " uniform half _NormalMapScale = 1.0f; " + '\n'
        ;

    public static string Phong_Variables =

        " uniform float _CustomShininess; " + '\n' +
        " uniform float4 _PhongAmbientColor; " + '\n' +
        " uniform float _PhongAmbientForce; " + '\n' +
        " uniform float4 _PhongSpecularColor; " + '\n' +
        " uniform float _PhongSpecularForce; " + '\n' +
        " uniform float4 _PhongDiffuseColor; " + '\n' +
        " uniform float _PhongDiffuseForce; " + '\n'
        ;

    public static string Lambert_Variables =

        " uniform float _LambertTintForce; " + '\n' +
        " uniform float4 _LambertTintColor; " + '\n';

    public static string NoTextureMap_Variables =
        " uniform fixed4 _TextureTint; " + '\n';

    //============STRUCTS ===================================================

    public static string vertexInput_AllVariables =
        "struct vertexInput_AllVariables " +
        " { " +
        " float4 vertex : POSITION; " + '\n' +
        " float3 normal : NORMAL; " + '\n' +
        " float2 texcoord : TEXCOORD0; " + '\n' +
        " float4 tangent : TANGENT; " + " };" + '\n'
        ;

    public static string vertexInput_NoTextureNoNormalMap =
        " struct vertexInput_NoTextureNoNormalMap " +
        " { " +
            " float4 vertex : POSITION; " + '\n' +
            " float3 normal : NORMAL; " + '\n' +
        " }; " + '\n';

    public static string vertexInput_NoNormalMap =
        " struct vertexInput_NoNormalMap " + '\n' +
        " { " +
            " float4 vertex : POSITION; " + '\n' +
            " float3 normal : NORMAL; " + '\n' +
            " float2 texcoord : TEXCOORD0; " + '\n' +
        " }; " + '\n';

    public static string vertexInput_NoLight =
        " struct vertexInput_NoLight " + '\n' +
        " { " +
            " float4 vertex : POSITION; " + '\n' +
            " float3 normal : NORMAL; " + '\n' +
            " float2 texcoord : TEXCOORD0; " + '\n' +
            " float4 tangent : TANGENT; " + '\n' +
        " }; " + '\n';

    public static string vertexInput_NoLight_NoTextureNoNormalMap =
       "  struct vertexInput_NoLight_NoTextureNoNormalMap " + '\n' +
        " { " +
            " float4 vertex : POSITION; " + '\n' +
        " }; " + '\n';

    public static string vertexOutput_NoTextureNoNormalMap_PerVertexLighting =
        " struct vertexOutput_NoTextureNoNormalMap_PerVertexLighting " + '\n' +
        " { " +
            " float4 pos : SV_POSITION; " + '\n' +
            " float4 col : COLOR; " + '\n' +
        " }; " + '\n';

    public static string vertexOutput_NoNormalMap_PerPixelLighting =
        " struct vertexOutput_NoNormalMap_PerPixelLighting " + '\n' +
        " { " +
            " float4 pos : SV_POSITION; " + '\n' +
            " float3 posWorld : TEXCOORD0; " + '\n' +
            " float3 normalDir : TEXCOORD1; " + '\n' +
            " float3 normal : NORMAL; " + '\n' +
            " float2 tex : TEXCOORD2; " + '\n' +
        " }; " + '\n';

    public static string vertexOutput_NoTextureNoNormalMap_PerPixelLighting =
        " struct vertexOutput_NoTextureNoNormalMap_PerPixelLighting " +
        " { " +
            " float4 pos : SV_POSITION; " +
            " float3 posWorld : TEXCOORD0; " +
            " float3 normalDir : TEXCOORD1; " +
            " float3 normal : NORMAL; " +
        " }; " + '\n';

    public static string vertexOutput_NoLight_NoTextureNoNormalMap =
        " struct vertexOutput_NoLight_NoTextureNoNormalMap " + '\n' +
        " { " +
            " float4 pos : SV_POSITION; " + '\n' +
            " float4 col : COLOR; " + '\n' +
        " }; " + '\n';

    public static string vertexOutput_PerVertexLighting =
        "struct vertexOutput_PerVertexLighting " +
        " { " +
        " float4 pos : SV_POSITION; " +
        " float4 col : COLOR; " +
        " float2 tex : TEXCOORD1; " + "};" + '\n'
        ;

    public static string vertexOutput_PerPixelLighting =
        " struct vertexOutput_PerPixelLighting " + '\n' +
        " { " +
        " float4 pos : SV_POSITION; " + '\n' +
        " float3 posWorld : TEXCOORD0; " + '\n' +
        " float3 normalDir : TEXCOORD1; " + '\n' +
        " float2 tex : TEXCOORD2; " + '\n' +
        " float4 tangent : TANGENT; " + '\n' +
        " float3 normal : NORMAL; " + "};" + '\n'
        ;

    //===============================================================================================
    //===============================================================================================

    public static string Texture_Handling_Pixel =
        " float4 Texture_Handling_Pixel(vertexOutput_PerPixelLighting input) " + '\n' +
        " { " +
        " float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY); " + '\n' +
        " texCoordsScale *= input.tex.xy; " + '\n' +
        " texCoordsScale += float2(_OffsetTileX, _OffsetTileY); " + '\n' +
        " float4 textureColor = tex2Dlod(_CustomTexture, float4(texCoordsScale, 0, 0)); " + '\n' +
        " textureColor = textureColor * _TextureTint; " + '\n' +
        " return textureColor; " + "}" + '\n'
        ;

    public static string Texture_Handling_Vertex =
        " float4 Texture_Handling_Vertex(vertexOutput_PerVertexLighting input) " + '\n' +
         " { " +
        " float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY); " + '\n' +
        " texCoordsScale *= input.tex.xy; " + '\n' +
        " float4 textureColor = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY)); " + '\n' +
        " textureColor = textureColor * _TextureTint; " + '\n' +
        " return textureColor; " + "}" + '\n'
        ;

    public static string Texture_Handling_Pixel_NoNormalMap =
        " float4 Texture_Handling_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) " + '\n' +
        " { " +
            " float2 texCoordsScale = float2(_TextureTileX, _TextureTileY); " + '\n' +
            " texCoordsScale *= input.tex.xy; " + '\n' +
            " float4 textureColor = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY)); " + '\n' +
            " textureColor = textureColor * _TextureTint; " + '\n' +
            " return textureColor; " +
        " } " + '\n';

    public static string Normal_Direction_With_Normal_Map_Handling_Vertex =
       " float3 Normal_Direction_With_Normal_Map_Handling_Vertex(vertexInput_AllVariables input) " + '\n' +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " float3 tangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz); " + '\n' +
        " float3 normalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
        " float3 BitangentWorld = normalize(cross(normalWorld, tangentWorld) * input.tangent.w); " + '\n' +
        " float3 biNormal = cross(input.normal, input.tangent.xyz) * input.tangent.w; " + '\n' +
        " float2 normalCoordsScaled = float2(_NormalTileX, _NormalTileY); " + '\n' +
        " normalCoordsScaled *= input.texcoord.xy; " + '\n' +
        " normalCoordsScaled += float2(_NormalOffsetX, _NormalOffsetY); " + '\n' +
        " float4 encodedNormal = tex2Dlod(_NormalMap, float4(normalCoordsScaled.xy,0, 0)); " + '\n' +
        " float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0, 1.0), 0.0); " + '\n' +
        " localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords); " + '\n' +
        " float3x3 local2WorldTranspose = float3x3(tangentWorld, BitangentWorld, normalWorld); " + '\n' +
        " float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); " + '\n' +
        " normalDirection = float3(_NormalMapScale, _NormalMapScale, 1.0f) * normalDirection; " + '\n' +
        " return normalDirection; " + "}" + '\n'
        ;

    public static string Normal_Direction_With_Normal_Map_Handling_Pixel =
        " float3 Normal_Direction_With_Normal_Map_Handling_Pixel(vertexOutput_PerPixelLighting input) " + '\n' +
          " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " float3 tangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz); " + '\n' +
        " float3 normalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
        " float3 BitangentWorld = normalize(cross(normalWorld, tangentWorld) * input.tangent.w); " + '\n' +
        " float3 biNormal = cross(input.normal, input.tangent.xyz) * input.tangent.w; " + '\n' +
        " float2 normalCoordsScaled = float2(_NormalTileX, _NormalTileY); " + '\n' +
        " normalCoordsScaled *= input.tex.xy; " + '\n' +
        " float4 encodedNormal = tex2D(_NormalMap, (normalCoordsScaled + float2(_NormalOffsetX, _NormalOffsetY)).xy); " + '\n' +
        " float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0, 1.0), 0.0); " + '\n' +
        " localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords); " + '\n' +
        " float3x3 local2WorldTranspose = float3x3(tangentWorld, BitangentWorld, normalWorld); " + '\n' +
        " float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); " + '\n' +
        " normalDirection = float3(_NormalMapScale, _NormalMapScale, 1.0f) * normalDirection; " + '\n' +
        " return normalDirection; " + "}" + '\n'
        ;


    public static string Phong_Lighting_Vertex =
        " float3 Phong_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " + '\n' +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float3x3 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.vertex).xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
            " attenuation = 1.0; " + '\n' +
            " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " + '\n' +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(modelMatrix, input.vertex).xyz; " + '\n' +
            " float3 distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
        " } " +

        " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb; " + '\n' +
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float3 specularReflection; " + '\n' +
        " if (dot(normalDirection, lightDirection) < 0.0) " + '\n' +
        " { specularReflection = float3 (0.00001, 0.00001, 0.00001); } " + '\n' +
        " else " + '\n' +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)); " + '\n' +
            " specularReflection = specularReflection * _CustomShininess; " + '\n' +
        " } " +
        "return (ambientLighting * _PhongAmbientForce) + (diffuseReflection * _PhongDiffuseForce) + (specularReflection * _PhongSpecularForce); } " + '\n' 
        ;

    public static string Phong_Lighting_Vertex_NoNormalMap =
        " float3 Phong_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " + '\n' +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float3x3 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " float3 normalDirection = normalize(mul(input.normal, modelMatrixInverse)); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.vertex).xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
            " attenuation = 1.0; " + '\n' +
            " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(modelMatrix, input.vertex).xyz; " + '\n' +
            " float3 distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
        " } " +

        " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb; " + '\n' +
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float3 specularReflection; " + '\n' +
        " if (dot(normalDirection, lightDirection) < 0.0) " + '\n' +
        " { specularReflection = float3(0.00001, 0.00001, 0.00001); } " + '\n' +
        " else " + '\n' +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)); " + '\n' +
            " specularReflection = specularReflection * _CustomShininess; " + '\n' +
        " } " +
        "return (ambientLighting * _PhongAmbientForce) + (diffuseReflection * _PhongDiffuseForce) + (specularReflection * _PhongSpecularForce); } " + '\n'
        ;

    public static string Lambert_Lighting_Vertex =
       " float4 Lambert_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " float3 posWorld = mul(modelMatrix, input.vertex); " + '\n' +
        " float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
        " normalDirection += normalize(normalDir); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
           " attenuation = 1.0f; " + '\n' +
            " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz; " + '\n' +
            " float distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
         " } " +
        " float NDotL = max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float LambertDiffuse = NDotL; " + '\n' +
        " float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb; " + '\n' +
        " return float4 (finalColor, 1.0f); " + " } " + '\n'
        ;

    public static string Lambert_Lighting_Vertex_NoNormalMap =
       " float4 Lambert_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " + '\n' +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " float3 posWorld = mul(modelMatrix, input.vertex); " + '\n' +
        " float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
        " float3 normalDirection = normalize(mul(input.normal, modelMatrixInverse)); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
           " attenuation = 1.0f; " + '\n' +
            " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz; " + '\n' +
            " float distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
         " } " +
        " float NDotL = max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float LambertDiffuse = NDotL; " + '\n' +
        " float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb; " + '\n' +
        " return float4 (finalColor, 1.0f); " + " } " + '\n'
        ;

    public static string HalfLambert_Lighting_Vertex =
        " float4 HalfLambert_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " + '\n' +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " float3 posWorld = mul(modelMatrix, input.vertex); " + '\n' +
        " float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
        " normalDirection += normalize(normalDir); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
          " attenuation = 1.0f; " + '\n' +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz; " + '\n' +
            " float distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
        " } " +
        " float3 NDotL = max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0); " + '\n' +
        " float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb; " + '\n' +
        " return float4 (finalColor, 1.0f); " + " } " + '\n'
        ;

    public static string HalfLambert_Lighting_Vertex_NoNormalMap =
        " float4 HalfLambert_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " + '\n' +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " float3 posWorld = mul(modelMatrix, input.vertex); " + '\n' +
        " float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
        " float3 normalDirection = normalize(mul(input.normal, modelMatrixInverse)); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
          " attenuation = 1.0f; " + '\n' +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz; " + '\n' +
            " float distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
        " } " +
        " float3 NDotL = max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0); " + '\n' +
        " float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb; " + '\n' +
        " return float4 (finalColor, 1.0f); " + " } " + '\n'
        ;

    public static string Phong_Lighting_Pixel =
        " float3 Phong_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " + '\n' +
        " { " +
        " normalDirection += normalize(input.normalDir); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
          " attenuation = 1.0f; " + '\n' +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " + '\n' +
            " float distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
        " } " +
        " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb; " + '\n' +
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float3 specularReflection; " + '\n' +
        " if (dot(normalDirection, lightDirection) < 0.0) " + '\n' +
        " { specularReflection = float3(0.00001, 0.00001, 0.00001); } " + '\n' +
        " else " + '\n' +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)); " + '\n' +
            " specularReflection = specularReflection * _CustomShininess; " + '\n' +
        " } " +
        " return (ambientLighting * _PhongAmbientForce) + (diffuseReflection * _PhongDiffuseForce) + (specularReflection * _PhongSpecularForce); }" + '\n'
        ;

    public static string Phong_Lighting_Pixel_NoNormalMap =
       " float3 Phong_Lighting_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) " + '\n' +
       " { " +
       " float3 normalDirection = normalize(input.normalDir); " + '\n' +
       " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " + '\n' +
       " float3 lightDirection; " + '\n' +
       " float attenuation; " + '\n' +
       " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
       " { " +
         " attenuation = 1.0f; " + '\n' +
         " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
       " } " +
       " else " + '\n' +
       " { " +
           " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " + '\n' +
           " float distance = length(vertexToLightSource); " + '\n' +
           " attenuation = 1.0 / distance; " + '\n' +
           " lightDirection = normalize(vertexToLightSource); " + '\n' +
       " } " +
       " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb; " + '\n' +
       " float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * max(0.0, dot(normalDirection, lightDirection)); " +
       " float3 specularReflection; " + '\n' +
       " if (dot(normalDirection, lightDirection) < 0.0) " + '\n' +
       " { specularReflection = float3(0.00001, 0.00001, 0.00001); } " + '\n' +
       " else " + '\n' +
       " { " +
           " specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)); " + '\n' +
            " specularReflection = specularReflection * _CustomShininess; " + '\n' + 
       " } " + '\n' +
       " return (ambientLighting * _PhongAmbientForce) + (diffuseReflection * _PhongDiffuseForce) + (specularReflection * _PhongSpecularForce); } " + '\n'
        ;

    public static string Lambert_Lighting_Pixel =
        " float3 Lambert_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " + '\n' +
        " { " +
        " normalDirection += normalize(input.normalDir); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
          " attenuation = 1.0f; " + '\n' +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " + '\n' +
            " float distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
        " } " +
        " float NDotL = max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float LambertDiffuse = NDotL; " + '\n' +
        " float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb; " + '\n' +
        " return finalColor; " + " } " + '\n'
        ;

    public static string Lambert_Lighting_Pixel_NoNormalMap =
        " float3 Lambert_Lighting_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) " + '\n' +
        " { " +
        " float3 normalDirection = normalize(input.normalDir); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
          " attenuation = 1.0f; " + '\n' +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " + '\n' +
            " float distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
        " } " +
        " float NDotL = max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float LambertDiffuse = NDotL; " + '\n' +
        " float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb; " + '\n' +
        " return finalColor; " + " } " + '\n'
        ;

    public static string HalfLambert_Lighting_Pixel =
        " float3 HalfLambert_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " + '\n' +
        " { " +
        " normalDirection += normalize(input.normalDir); " + '\n' +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " + '\n' +
        " float3 lightDirection; " + '\n' +
        " float attenuation; " + '\n' +
        " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
        " { " +
          " attenuation = 1.0f; " + '\n' +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
        " } " +
        " else " + '\n' +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " + '\n' +
            " float distance = length(vertexToLightSource); " + '\n' +
            " attenuation = 1.0 / distance; " + '\n' +
            " lightDirection = normalize(vertexToLightSource); " + '\n' +
        " } " +
        " float3 NDotL = max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
        " float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0); " + '\n' +
        " float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb; " + '\n' +
        " return finalColor; " + " } " + '\n'
        ;

    public static string HalfLambert_Lighting_Pixel_NoNormalMap =
       " float3 HalfLambert_Lighting_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) " + '\n' +
       " { " +
       " float3 normalDirection = normalize(input.normalDir); " + '\n' +
       " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " + '\n' +
       " float3 lightDirection; " + '\n' +
       " float attenuation; " + '\n' +
       " if (0.0 == _WorldSpaceLightPos0.w) " + '\n' +
       " { " +
         " attenuation = 1.0f; " + '\n' +
         " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " + '\n' +
       " } " +
       " else " + '\n' +
       " { " +
           " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " + '\n' +
           " float distance = length(vertexToLightSource); " + '\n' +
           " attenuation = 1.0 / distance; " + '\n' +
           " lightDirection = normalize(vertexToLightSource); " + '\n' +
       " } " +
       " float3 NDotL = max(0.0, dot(normalDirection, lightDirection)); " + '\n' +
       " float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0); " + '\n' +
       " float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb; " + '\n' +
       " return finalColor; " + " } " + '\n'
       ;

    public static string vert_PerVertexLighting_Phong =
       " vertexOutput_PerVertexLighting vert_PerVertexLighting_Phong(vertexInput_AllVariables input) " + '\n' +
        " { " +
        " vertexOutput_PerVertexLighting output; " + '\n' +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " + '\n' +
        " output.col = float4(Phong_Lighting_Vertex(input, normalDirection), 1.0f); " + '\n' +
        " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
        " output.tex = input.texcoord; " + '\n' +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_Phong_NoNormalMap =
       " vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_Phong_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " + '\n' +
        " { " +
        " vertexOutput_NoTextureNoNormalMap_PerVertexLighting output; " + '\n' +
        " output.col = float4(Phong_Lighting_Vertex_NoNormalMap(input), 1.0f);" + '\n' +
        " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_Lambert =
      " vertexOutput_PerVertexLighting vert_PerVertexLighting_Lambert(vertexInput_AllVariables input) " + '\n' +
        " { " +
        " vertexOutput_PerVertexLighting output; " + '\n' +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " + '\n' +
        " output.col = float4(Lambert_Lighting_Vertex(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0); " + '\n' +
        " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
        " output.tex = input.texcoord; " + '\n' +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_Lambert_NoNormalMap =
     " vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_Lambert_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " + '\n' +
       " { " +
       " vertexOutput_NoTextureNoNormalMap_PerVertexLighting output; " + '\n' +
       " output.col = output.col = float4(Lambert_Lighting_Vertex_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);" + '\n' +
       " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
       " return output; " + " } " + '\n'
       ;

    public static string vert_PerVertexLighting_HalfLambert =
        " vertexOutput_PerVertexLighting vert_PerVertexLighting_HalfLambert(vertexInput_AllVariables input) " + '\n' +
        " { " +
        " vertexOutput_PerVertexLighting output; " + '\n' +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " + '\n' +
        " output.col = float4(HalfLambert_Lighting_Vertex(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0); " + '\n' +
        " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
        " output.tex = input.texcoord; " + '\n' +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_HalfLambert_NoNormalMap =
        " vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_HalfLambert_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " + '\n' +
        " { " +
        " vertexOutput_NoTextureNoNormalMap_PerVertexLighting output; " + '\n' +
        " output.col = float4(HalfLambert_Lighting_Vertex_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);" + '\n' +
        " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_NoLight =
        " vertexOutput_PerVertexLighting vert_PerVertexLighting_NoLight(vertexInput_AllVariables input) " + '\n' +
        " { " +
        " vertexOutput_PerVertexLighting output; " + '\n' +
        " output.col = float4 (1.0f, 1.0f, 1.0f, 1.0f); " + '\n' +
        " output.tex = input.texcoord; " + '\n' +
        " return output; " + " } " + '\n';

    public static string vert_PerVertexLighting_NoLight_NoTextureNoNormalMap =
        " vertexOutput_NoLight_NoTextureNoNormalMap vert_PerVertexLighting_NoLight_NoTextureNoNormalMap (vertexInput_NoLight_NoTextureNoNormalMap input) " +
        " { " +
        " vertexOutput_NoLight_NoTextureNoNormalMap output; " + '\n' +
        " output.pos = UnityObjectToClipPos(input.vertex);" + '\n' +
        " output.col = float4 (1.0f, 1.0f, 1.0f, 1.0f); " + '\n' +
        " return output; " + " } " + '\n';

    public static string frag_PerVertexLighting =
        " float4 frag_PerVertexLighting(vertexOutput_PerVertexLighting input) : COLOR " +
        " {	" +
        " float4 TextureColor = Texture_Handling_Vertex(input); " + '\n' +
        " return float4(input.col.xyz * TextureColor.xyz , 1.0f);" + '\n' +
        " } " + '\n'
        ;

    public static string frag_PerVertexLighting_NoTextureMap =
       " float4 frag_PerVertexLighting_NoTextureMap (vertexOutput_NoTextureNoNormalMap_PerVertexLighting input): COLOR " + '\n' +
       " {	" +
       " return float4(input.col.xyz * _TextureTint.xyz, 1.0f); " +
       " } " + '\n'
       ;

    public static string vert_PerPixelLighting =
        " vertexOutput_PerPixelLighting vert_PerPixelLighting(vertexInput_AllVariables input) " + '\n' +
        " { " +
        " vertexOutput_PerPixelLighting output; " + '\n' +
        " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
        " output.posWorld = mul(modelMatrix, input.vertex); " + '\n' +
        " output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
        " output.tex = input.texcoord; " + '\n' +
        " output.tangent = input.tangent; " + '\n' +
        " output.normal = input.normal; " + '\n' +
        " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerPixelLighting_NoTextureNoNormalMap =
       " vertexOutput_NoTextureNoNormalMap_PerPixelLighting vert_PerPixelLighting_NoTextureNoNormalMap(vertexInput_NoTextureNoNormalMap input)" + '\n' +
       " { " +
       " vertexOutput_NoTextureNoNormalMap_PerPixelLighting output; " + '\n' +
       " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
       " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
       " output.posWorld = mul(modelMatrix, input.vertex); " + '\n' +
       " output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
       " output.normal = input.normal; " + '\n' +
       " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
       " return output; " + " } " + '\n'
       ;

    public static string vert_PerPixelLighting_NoNormalMap =
       " vertexOutput_NoNormalMap_PerPixelLighting vert_PerPixelLighting_NoNormalMap (vertexInput_NoNormalMap input) " + '\n' +
       " { " +
       " vertexOutput_NoNormalMap_PerPixelLighting output; " + '\n' +
       " float4x4 modelMatrix = unity_ObjectToWorld; " + '\n' +
       " float4x4 modelMatrixInverse = unity_WorldToObject; " + '\n' +
       " output.posWorld = mul(modelMatrix, input.vertex); " + '\n' +
       " output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " + '\n' +
       " output.tex = input.texcoord; " + '\n' +
       " output.normal = input.normal; " + '\n' +
       " output.pos = UnityObjectToClipPos(input.vertex); " + '\n' +
       " return output; " + " } " + '\n'
       ;

    public static string frag_PerPixelLighting_Phong =
        " float4 frag_PerPixelLighting_Phong(vertexOutput_PerPixelLighting input) : COLOR " + '\n' +
        " {	 " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " + '\n' +
            " return float4(Texture_Handling_Pixel(input).xyz * Phong_Lighting_Pixel(input, normalDirection), 1.0f); " + '\n' +
        " } " + '\n'
        ;

    public static string frag_PerPixelLighting_Phong_NoNormalMap =
        " float4 frag_PerPixelLighting_Phong_NoNormalMap (vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR " + '\n' +
        " {	 " +
            " return float4(Texture_Handling_Pixel_NoNormalMap(input).xyz * Phong_Lighting_Pixel_NoNormalMap(input), 1.0f); " + '\n' +
        " } " + '\n'
        ;
    public static string frag_PerPixelLighting_Phong_NoTexture =
        " float4 frag_PerPixelLighting_Phong_NoTexture(vertexOutput_PerPixelLighting input) : COLOR " + '\n' +
        " {	 " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " + '\n' +
            " return float4(Phong_Lighting_Pixel(input, normalDirection) * _TextureTint.xyz, 1.0f); " + '\n' +
        " } " + '\n'
        ;

    public static string frag_PerPixelLighting_Phong_NoTextureNoNormalMap =
        " float4 frag_PerPixelLighting_Phong_NoTextureNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " vertexOutput_NoNormalMap_PerPixelLighting dummyOutput; " + '\n' +
            " dummyOutput.posWorld = input.posWorld; " + '\n' +
            " dummyOutput.normalDir = input.normalDir; " + '\n' +
            " dummyOutput.normal = input.normal; " + '\n' +
            " dummyOutput.pos = input.pos; " + '\n' +
            " return float4 (Phong_Lighting_Pixel_NoNormalMap(dummyOutput)* _TextureTint.xyz, 1.0f); " + '\n' +
        " } " + '\n';


    public static string frag_PerPixelLighting_Lambert =
        " float4 frag_PerPixelLighting_Lambert(vertexOutput_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " + '\n' +
            " return float4 (Texture_Handling_Pixel(input) * Lambert_Lighting_Pixel(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0); " + '\n' +
        " } " + '\n'
        ;

    public static string frag_PerPixelLighting_Lambert_NoNormalMap =
        " float4 frag_PerPixelLighting_Lambert_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " return float4 (Texture_Handling_Pixel_NoNormalMap(input) * Lambert_Lighting_Pixel_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0); " + '\n' +
         " } " + '\n';

    public static string frag_PerPixelLighting_Lambert_NoTextureMap =
        " float4 frag_PerPixelLighting_Lambert_NoTextureMap(vertexOutput_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " + '\n' +
            " return float4(Lambert_Lighting_Pixel(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz * _TextureTint.xyz, 1.0); " + '\n' +
        " } " + '\n';

    public static string frag_PerPixelLighting_Lambert_NoTextureNoNormalMap =
        " float4 frag_PerPixelLighting_Lambert_NoTextureNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " vertexOutput_NoNormalMap_PerPixelLighting dummyOutput; " + '\n' +
            " dummyOutput.posWorld = input.posWorld; " + '\n' +
            " dummyOutput.normalDir = input.normalDir; " + '\n' +
            " dummyOutput.normal = input.normal; " + '\n' +
            " dummyOutput.pos = input.pos; " + '\n' +
            " return float4 (Lambert_Lighting_Pixel_NoNormalMap(dummyOutput).xyz * (_LambertTintColor * _LambertTintForce).xyz * _TextureTint.xyz, 1.0); " + '\n' +
        " } " + '\n';


    public static string frag_PerPixelLighting_HalfLambert =
        " float4 frag_PerPixelLighting_HalfLambert(vertexOutput_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " + '\n' +
            " return float4 (Texture_Handling_Pixel(input) * HalfLambert_Lighting_Pixel(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0); " + '\n' +
        " } " + '\n'
        ;

    public static string frag_PerPixelLighting_HalfLambert_NoNormalMap =
        " float4 frag_PerPixelLighting_HalfLambert_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " return float4 (Texture_Handling_Pixel_NoNormalMap(input) * HalfLambert_Lighting_Pixel_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0); " + '\n' +
        " } " + '\n';

    public static string frag_PerPixelLighting_HalfLambert_NoTextureMap =
        " float4 frag_PerPixelLighting_HalfLambert_NoTextureMap(vertexOutput_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " + '\n' +
            " return float4(HalfLambert_Lighting_Pixel(input, normalDirection).xyz * (_LambertTintColor * _LambertTintForce).xyz * _TextureTint.xyz, 1.0); " + '\n' +
         " } " + '\n';

    public static string frag_PerPixelLighting_HalfLambert_NoTextureMapNoNormalMap =
        " float4 frag_PerPixelLighting_HalfLambert_NoTextureMapNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " vertexOutput_NoNormalMap_PerPixelLighting dummyOutput; " + '\n' +
            " dummyOutput.posWorld = input.posWorld; " + '\n' +
            " dummyOutput.normalDir = input.normalDir; " + '\n' +
            " dummyOutput.normal = input.normal; " + '\n' +
            " dummyOutput.pos = input.pos; " + '\n' +
            " return float4 (HalfLambert_Lighting_Pixel_NoNormalMap(dummyOutput).xyz * (_LambertTintColor * _LambertTintForce).xyz * _TextureTint.xyz, 1.0); " + '\n' +
         " } " + '\n';

    public static string frag_PerPixelLighting_NoLight =
        " float4 frag_PerPixelLighting_NoLight(vertexOutput_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
            " float4 TextureColor = Texture_Handling_Pixel(input); " + '\n' +
            "return float4(TextureColor.xyz, 1.0f); " + " } " + '\n'
        ;

    public static string frag_PerPixelLighting_NoLight_NoNormalMap =
       " float4 frag_PerPixelLighting_NoLight_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR" + '\n' +
       " { " +
           " float4 TextureColor = Texture_Handling_Pixel_NoNormalMap(input); " + '\n' +
           " return float4(TextureColor.xyz, 1.0f); " + " } " + '\n'
       ;


    public static string frag_PerPixelLighting_NoLight_NoTextureMap =
        "  float4 frag_PerPixelLighting_NoLight_NoTextureMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR " + '\n' +
        " { " +
        " return float4(_TextureTint.xyz, 1.0f); " + '\n' +
        " } " + '\n';


}
