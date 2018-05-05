using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Crosstales.FB;

public class ShaderExport : MonoBehaviour {

    [Header("Editable Shader Files References")]
    public Shader editableShaderReferences;
    public TextAsset[] neededReferencesForEditableShader;

    #region SHADER_VARIABLES

    public static string RenderType = "Opaque";
    public static string LightModeType = "ForwardBase";
    public static string shaderName = "GreatTest";
    public static string temporalShaderName = "NewShader";

    public static string currentVertexFunction = "vert_PerVertexLighting_PhongBase";
    public static string currentFragmentFunction = "frag_PerVertexLighting";

    #endregion

    private string temporalSpellBook;


    // Use this for initialization
    void Start()
    {
        //CreateTemporalSpellBook();
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

        PrepareSpellBook();
        
        string newRoute = FileBrowser.SaveFile("Save Shader to...", Application.dataPath, shaderName, "shader");
        //print(newRoute);
        
        StreamWriter newShader = new StreamWriter(newRoute, true, System.Text.Encoding.UTF8);
        newShader.Write(shaderText);
        newShader.Close();

        string spellBookRoute = newRoute.Substring(0, newRoute.Length - 7 - shaderName.Length);
        spellBookRoute += "/SpellBook.cginc";

        StreamWriter customSpellBook = new StreamWriter(spellBookRoute, true, System.Text.Encoding.UTF8);
        customSpellBook.Write(temporalSpellBook);
        customSpellBook.Close();

    }

    public void CreateTemporalSpellBook()
    {
        // Variables parts

        temporalSpellBook = SpellBookFunctions.necessaryIncludes + 
                            SpellBookFunctions.Texture_Handling_Variables +
                            SpellBookFunctions.Normal_Handling_Variables +
                            SpellBookFunctions.Phong_Variables +
                            SpellBookFunctions.vertexInput_AllVariables +
                            SpellBookFunctions.vertexOutput_PerVertexLighting +
                            SpellBookFunctions.vertexOutput_PerPixelLighting;

        //Functions parts

        temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel +
                             SpellBookFunctions.Texture_Handling_Vertex +
                             SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Pixel +
                             SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Vertex +
                             SpellBookFunctions.PhongBase_Lighting_Vertex +
                             SpellBookFunctions.PhongAdd_Lighting_Vertex +
                             SpellBookFunctions.Lambert_Lighting_Vertex +
                             SpellBookFunctions.HalfLambert_Lighting_Vertex +
                             SpellBookFunctions.Phong_Lighting_Pixel +
                             SpellBookFunctions.Lambert_Lighting_Pixel +
                             SpellBookFunctions.HalfLambert_Lighting_Pixel;

        //Vert and frag functions

        temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_PhongBase +
                             SpellBookFunctions.vert_PerVertexLighting_PhongAdd +
                             SpellBookFunctions.vert_PerVertexLighting_Lambert +
                             SpellBookFunctions.vert_PerVertexLighting_HalfLambert +
                             SpellBookFunctions.vert_PerPixelLighting +
                             SpellBookFunctions.frag_PerVertexLighting +
                             SpellBookFunctions.frag_PerPixelLighting_Phong +
                             SpellBookFunctions.frag_PerPixelLighting_Lambert +
                             SpellBookFunctions.frag_PerPixelLighting_HalfLambert + 
                             SpellBookFunctions.frag_PerPixelLighting_NoLight;

        string spellBookRoute = Application.dataPath + "/SpellBook.cginc";
        print(spellBookRoute);
        if (File.Exists(spellBookRoute) != true)
        {
            StreamWriter customSpellBook = new StreamWriter(spellBookRoute, true, System.Text.Encoding.UTF8);

            customSpellBook.Write(temporalSpellBook);
            customSpellBook.Close();
        }
        



    }

    public void PrepareSpellBook()
    {
        temporalSpellBook = SpellBookFunctions.necessaryIncludes;

        switch (ShaderEdition.currentInstance._Current_Lighting_Model)
        {
            case (0):
                {
                    if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //No Light, Pixel
                    {
                        currentVertexFunction = "vert_PerPixelLighting";
                        currentFragmentFunction = "frag_PerPixelLighting_NoLight";
                    }
                    else //No Light, Vertex
                    {
                        currentVertexFunction = "vert_PerVertexLighting_NoLight";
                        currentFragmentFunction = "frag_PerVertexLighting";

                    }
                    break;
                }
            case (1):
                {
                    if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Phong, Pixel
                    {
                        currentVertexFunction = "vert_PerPixelLighting";
                        currentFragmentFunction = "frag_PerPixelLighting_Phong";
                    }
                    else //Phong, Vertex
                    {
                        currentVertexFunction = "vert_PerVertexLighting_PhongBase";
                        currentFragmentFunction = "frag_PerVertexLighting";

                    }
                    break;
                }
            case (2):
                {
                    if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Lambert, Pixel
                    {
                        currentVertexFunction = "vert_PerPixelLighting";
                        currentFragmentFunction = "frag_PerPixelLighting_Lambert";
                    }
                    else //Lambert, Vertex
                    {
                        currentVertexFunction = "vert_PerVertexLighting_Lambert";
                        currentFragmentFunction = "frag_PerVertexLighting";

                    }
                    break;
                }
            case (3):
                {
                    if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //HalfLambert, Pixel
                    {
                        currentVertexFunction = "vert_PerPixelLighting";
                        currentFragmentFunction = "frag_PerPixelLighting_HalfLambert";
                    }
                    else //HalfLambert, Vertex
                    {
                        currentVertexFunction = "vert_PerVertexLighting_HalfLambert";
                        currentFragmentFunction = "frag_PerVertexLighting";

                    }
                    break;
                }
            case (4):
                {
                    if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Phong, Pixel
                    {
                        currentVertexFunction = "vert_PerPixelLighting";
                        currentFragmentFunction = "frag_PerPixelLighting_Phong";
                    }
                    else //PhongAdd, Vertex
                    {
                        currentVertexFunction = "vert_PerVertexLighting_PhongAdd";
                        currentFragmentFunction = "frag_PerVertexLighting";

                    }
                    break;
                }

        }



    }





  
    static string shaderText =
       " Shader" + '"' + "Custom/" + shaderName + '"' + "  " + '\n' + 
        " { " + '\n' +
            " SubShader " + '\n' +
            " { " + '\n' +
                " Pass " + '\n' +
                "{ " + '\n' +
                    " Tags { " + '"' + "LightMode" + '"' + " = " + '"' + LightModeType + '"' + " } " + "  " + '\n' +
                    " LOD 100 " + '\n' +
                    "CGPROGRAM" + '\n' +
                    "  #include " + '"' + "SpellBook.cginc" + '"' + "  " + '\n' +
                    "  #pragma vertex " +  currentVertexFunction + "  " + '\n' +
                    "  #pragma fragment " + currentFragmentFunction + "  " + '\n' +
                    " ENDCG " + "  " + '\n' +
                " } " +
	
            " } " +
        " } ";




 }