﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Crosstales.FB;

public class ShaderExport : MonoBehaviour {

    [Header("Editable Shader Files References")]
    public Shader editableShaderReferences;
    public TextAsset[] neededReferencesForEditableShader;


    public static string RenderType = "Opaque";
    public static string LightModeType = "ForwardBase";
    public static string shaderName = "TestExport";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ReadFile()
    {
        //StreamReader shaderFound = new StreamReader(Application.dataPath, System.Text.Encoding.UTF8);
        //string textFound = shaderFound.ReadToEnd();

    }



    public void SaveFile()
    {
        string newRoute = FileBrowser.SaveFile("Save Shader to...", Application.dataPath, shaderName, "shader");
        StreamWriter newShader = new StreamWriter(newRoute, true, System.Text.Encoding.UTF8);
        newShader.Write(shaderText);

        newShader.Close();
    }

    static string shaderText =
        "Shader " + '"' + "Shaderonomicon/" + shaderName + '"' +
        "{  Properties { " +
        "_MainTex(" + '"' + "Texture"+ '"' + ", 2D) = "+ '"' + "white"+ '"' + "{} " +
        "_CustomTexture("+ '"' + "Custom Texture"+ '"' + ", 2D) = " + '"' + "white" + '"' + "{} " +
        "_TextureTint(" + '"' + "Custom Texture Tint"+ '"' + ", Color) = (1.0, 1.0, 1.0, 1.0) " +
        "_NormalMap(" + '"'+ "Normal Map"+ '"' + ", 2D) = "+ '"' + "bump"+ '"' + "{} " +
        "_BumpMap(" + '"' + "Bump Map"+ '"' + ", 2D) = " + '"' + "white" + '"' + "{} " +
        "_NormalMapScale(" + '"' + "Normal Map Scale"+ '"' + ", float) = 1.0 " + 
        "_MaxHeightBumpMap(" + '"' + "Bump Map Max Height"+ '"' + ", float) = 0.5 " +
        "_MaxTexCoordOffset(" + '"'+ "Bump Map Max Texture Coordinate offset" + '"' + ", float) = 0.5 " +
        "_CustomAmbientLightForce(" + '"' + "Ambient Light Force"+ '"' + ", float) = 0.75 " +
        "_CustomSpecularColor(" + '"' + "Specular Color"+'"'+ ", Color) = (1.0, 1.0, 1.0, 1.0) " +
        "_CustomShininess("+ '"' + "Shininess"+ '"' + ", Range(0.0, 1.0)) = 0.5 " +
        "_PhongDiffuseColor(" + '"' + "Phong Diffuse Color"+ '"' + ", Color) = (1.0, 1.0, 1.0, 1.0) " + 
        "_PhongSpecularColor(" + '"' + "Phong Specular Color" + '"' + ", Color) = (1.0, 1.0, 1.0, 1.0) " +
        "_PhongSpecularGlossiness(" + '"' + "Phong Specular Glossiness"+ '"' + ", Range(0.0, 1.0)) = 0.5 " +
        "_PhongSpecularPower(" + '"' + "Phong Specular Power"+ '"' + ", float) = 1.0 " +
        "_TextureTileX("+'"'+ "Texture Tiling X"+ '"' + ", float) = 1.0 " +
        "_TextureTileY(" + '"' + "Texture Tiling Y" + '"' + ", float) = 1.0 " +
        "_OffsetTileX(" + '"' + "Offset Tiling X" + '"' + ", float) = 0.0 " +
        "_OffsetTileY(" + '"' + "Offset Tiling Y" + '"' + ", float) = 0.0 " +
        "_LightingModel(" + '"' + "Lighting Model"+'"'+ ", int) = 0 " +
    "}" +
     "SubShader{" +
        "Tags { " + '"' + "LightMode" + '"' + " = " + '"' + LightModeType + '"' +" " + '"' + "RenderType" + '"' + " = " + '"' + RenderType + '"' + " }" +
        "LOD 100  " +

        "  Pass  {" +
        "CGPROGRAM  " +
            "#pragma vertex vert  " +
            "#pragma fragment frag  " +
        "  # include " + '"' + "UnityCG.cginc"+ '"' +
        "  # include " + '"' + "UnityLightingCommon.cginc"+ '"' +
        
        "uniform sampler2D _MainTex;" +
        "uniform float4 _MainTex_ST;" +

        "uniform sampler2D _CustomTexture;" +
        "uniform float4 _CustomTexture_ST;" +
        "uniform fixed4 _TextureTint;" +

        "uniform sampler2D _NormalMap;" +
        "uniform float4 _NormalMap_ST;" +
        "uniform half _NormalMapScale = 1.0f;" +

        "uniform sampler2D _BumpMap;" +
        "uniform float4 _BumpMap_ST;" +
        "uniform float _MaxHeightBumpMap = 5.0f;" +
        "uniform float _MaxTexCoordOffset = 3.0f;" +

