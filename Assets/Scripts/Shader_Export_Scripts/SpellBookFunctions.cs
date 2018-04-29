using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookFunctions : MonoBehaviour {

    //This is a library of the functions that are available in SpellBook.
    // Ultima fecha de actualización: 29/04/2018

    public static string Texture_Handling_Variables =
        " uniform sampler2D _CustomTexture; " +
        " uniform fixed4 _TextureTint; " +
        " uniform float _TextureTileX; " +
        " uniform float _TextureTileY; " +
        " uniform float _OffsetTileX; " +
        " uniform float _OffsetTileY; "
        ;

    public static string Normal_Handling_Variables =
        " uniform sampler2D _NormalMap; " +
        " uniform float _NormalTileX; " +
        " uniform float _NormalTileY; " +
        " uniform float _NormalOffsetX; " +
        " uniform float _NormalOffsetY; "
        ;

    public static string Phong_Variables = 
        " uniform fixed4 _CustomSpecularColor; " +
        " uniform float _CustomShininess; " +
        " uniform float4 _Color; " +
        " uniform float _DiffuseColor; " +
        " uniform float _Shininess; " +
        " uniform float4 _SpecularColor; "
        ;


    //============FUNCTIONS ===================================================

    public static string vertexInput_AllVariables = 
        "struct vertexInput_AllVariables " + 
        " { " + 
        " float4 vertex : POSITION; " + 
		" float3 normal : NORMAL; " + 
		" float2 texcoord : TEXCOORD0; " +
		" float4 tangent : TANGENT; " + " };"
		;

    public static string vertexOutput_PerVertexLighting =
        "struct vertexOutput_PerVertexLighting " + 
        " { " +
        " float4 pos : SV_POSITION; " +
		" float4 col : COLOR; " + 
		" float2 tex : TEXCOORD1; " + "};"
		;

    public static string vertexOutput_PerPixelLighting =
        " struct vertexOutput_PerPixelLighting " + 
        " { " +
        " float4 pos : SV_POSITION; " +
		" float3 posWorld : TEXCOORD0; " +
		" float3 normalDir : TEXCOORD1; " +
		" float2 tex : TEXCOORD2; " +
		" float4 tangent : TANGENT; " +
		" float3 normal : NORMAL; " + "};"
		;

    public static string Texture_Handling_Pixel =
        " float4 Texture_Handling_Pixel(vertexOutput_PerPixelLighting input) " +
        " { " +
        " float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY); " +
        " texCoordsScale *= input.tex.xy; " +
		" float4 textureColor = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY)); " +
        " textureColor = textureColor * _TextureTint; " +
        " return textureColor; " + "}"
        ;

    public static string Texture_Handling_Vertex =
        " float4 Texture_Handling_Vertex(vertexOutput_PerVertexLighting input) " +
         " { " +
        " float2 texCoordsScale = float2 (_TextureTileX, _TextureTileY); " +
        " texCoordsScale *= input.tex.xy; " +
        " float4 textureColor = tex2D(_CustomTexture, texCoordsScale + float2(_OffsetTileX, _OffsetTileY)); " +
        " textureColor = textureColor * _TextureTint; " +
        " return textureColor; " + "}"
        ;

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
        " normalCoordsScaled *= input.texcoord; " +
		" float4 encodedNormal = tex2D(_NormalMap, normalCoordsScaled + float2(_NormalOffsetX, _NormalOffsetY)); " +
        " float3 localCoords = float3(2.0 * encodedNormal.ag - float2(1.0, 1.0), 0.0); " +
        " localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords); " +
        " float3x3 local2WorldTranspose = float3x3(tangentWorld, BitangentWorld, normalWorld); " +
        " float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose)); " +
        " normalDirection = float3(_NormalMapScale, _NormalMapScale, 1.0f) * normalDirection; " +
        " return normalDirection; " + "}"
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
        " return normalDirection; " + "}"
        ;

    public static string PhongBase_Lighting_Vertex =
        " float4 PhongBase_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " +
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

        " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb; " +
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection)); " +
        " float3 specularReflection; " +
        " if (dot(normalDirection, lightDirection) < 0.0) " +
        " { specularReflection = float3(0.0, 0.0, 0.0); } " +
        " else " +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _SpecularColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)), _Shininess); " +
        " } " +
        " return float4(ambientLighting + diffuseReflection + specularReflection, 1.0); " + " } "
        ;

    public static string PhongAdd_Lighting_Vertex =
       "  float4 PhongAdd_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " +
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
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection)); " +
        " float3 specularReflection; " +
        " if (dot(normalDirection, lightDirection) < 0.0) " +
        " { specularReflection = float3(0.0, 0.0, 0.0); } " +
        " else " +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _SpecularColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)), _Shininess); " +
        " } " +
        " return float4(diffuseReflection + specularReflection, 1.0); " + " } "
        ;


    public static string Lambert_Lighting_Vertex =
       " float3 Lambert_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " +
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
        " return finalColor; " + " } "
        ;

    public static string HalfLambert_Lighting_Vertex =
        " float3 HalfLambert_Lighting_Vertex(vertexInput_AllVariables input, float3 normalDirection) " +
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
        " return finalColor; " + " } "
        ;

    public static string Phong_Lighting_Pixel =
        " float3 Phong_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " +
        " { " +
        " normalDirection += normalize(input.normalDir); " +
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
        " float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb; " +
        " float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection)); " +
        " float3 specularReflection; " +
        " if (dot(normalDirection, lightDirection) < 0.0) " +
        " { specularReflection = float3(0.0, 0.0, 0.0); } " +
        " else " +
        " { " +
            " specularReflection = attenuation * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess); " +
        " } " +
        " return float3(ambientLighting + diffuseReflection + specularReflection); " + " } "
        ;

    public static string Lambert_Lighting_Pixel =
        " float3 Lambert_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " +
        " { " +
        " normalDirection += normalize(input.normalDir); " +
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
        " return finalColor; " + " } "
        ;

    public static string HalfLambert_Lighting_Pixel =
        " float3 HalfLambert_Lighting_Pixel(vertexOutput_PerPixelLighting input, float3 normalDirection) " +
        " { " +
        " normalDirection += normalize(input.normalDir); " +
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
        " return finalColor; " + " } "
        ;

    public static string vert_PerVertexLighting_PhongBase =
       " vertexOutput_PerVertexLighting vert_PerVertexLighting_PhongBase(vertexInput_AllVariables input) " +
        " { " +
        " _Shininess = _CustomShininess; " +
        " _SpecularColor = _CustomSpecularColor; " +
        " _Color = _TextureTint; " +
        " vertexOutput_PerVertexLighting output; " +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " +
        " output.col = PhongBase_Lighting_Vertex(input, normalDirection); " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " return output; " + " } "
        ;

    public static string vert_PerVertexLighting_Lambert =
      " vertexOutput_PerVertexLighting vert_PerVertexLighting_Lambert(vertexInput_AllVariables input) " +
        " { " +
        " vertexOutput_PerVertexLighting output; " +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " +
        " output.col = float4(Lambert_Lighting_Vertex(input, normalDirection), 1.0); " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " return output; " + " } "
        ;

    public static string vert_PerVertexLighting_HalfLambert =
        " vertexOutput_PerVertexLighting vert_PerVertexLighting_HalfLambert(vertexInput_AllVariables input) " +
        " { " +
        " vertexOutput_PerVertexLighting output; " +
        " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " +
        " output.col = float4(HalfLambert_Lighting_Vertex(input, normalDirection), 1.0); " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " return output; " + " } "
        ;

    public static string vert_PerVertexLighting_PhongAdd =
        " vertexOutput_PerVertexLighting vert_PerVertexLighting_PhongAdd(vertexInput_AllVariables input) " +
        " { " +
        " _Shininess = _CustomShininess; " +
        " _SpecularColor = _CustomSpecularColor; " +
        " _Color = _TextureTint; " +
        " vertexOutput_PerVertexLighting output; " +
        " float normalDirection = Normal_Direction_With_Normal_Map_Handling_Vertex(input); " +
        " output.col = PhongAdd_Lighting_Vertex(input, normalDirection); " +
        " output.pos = UnityObjectToClipPos(input.vertex); " +
        " return output; " + " } "
        ;

    public static string frag_PerVertexLighting =
        " float4 frag_PerVertexLighting(vertexOutput_PerVertexLighting input) : COLOR " +
        " {	" +
        " float4 TextureColor = Texture_Handling_Vertex(input); " +
        " return input.col* TextureColor; " +
        " } "
        ;

    public static string vert_PerPixelLighting =
        " vertexOutput_PerPixelLighting vert_PerPixelLighting(vertexInput_AllVariables input) " +
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
        " return output; " + " } "
        ;

    public static string frag_PerPixelLighting_Phong =
        " float4 frag_PerPixelLighting_Phong(vertexOutput_PerPixelLighting input) : COLOR " +
        " {	 " +
            " _Shininess = _CustomShininess; " +
            " _SpecularColor = _CustomSpecularColor; " +
            " _Color = _TextureTint; " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            " return float4(Texture_Handling_Pixel(input) * Phong_Lighting_Pixel(input, normalDirection), 1.0f); " +
        " } "
        ;

    public static string frag_PerPixelLighting_Lambert =
        " float4 frag_PerPixelLighting_Lambert(vertexOutput_PerPixelLighting input) : COLOR " +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            " return float4(Texture_Handling_Pixel(input) * Lambert_Lighting_Pixel(input, normalDirection), 1.0f); " +
        " } "
        ;

    public static string frag_PerPixelLighting_HalfLambert =
        " float4 frag_PerPixelLighting_HalfLambert(vertexOutput_PerPixelLighting input) : COLOR " +
        " { " +
            " float3 normalDirection = Normal_Direction_With_Normal_Map_Handling_Pixel(input); " +
            " return float4(Texture_Handling_Pixel(input) * HalfLambert_Lighting_Pixel(input, normalDirection), 1.0); " +
        " } "
        ;

    public static string frag_PerPixelLighting_NoLight =
        " float4 frag_PerPixelLighting_NoLight(vertexOutput_PerPixelLighting input) : COLOR " +
        " { " +
            " float4 TextureColor = Texture_Handling_Pixel(input); " +
            " return (TextureColor); " + " } "
        ;


}
