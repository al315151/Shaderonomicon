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
        " uniform sampler2D _CustomTexture; " +
        " uniform fixed4 _TextureTint; " +
        " uniform float _TextureTileX; " +
        " uniform float _TextureTileY; " +
        " uniform float _OffsetTileX; " +
        " uniform float _OffsetTileY; " + '\n'
        ;

    public static string Normal_Handling_Variables =
        " uniform sampler2D _NormalMap; " +
        " uniform float _NormalTileX; " +
        " uniform float _NormalTileY; " +
        " uniform float _NormalOffsetX; " +
        " uniform float _NormalOffsetY; " +
        " uniform half _NormalMapScale = 1.0f; " + '\n'
        ;

    public static string Phong_Variables =

        " uniform float _CustomShininess; " +
        " uniform float4 _PhongAmbientColor; " +
        " uniform float _PhongAmbientForce; " +
        " uniform float4 _PhongSpecularColor; " +
        " uniform float _PhongSpecularForce; " +
        " uniform float4 _PhongDiffuseColor; " +
        " uniform float _PhongDiffuseForce; " + '\n'
        ;

    public static string Lambert_Variables =

        " uniform float _LambertTintForce; " +
        " uniform float4 _LambertTintColor; " + '\n';

    public static string NoTextureMap_Variables =
        " uniform fixed4 _TextureTint; " + '\n';

    //============FUNCTIONS ===================================================

    public static string vertexInput_AllVariables =
        "struct vertexInput_AllVariables " +
        " { " +
        " float4 vertex : POSITION; " +
        " float3 normal : NORMAL; " +
        " float2 texcoord : TEXCOORD0; " +
        " float4 tangent : TANGENT; " + " };" + '\n'
        ;

    public static string vertexInput_NoTextureNoNormalMap =
        " struct vertexInput_NoTextureNoNormalMap " +
        " { " +
            " float4 vertex : POSITION; " +
            " float3 normal : NORMAL; " +
        " }; " + '\n';

    public static string vertexInput_NoNormalMap =
        " struct vertexInput_NoNormalMap " +
        " { " +
            " float4 vertex : POSITION; " +
            " float3 normal : NORMAL; " +
            " float2 texcoord : TEXCOORD0; " +
        " }; " + '\n';

    public static string vertexInput_NoLight =
        " struct vertexInput_NoLight " +
        " { " +
            " float4 vertex : POSITION; " +
            " float3 normal : NORMAL; " +
            " float2 texcoord : TEXCOORD0; " +
            " float4 tangent : TANGENT; " +
        " }; " + '\n';

    public static string vertexInput_NoLight_NoTextureNoNormalMap =
       "  struct vertexInput_NoLight_NoTextureNoNormalMap " +
        " { " +
            " float4 vertex : POSITION; " +
        " }; " + '\n';

    public static string vertexOutput_NoTextureNoNormalMap_PerVertexLighting =
        " struct vertexOutput_NoTextureNoNormalMap_PerVertexLighting " +
        " { " +
            " float4 pos : SV_POSITION; " +
            " float4 col : COLOR; " +
        " }; " + '\n';

    public static string vertexOutput_NoNormalMap_PerPixelLighting =
        " struct vertexOutput_NoNormalMap_PerPixelLighting " +
        " { " +
            " float4 pos : SV_POSITION; " +
            " float3 posWorld : TEXCOORD0; " +
            " float3 normalDir : TEXCOORD1; " +
            " float3 normal : NORMAL; " +
            " float2 tex : TEXCOORD2; " +
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
        " struct vertexOutput_NoLight_NoTextureNoNormalMap " +
        " { " +
            " float4 pos : SV_POSITION; " +
            " float4 col : COLOR; " +
        " }; " + '\n';

    public static string vertexOutput_PerVertexLighting =
        "struct vertexOutput_PerVertexLighting " +
        " { " +
        " float4 pos : SV_POSITION; " +
        " float4 col : COLOR; " +
        " float2 tex : TEXCOORD1; " + "};" + '\n'
        ;

    public static string vertexOutput_PerPixelLighting =
        " struct vertexOutput_PerPixelLighting " +
        " { " +
        " float4 pos : SV_POSITION; " +
        " float3 posWorld : TEXCOORD0; " +
        " float3 normalDir : TEXCOORD1; " +
        " float2 tex : TEXCOORD2; " +
        " float4 tangent : TANGENT; " +
        " float3 normal : NORMAL; " + "};" + '\n'
        ;

    public static string Texture_Handling_Pixel =
        " float4 Texture_Handling_Pixel(vertexOutput_PerPixelLighting input) " +
        " { " +
        " float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY); " +
        " texCoordsScale *= input.tex.xy; " +
        " float4 textureColor = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY)); " +
        " textureColor = textureColor * _TextureTint; " +
        " return textureColor; " + "}" + '\n'
        ;

    public static string Texture_Handling_Vertex =
        " float4 Texture_Handling_Vertex(vertexOutput_PerVertexLighting input) " +
         " { " +
        " float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY); " +
        " texCoordsScale *= input.tex.xy; " +
        " float4 textureColor = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY)); " +
        " textureColor = textureColor * _TextureTint; " +
        " return textureColor; " + "}" + '\n'
        ;

    public static string Texture_Handling_Pixel_NoNormalMap =
        " float4 Texture_Handling_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) " +
        " { " +
            " float2 texCoordsScale = float2(_TextureTileX, _TextureTileY); " +
            " texCoordsScale *= input.tex.xy; " +
            " float4 textureColor = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY)); " +
            " textureColor = textureColor * _TextureTint; " +
            " return textureColor; " +
        " } " + '\n';

    public static string Normal_Direction_With_Normal_Map_Handling_Vertex =
       " float3 Normal_Direction_With_Normal_Map_Handling_Vertex(vertexInput_AllVariables input) " +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " +
        " float3 tangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz); " +
        " float3 normalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
        " float3 BitangentWorld = normalize(cross(normalWorld, tangentWorld) * input.tangent.w); " +
        " float3 biNormal = cross(input.normal, input.tangent.xyz) * input.tangent.w; " +
        " float2 normalCoordsScaled = float2(_NormalTileX, _NormalTileY); " +
        " normalCoordsScaled *= input.texcoord.xy; " +
        " float4 encodedNormal = tex2Dlod(_NormalMap, float4(normalCoordsScaled, float2(_NormalOffsetX, _NormalOffsetY))); " +
        " float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0, 1.0), 0.0); " +
        " localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords); " +
        " float3x3 local2WorldTranspose = float3x3(tangentWorld, BitangentWorld, normalWorld); " +
        " float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); " +
        " normalDirection = float3(_NormalMapScale, _NormalMapScale, 1.0f) * normalDirection; " +
        " return normalDirection; " + "}" + '\n'
        ;

    public static string Normal_Direction_With_Normal_Map_Handling_Pixel =
        " float3 Normal_Direction_With_Normal_Map_Handling_Pixel(vertexOutput_PerPixelLighting input) " +
          " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " +
        " float3 tangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz); " +
        " float3 normalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
        " float3 BitangentWorld = normalize(cross(normalWorld, tangentWorld) * input.tangent.w); " +
        " float3 biNormal = cross(input.normal, input.tangent.xyz) * input.tangent.w; " +
        " float2 normalCoordsScaled = float2(_NormalTileX, _NormalTileY); " +
        " normalCoordsScaled *= input.tex.xy; " +
        " float4 encodedNormal = tex2D(_NormalMap, normalCoordsScaled + float2(_NormalOffsetX, _NormalOffsetY)); " +
        " float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0, 1.0), 0.0); " +
        " localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords); " +
        " float3x3 local2WorldTranspose = float3x3(tangentWorld, BitangentWorld, normalWorld); " +
        " float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); " +
        " normalDirection = float3(_NormalMapScale, _NormalMapScale, 1.0f) * normalDirection; " +
        " return normalDirection; " + "}" + '\n'
        ;

    public static string Phong_Lighting_Vertex =
        " float4 Phong_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float3x3 modelMatrixInverse = unity_WorldToObject; " +
        " normalDirection += normalize(mul(input.normal, modelMatrixInverse)); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.vertex).xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
            " attenuation = 1.0; " +
            " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(modelMatrix, input.vertex).xyz; " +
            " float3 distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
        " } " +

        " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb; " +
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * max(0.0, dot(normalDirection, lightDirection)); " +
        " float3 specularReflection; " +
        " if (dot(normalDirection, lightDirection) < 0.0) " +
        " { specularReflection = float3(0.0, 0.0, 0.0); } " +
        " else " +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)), _CustomShininess); " +
        " } " +
        "return float4(ambientLighting * _PhongAmbientForce + diffuseReflection * _PhongDiffuseForce  + specularReflection * _PhongSpecularForce, 1.0f) " + " } " + '\n'
        ;

    public static string Phong_Lighting_Vertex_NoNormalMap =
        " float4 Phong_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float3x3 modelMatrixInverse = unity_WorldToObject; " +
        " float3 normalDirection = normalize(mul(input.normal, modelMatrixInverse)); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.vertex).xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
            " attenuation = 1.0; " +
            " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(modelMatrix, input.vertex).xyz; " +
            " float3 distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
        " } " +

        " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb; " +
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * max(0.0, dot(normalDirection, lightDirection)); " +
        " float3 specularReflection; " +
        " if (dot(normalDirection, lightDirection) < 0.0) " +
        " { specularReflection = float3(0.0, 0.0, 0.0); } " +
        " else " +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)), _CustomShininess); " +
        " } " +
        "return float4(ambientLighting * _PhongAmbientForce + diffuseReflection * _PhongDiffuseForce + specularReflection * _PhongSpecularForce, 1.0f); " + " } " + '\n'
        ;

    public static string Lambert_Lighting_Vertex =
       " float4 Lambert_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " +
        " float3 posWorld = mul(modelMatrix, input.vertex); " +
        " float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
        " normalDirection += normalize(normalDir); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
           " attenuation = 1.0f; " +
            " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz; " +
            " float distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
         " } " +
        " float NDotL = max(0.0, dot(normalDirection, lightDirection)); " +
        " float LambertDiffuse = NDotL; " +
        " float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb; " +
        " return float4 (finalColor, 1.0f); " + " } " + '\n'
        ;

    public static string Lambert_Lighting_Vertex_NoNormalMap =
       " float4 Lambert_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " +
        " float3 posWorld = mul(modelMatrix, input.vertex); " +
        " float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
        " float3 normalDirection = normalize(normalDir); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
           " attenuation = 1.0f; " +
            " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz; " +
            " float distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
         " } " +
        " float NDotL = max(0.0, dot(normalDirection, lightDirection)); " +
        " float LambertDiffuse = NDotL; " +
        " float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb; " +
        " return float4 (finalColor, 1.0f); " + " } " + '\n'
        ;

    public static string HalfLambert_Lighting_Vertex =
        " float4 HalfLambert_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " +
        " float3 posWorld = mul(modelMatrix, input.vertex); " +
        " float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
        " normalDirection += normalize(normalDir); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
          " attenuation = 1.0f; " +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz; " +
            " float distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
        " } " +
        " float3 NDotL = max(0.0, dot(normalDirection, lightDirection)); " +
        " float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0); " +
        " float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb; " +
        " return float4 (finalColor, 1.0f); " + " } " + '\n'
        ;

    public static string HalfLambert_Lighting_Vertex_NoNormalMap =
        " float4 HalfLambert_Lighting_Vertex_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " +
        " { " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " +
        " float3 posWorld = mul(modelMatrix, input.vertex); " +
        " float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
        " float3 normalDirection = normalize(normalDir); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
          " attenuation = 1.0f; " +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz; " +
            " float distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
        " } " +
        " float3 NDotL = max(0.0, dot(normalDirection, lightDirection)); " +
        " float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0); " +
        " float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb; " +
        " return float4 (finalColor, 1.0f); " + " } " + '\n'
        ;

    public static string Phong_Lighting_Pixel =
        " float3 Phong_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " +
        " { " +
        " normalDirection += normalize(input.normalDir); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
          " attenuation = 1.0f; " +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " +
            " float distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
        " } " +
        " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb; " +
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * max(0.0, dot(normalDirection, lightDirection)); " +
        " float3 specularReflection; " +
        " if (dot(normalDirection, lightDirection) < 0.0) " +
        " { specularReflection = float3(0.0, 0.0, 0.0); } " +
        " else " +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _CustomShininess); " +
        " } " +
        " return float3(ambientLighting * _PhongAmbientForce + diffuseReflection * _PhongDiffuseForce  + specularReflection * _PhongSpecularForce); " + " } " + '\n'
        ;

    public static string Phong_Lighting_Pixel_NoNormalMap =
       " float3 Phong_Lighting_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) " +
       " { " +
       " float3 normalDirection = normalize(input.normalDir); " +
       " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " +
       " float3 lightDirection; " +
       " float attenuation; " +
       " if (0.0 == _WorldSpaceLightPos0.w) " +
       " { " +
         " attenuation = 1.0f; " +
         " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
       " } " +
       " else " +
       " { " +
           " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " +
           " float distance = length(vertexToLightSource); " +
           " attenuation = 1.0 / distance; " +
           " lightDirection = normalize(vertexToLightSource); " +
       " } " +
       " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _PhongAmbientColor.rgb; " +
       " float3 diffuseReflection = attenuation * _LightColor0.rgb * _PhongDiffuseColor.rgb * max(0.0, dot(normalDirection, lightDirection)); " +
       " float3 specularReflection; " +
       " if (dot(normalDirection, lightDirection) < 0.0) " +
       " { specularReflection = float3(0.0, 0.0, 0.0); } " +
       " else " +
       " { " +
           " specularReflection = attenuation * _LightColor0.rgb * _PhongSpecularColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _CustomShininess); " +
       " } " +
       " return float3(ambientLighting * _PhongAmbientForce + diffuseReflection * _PhongDiffuseForce  + specularReflection * _PhongSpecularForce); " + " } " + '\n'
       ;

    public static string Lambert_Lighting_Pixel =
        " float3 Lambert_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " +
        " { " +
        " normalDirection += normalize(input.normalDir); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
          " attenuation = 1.0f; " +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " +
            " float distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
        " } " +
        " float NDotL = max(0.0, dot(normalDirection, lightDirection)); " +
        " float LambertDiffuse = NDotL; " +
        " float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb; " +
        " return finalColor; " + " } " + '\n'
        ;

    public static string Lambert_Lighting_Pixel_NoNormalMap =
        " float3 Lambert_Lighting_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) " +
        " { " +
        " float3 normalDirection = normalize(input.normalDir); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
          " attenuation = 1.0f; " +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " +
            " float distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
        " } " +
        " float NDotL = max(0.0, dot(normalDirection, lightDirection)); " +
        " float LambertDiffuse = NDotL; " +
        " float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb; " +
        " return finalColor; " + " } " + '\n'
        ;

    public static string HalfLambert_Lighting_Pixel =
        " float3 HalfLambert_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " +
        " { " +
        " normalDirection += normalize(input.normalDir); " +
        " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " +
        " float3 lightDirection; " +
        " float attenuation; " +
        " if (0.0 == _WorldSpaceLightPos0.w) " +
        " { " +
          " attenuation = 1.0f; " +
          " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
        " } " +
        " else " +
        " { " +
            " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " +
            " float distance = length(vertexToLightSource); " +
            " attenuation = 1.0 / distance; " +
            " lightDirection = normalize(vertexToLightSource); " +
        " } " +
        " float3 NDotL = max(0.0, dot(normalDirection, lightDirection)); " +
        " float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0); " +
        " float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb; " +
        " return finalColor; " + " } " + '\n'
        ;

    public static string HalfLambert_Lighting_Pixel_NoNormalMap =
       " float3 HalfLambert_Lighting_Pixel_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) " +
       " { " +
       " float3 normalDirection = normalize(input.normalDir); " +
       " float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz); " +
       " float3 lightDirection; " +
       " float attenuation; " +
       " if (0.0 == _WorldSpaceLightPos0.w) " +
       " { " +
         " attenuation = 1.0f; " +
         " lightDirection = normalize(_WorldSpaceLightPos0.xyz); " +
       " } " +
       " else " +
       " { " +
           " float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz; " +
           " float distance = length(vertexToLightSource); " +
           " attenuation = 1.0 / distance; " +
           " lightDirection = normalize(vertexToLightSource); " +
       " } " +
       " float3 NDotL = max(0.0, dot(normalDirection, lightDirection)); " +
       " float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0); " +
       " float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb; " +
       " return finalColor; " + " } " + '\n'
       ;

    public static string vert_PerVertexLighting_Phong =
       " vertexOutput_PerVertexLighting vert_PerVertexLighting_PhongBase(vertexInput_AllVariables input) " +
        " { " +
        " vertexOutput_PerVertexLighting output; " +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " +
        " output.col = PhongBase_Lighting_Vertex(input, normalDirection); " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " output.tex = input.texcoord; " +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_Phong_NoNormalMap =
       " vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_Phong_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " +
        " { " +
        " vertexOutput_NoTextureNoNormalMap_PerVertexLighting output; " +
        " output.col = Phong_Lighting_Vertex_NoNormalMap(input); " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_Lambert =
      " vertexOutput_PerVertexLighting vert_PerVertexLighting_Lambert(vertexInput_AllVariables input) " +
        " { " +
        " vertexOutput_PerVertexLighting output; " +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " +
        " output.col = float4(Lambert_Lighting_Vertex(input, normalDirection), 1.0); " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " output.tex = input.texcoord; " +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_Lambert_NoNormalMap =
     " vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_Lambert_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " +
       " { " +
       " vertexOutput_NoTextureNoNormalMap_PerVertexLighting output; " +
       " output.col = float4(Lambert_Lighting_Vertex_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0); " +
       " output.pos = UnityObjectToClipPos(input.vertex); " +
       " return output; " + " } " + '\n'
       ;

    public static string vert_PerVertexLighting_HalfLambert =
        " vertexOutput_PerVertexLighting vert_PerVertexLighting_HalfLambert(vertexInput_AllVariables input) " +
        " { " +
        " vertexOutput_PerVertexLighting output; " +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " +
        " output.col = float4(HalfLambert_Lighting_Vertex(input, normalDirection), 1.0); " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " output.tex = input.texcoord; " +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_HalfLambert_NoNormalMap =
        " vertexOutput_NoTextureNoNormalMap_PerVertexLighting vert_PerVertexLighting_HalfLambert_NoNormalMap(vertexInput_NoTextureNoNormalMap input) " +
        " { " +
        " vertexOutput_NoTextureNoNormalMap_PerVertexLighting output; " +
        " output.col = float4(HalfLambert_Lighting_Vertex_NoNormalMap(input).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0);" +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerVertexLighting_NoLight =
        " vertexOutput_PerVertexLighting vert_PerVertexLighting_NoLight(vertexInput_AllVariables input) " +
        " { " +
        " vertexOutput_PerVertexLighting output; " +
        " output.col = float4 (1.0f, 1.0f, 1.0f, 1.0f); " +
        " output.tex = input.texcoord; " +
        " return output; " + " } " + '\n';

    public static string vert_PerVertexLighting_NoLight_NoTextureNoNormalMap =
        " vertexOutput_NoLight_NoTextureNoNormalMap vert_PerVertexLighting_NoLight_NoTextureNoNormalMap (vertexInput_NoLight_NoTextureNoNormalMap input) " +
        " { " +
        " vertexOutput_NoLight_NoTextureNoNormalMap output; " +
        " output.pos = UnityObjectToClipPos(input.vertex);" +
        " output.col = float4 (1.0f, 1.0f, 1.0f, 1.0f); " +
        " return output; " + " } " + '\n';

    public static string frag_PerVertexLighting =
        " float4 frag_PerVertexLighting(vertexOutput_PerVertexLighting input) : COLOR " +
        " {	" +
        " float4 TextureColor = Texture_Handling_Vertex(input); " +
        " return input.col* TextureColor; " +
        " } " + '\n'
        ;

    public static string frag_PerVertexLighting_NoTextureMap =
       " float4 frag_PerVertexLighting_NoTextureMap (vertexOutput_NoTextureNoNormalMap_PerVertexLighting input): COLOR " +
       " {	" +
       " return float4(input.col.xyz * _TextureTint.xyz, 1.0f); " +
       " } " + '\n'
       ;

    public static string vert_PerPixelLighting =
        " vertexOutput_PerPixelLighting vert_PerPixelLighting(vertexInput_AllVariables input) : COLOR " +
        " { " +
        " vertexOutput_PerPixelLighting output; " +
        " float4x4 modelMatrix = unity_ObjectToWorld; " +
        " float4x4 modelMatrixInverse = unity_WorldToObject; " +
        " output.posWorld = mul(modelMatrix, input.vertex); " +
        " output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
        " output.tex = input.texcoord; " +
        " output.tangent = input.tangent; " +
        " output.normal = input.normal; " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " return output; " + " } " + '\n'
        ;

    public static string vert_PerPixelLighting_NoTextureNoNormalMap =
       " vertexOutput_NoTextureNoNormalMap_PerPixelLighting vert_PerPixelLighting_NoTextureNoNormalMap(vertexInput_NoTextureNoNormalMap input) : COLOR " +
       " { " +
       " vertexOutput_NoTextureNoNormalMap_PerPixelLighting output; " +
       " float4x4 modelMatrix = unity_ObjectToWorld; " +
       " float4x4 modelMatrixInverse = unity_WorldToObject; " +
       " output.posWorld = mul(modelMatrix, input.vertex); " +
       " output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
       " output.normal = input.normal; " +
       " output.pos = UnityObjectToClipPos(input.vertex); " +
       " return output; " + " } " + '\n'
       ;

    public static string vert_PerPixelLighting_NoNormalMap =
       " vertexOutput_NoNormalMap_PerPixelLighting vert_PerPixelLighting_NoNormalMap (vertexInput_NoNormalMap input) : COLOR " +
       " { " +
       " vertexOutput_NoNormalMap_PerPixelLighting output; " +
       " float4x4 modelMatrix = unity_ObjectToWorld; " +
       " float4x4 modelMatrixInverse = unity_WorldToObject; " +
       " output.posWorld = mul(modelMatrix, input.vertex); " +
       " output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz); " +
       " output.tex = input.texcoord; " +
       " output.normal = input.normal; " +
       " output.pos = UnityObjectToClipPos(input.vertex); " +
       " return output; " + " } " + '\n'
       ;

    public static string frag_PerPixelLighting_Phong =
        " float4 frag_PerPixelLighting_Phong(vertexOutput_PerPixelLighting input) : COLOR " +
        " {	 " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            " return float4(Texture_Handling_Pixel(input) * Phong_Lighting_Pixel(input, normalDirection), 1.0f); " +
        " } " + '\n'
        ;

    public static string frag_PerPixelLighting_Phong_NoNormalMap =
        " float4 frag_PerPixelLighting_Phong_NoNormalMap (vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR " +
        " {	 " +
            " return float4(Texture_Handling_Pixel_NoNormalMap(input).xyz * Phong_Lighting_Pixel_NoNormalMap(input), 1.0f); " +
        " } " + '\n'
        ;
    public static string frag_PerPixelLighting_Phong_NoTexture =
        " float4 frag_PerPixelLighting_Phong_NoTexture(vertexOutput_PerPixelLighting input) : COLOR " +
        " {	 " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            " return float4(Phong_Lighting_Pixel(input, normalDirection), 1.0f); " +
        " } " + '\n'
        ;

    public static string frag_PerPixelLighting_Phong_NoTextureNoNormalMap =
        " float4 frag_PerPixelLighting_Phong_NoTextureNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR " +
        " { " +
             " vertexOutput_NoNormalMap_PerPixelLighting dummyOutput; " +
            " dummyOutput.posWorld = input.posWorld; " +
            " dummyOutput.normalDir = input.normalDir; " +
            " dummyOutput.normal = input.normal; " +
            " dummyOutput.pos = input.pos; " +
            " return float4(Phong_Lighting_Pixel_NoNormalMap(dummyOutput), 1.0f); " +
        " } " + '\n';


    public static string frag_PerPixelLighting_Lambert =
        " float4 frag_PerPixelLighting_Lambert(vertexOutput_PerPixelLighting input) : COLOR " +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            " return float4(Texture_Handling_Pixel(input) * Lambert_Lighting_Pixel(input, normalDirection), 1.0f); " +
        " } " + '\n'
        ;

    public static string frag_PerPixelLighting_Lambert_NoNormalMap =
        " float4 frag_PerPixelLighting_Lambert_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR " +
        " { " +
            " return float4(Texture_Handling_Pixel_NoNormalMap(input) * Lambert_Lighting_Pixel_NoNormalMap(input).xyz* (_LambertTintColor* _LambertTintForce).xyz, 1.0); " +
         " } " + '\n';

    public static string frag_PerPixelLighting_Lambert_NoTextureMap =
        " float4 frag_PerPixelLighting_Lambert_NoTextureMap(vertexOutput_PerPixelLighting input) : COLOR " +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            "return float4(Lambert_Lighting_Pixel(input, normalDirection).xyz* (_LambertTintColor* _LambertTintForce).xyz, 1.0); " +
        " } " + '\n';

    public static string frag_PerPixelLighting_Lambert_NoTextureNoNormalMap =
        " float4 frag_PerPixelLighting_Lambert_NoTextureNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR " +
        " { " +
            " vertexOutput_NoNormalMap_PerPixelLighting dummyOutput; " +
            " dummyOutput.posWorld = input.posWorld; " +
            " dummyOutput.normalDir = input.normalDir; " +
            " dummyOutput.normal = input.normal; " +
            " dummyOutput.pos = input.pos; " +
            " return float4 (Lambert_Lighting_Pixel_NoNormalMap(dummyOutput).xyz * (_LambertTintColor * _LambertTintForce).xyz, 1.0); " +
        " } " + '\n';


    public static string frag_PerPixelLighting_HalfLambert =
        " float4 frag_PerPixelLighting_HalfLambert(vertexOutput_PerPixelLighting input) : COLOR " +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            " return float4(Texture_Handling_Pixel(input) * HalfLambert_Lighting_Pixel(input, normalDirection), 1.0); " +
        " } " + '\n'
        ;

    public static string frag_PerPixelLighting_HalfLambert_NoNormalMap =
        " float4 frag_PerPixelLighting_HalfLambert_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR " +
        " { " +
            " return float4(Texture_Handling_Pixel_NoNormalMap(input) * HalfLambert_Lighting_Pixel_NoNormalMap(input).xyz* (_LambertTintColor* _LambertTintForce).xyz, 1.0); " +
        " } " + '\n';

    public static string frag_PerPixelLighting_HalfLambert_NoTextureMap =
        " float4 frag_PerPixelLighting_HalfLambert_NoTextureMap(vertexOutput_PerPixelLighting input) : COLOR " +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            " return float4(HalfLambert_Lighting_Pixel(input, normalDirection).xyz* (_LambertTintColor* _LambertTintForce).xyz, 1.0); " +
         " } " + '\n';

    public static string frag_PerPixelLighting_HalfLambert_NoTextureMapNoNormalMap =
        " float4 frag_PerPixelLighting_HalfLambert_NoTextureMapNoNormalMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR " +
        " { " +
            " vertexOutput_NoNormalMap_PerPixelLighting dummyOutput; " +
            " dummyOutput.posWorld = input.posWorld; " +
            " dummyOutput.normalDir = input.normalDir; " +
            " dummyOutput.normal = input.normal; " +
            " dummyOutput.pos = input.pos; " +
            " return float4(HalfLambert_Lighting_Pixel_NoNormalMap(dummyOutput).xyz* (_LambertTintColor* _LambertTintForce).xyz, 1.0); " +
         " } " + '\n';

    public static string frag_PerPixelLighting_NoLight =
        " float4 frag_PerPixelLighting_NoLight(vertexOutput_PerPixelLighting input) : COLOR " +
        " { " +
            " float4 TextureColor = Texture_Handling_Pixel(input); " +
            "return float4(TextureColor.xyz, 1.0f); " + " } " + '\n'
        ;

    public static string frag_PerPixelLighting_NoLight_NoNormalMap =
       " float4 frag_PerPixelLighting_NoLight_NoNormalMap(vertexOutput_NoNormalMap_PerPixelLighting input) : COLOR" +
       " { " +
           " float4 TextureColor = Texture_Handling_Pixel_NoNormalMap(input); " +
           " return float4(TextureColor.xyz, 1.0f); " + " } " + '\n'
       ;


    public static string frag_PerPixelLighting_NoLight_NoTextureMap =
        "  float4 frag_PerPixelLighting_NoLight_NoTextureMap(vertexOutput_NoTextureNoNormalMap_PerPixelLighting input) : COLOR " +
        " { " +
        " return float4(_TextureTint.xyz, 1.0f); " +
        " } " + '\n';


}