        "uniform float _CustomAmbientLightForce = 0.75f;" +
        "uniform fixed4 _CustomSpecularColor;" +
        "uniform float _CustomShininess = 1.0f;" +

        "uniform float _PhongSpecularPower = 0.5f;" +
        "uniform float _PhongSpecularGlossiness = 0.5f;" +
        "uniform fixed4 _PhongDiffuseColor;" +
        "uniform fixed4 _PhongSpecularColor;" +

        "uniform float _MinnaertRoughness = 0.5f;" +
        "uniform fixed4 _MinnaertDiffuseColor;" +

        "uniform float _TextureTileX;" +
        "uniform float _TextureTileY;" +

        "uniform float _OffsetTileX;" +
        "uniform float _OffsetTileY;" +

        "uniform int _LightingModel;" +

        "struct vertexInput     {" +
            "float4 vertex : POSITION;" +
            "float2 texcoord : TEXCOORD0;" +
            "float3 normal : NORMAL;" +
            "float4 tangent : TANGENT;" +
            "};" +
        "struct vertexOutput    {" +
            "float4 pos : SV_POSITION;" +
            "float4 worldPosition : TEXCOORD0;" +
            "float2 tex : TEXCOORD1;" +
            "float3 NormalWorld : TEXCOORD2;" +
            "float3 TangentWorld : TEXCOORD3;" +
            "float3 BitangentWorld : TEXCOORD4;" +
            "float3 viewDirWorld : TEXCOORD5;" +
            "float3 viewDirInScaledSurfaceCoords : TEXCOORD6;" +
            "};" +

        "vertexOutput vert(vertexInput input) {" +
            "vertexOutput output;" +
            "fixed2 tileVector = fixed2(_TextureTileX, _TextureTileY);" +
            "fixed2 offsetVector = fixed2(_OffsetTileX, _OffsetTileY);" +
            "_NormalMap_ST = float4(tileVector, offsetVector);" +
            "_MainTex_ST = float4(tileVector, offsetVector);" +
            "_BumpMap_ST = float4(tileVector, offsetVector);" +
            "_CustomTexture_ST = float4(tileVector, offsetVector);" +

            "float4x4 modelMatrix = unity_ObjectToWorld;" +
            "float4x4 modelMatrixInverse = unity_WorldToObject;" +

            "output.TangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz);" +
            "output.NormalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);" +
            "output.BitangentWorld = normalize(cross(output.NormalWorld, output.TangentWorld) * input.tangent.w);" +

            "float3 biNormal = cross(input.normal, input.tangent.xyz) * input.tangent.w;" +
            "float3 viewDirInObjectCoords = mul(modelMatrixInverse, float4(_WorldSpaceCameraPos, 1.0).xyz) - input.vertex.xyz;" +
            "float3x3 localSurface2ScaledObjectT = float3x3(input.tangent.xyz, biNormal, input.normal);" +
   
    "output.viewDirInScaledSurfaceCoords = mul(localSurface2ScaledObjectT, viewDirInObjectCoords);" +
   
    "output.worldPosition = mul(modelMatrix, input.vertex);" +
    "output.viewDirWorld = normalize(_WorldSpaceCameraPos - output.worldPosition.xyz);" +

    "output.tex = input.texcoord;" +
    "output.pos = UnityObjectToClipPos(input.vertex);" +

    "return output;" + "}" +

"float4 LightingModelsResult(int LightModel, vertexOutput input, float3 normalDirection) {" +
    "if (LightModel == 1)   {  " +
        "float3 viewDirection = normalize(_WorldSpaceCameraPos - input.worldPosition.xyz);" +
        "float3 lightDirection;" +
        "float attenuation;" +
        "if (0.0 == _WorldSpaceLightPos0.w)" +
        "{" +
            "attenuation = 1.0f;" +
            "lightDirection = normalize(_WorldSpaceLightPos0.xyz);" +
        "}" +
        "else  {" +
            "float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.worldPosition.xyz;" +
            "float distance = length(vertexToLightSource);" +
            "attenuation = 1.0 / distance;" +
            "lightDirection = normalize(vertexToLightSource);" +
        "}" +

        "float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _TextureTint.rgb;" +
        "float3 diffuseReflection = attenuation * _LightColor0.rgb * _TextureTint.rgb * max(0.0f, dot(normalDirection, lightDirection));" +
        "float3 specularReflection;" +
        "if (dot(normalDirection, lightDirection) < 0.0) { specularReflection = float3(0.0, 0.0, 0.0); }" +
        "else { specularReflection = attenuation * _LightColor0.rgb * _CustomSpecularColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _CustomShininess);" +
        "}" +
        "return float4(ambientLighting + diffuseReflection + specularReflection, 1.0f);" +
    "}" +
    "else if (LightModel == 2) {" +
        "float3 lightDirection;" +
        "float attenuation;" +
        "if (0.0 == _WorldSpaceLightPos0.w)" +
        "{" +
            "attenuation = 1.0f;" +
            "lightDirection = normalize(_WorldSpaceLightPos0.xyz);" +
        "}" +
        "else  {" +
            "float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.worldPosition.xyz;" +
            "float distance = length(vertexToLightSource);" +
            "attenuation = 1.0 / distance;" +
            "lightDirection = normalize(vertexToLightSource);" +
        "}" +

        "float NDotL = max(0.0, dot(normalDirection, lightDirection));" +
        "float LambertDiffuse = NDotL * _TextureTint.rgb;" +
        "float3 finalColor = LambertDiffuse * attenuation * _LightColor0.rgb;" +

        "return float4(finalColor, 1.0f);" +
    "}" +
    "else if (LightModel == 3)" +
    "{" +
        "float3 lightDirection;" +
        "float attenuation;" +
        "if (0.0 == _WorldSpaceLightPos0.w)" +
        "{" +
            "attenuation = 1.0f;" +
            "lightDirection = normalize(_WorldSpaceLightPos0.xyz);" +
        "}" +
        "else  {" +
            "float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.worldPosition.xyz;" +
            "float distance = length(vertexToLightSource);" +
            "attenuation = 1.0 / distance;" +
            "lightDirection = normalize(vertexToLightSource);" +
        "}" +

        "float3 NDotL = max(0.0, dot(normalDirection, lightDirection));" +
        "float HalfLambertDiffuse = pow(NDotL * 0.5 + 0.5, 2.0) * _TextureTint.rgb;" +
        "float3 finalColor = HalfLambertDiffuse * attenuation * _LightColor0.rgb;" +

        "return float4(finalColor, 1.0);" +

    "}" +
    "else if (LightModel == 4)" +
    "{" +
        "float3 viewDirection = normalize(_WorldSpaceCameraPos - input.worldPosition.xyz);" +
        "float3 lightDirection;" +
        "float attenuation;" +
        "if (0.0 == _WorldSpaceLightPos0.w)" +
        "{" +
            "attenuation = 1.0f;" +
            "lightDirection = normalize(_WorldSpaceLightPos0.xyz);" +
        "}" +
        "else  {" +
            "float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.worldPosition.xyz;" +
            "float distance = length(vertexToLightSource);" +
            "attenuation = 1.0 / distance;" +
            "lightDirection = normalize(vertexToLightSource);" +
        "}" +
        "float3 lightReflectDirection = reflect(-lightDirection, normalDirection);" +
        "float NDotL = max(0.0, dot(normalDirection, lightDirection));" +
        "float RDotV = max(0.0, dot(lightReflectDirection, viewDirection));" +
        "float3 specularity = pow(RDotV, _PhongSpecularGlossiness / 4) * _PhongSpecularPower * _PhongSpecularColor.rgb;" +
        "float3 lightingModel = NDotL * _PhongDiffuseColor + specularity;" +
        "float3 attenColor = attenuation * _LightColor0.rgb;" +
        "float4 finalDiffuse = float4(lightingModel * attenColor, 1.0f);" +
        "return finalDiffuse;" +
    "}" +
    "else" +
    "{ return float4(1.0f, 1.0f, 1.0f, 1.0f); }" +
"}" +

"float4 frag(vertexOutput input) : COLOR    " +
         "{" +
            "_MainTex = _CustomTexture;		" +

            "float height = _MaxHeightBumpMap * (-0.5 + tex2D(_BumpMap, _BumpMap_ST.xy * input.tex.xy + _BumpMap_ST.zw).x);" +
            "float2 texCoordOffsets = clamp(height * input.viewDirInScaledSurfaceCoords.xy / input.viewDirInScaledSurfaceCoords.z, -_MaxTexCoordOffset, +_MaxTexCoordOffset);" +
            "float4 encodedNormal = tex2D(_NormalMap, _NormalMap_ST.xy * (input.tex.xy + texCoordOffsets) + _NormalMap_ST.zw);" +
            "float3 localCoords = float3(2.0 * encodedNormal.a - 1.0, 2.0 * encodedNormal.g - 1.0, 0.0);" +
            "localCoords.z = 1-0 - 0.5 * dot (localCoords, localCoords);" +
            "float3x3 local2WorldTranspose = float3x3(input.TangentWorld, input.BitangentWorld, input.NormalWorld);" +
            "float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));" + 
        
            "float4 lightingModelCalculation = LightingModelsResult(_LightingModel, input, normalDirection);" + 
            "float4 finalColor = tex2D(_CustomTexture, input.tex.xy) * lightingModelCalculation;" + 
			"return finalColor;" + 
         "}" +  
         "ENDCG" + 
		"}" + 
	"}" + 
"}";
       
        
        
        
        
        
        
        
        
        
        
        
        
        
        
       







}
