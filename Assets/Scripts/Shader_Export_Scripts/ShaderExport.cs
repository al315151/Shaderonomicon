using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Crosstales.FB;

public class ShaderExport : MonoBehaviour {


    #region SHADER_VARIABLES

    public static string RenderType = "Opaque";
    public static string LightModeType = "ForwardBase";
    public string shaderName = "GreatTest";

    public static string currentVertexFunction = "vert_PerVertexLighting_Phong";
    public static string currentFragmentFunction = "frag_PerVertexLighting";

    public static string shaderNecessaryFunctions = "";

    #endregion

    private string temporalSpellBook;

    string texture_Variables;
    string Normal_Handling_Variables;
    string Phong_Variables;
    string Lambert_Variables;
    string NoTextureMap_Variables;
    string transparency; // TO DO

    // Use this for initialization
    void Start()
    {
        //CreateTemporalSpellBook();
    }

    public void SaveFile()
    {
        StoreDesiredVariables();

        PrepareSpellBook();

        shaderNecessaryFunctions = temporalSpellBook;

        string vertexFunction = "  #pragma vertex " + currentVertexFunction + "  " + '\n' ;
        string PixelFunction = "  #pragma fragment " + currentFragmentFunction + "  " + '\n';

        shaderName = ShaderEdition.currentInstance.ShaderName;
        string shaderTextTitle = " Shader" + '"' + "Shaderonomicon/" + shaderName + '"' + "  " + '\n';

        string folderPath = FileBrowser.OpenSingleFolder("Choose the folder to save shader...");
        if (folderPath == "") { return; }

        else if (File.Exists(folderPath + '/' + shaderName + ".shader"))
        {
            //print("DELETE THIS");
            File.Delete(folderPath + '/' + shaderName + ".shader");
        }
        else
        {            
            //print("El directorio no existía, por lo que creamos doc!!");
        }

        StreamWriter newShader = new StreamWriter(folderPath + '/' + shaderName + ".shader", true, System.Text.Encoding.UTF8);
        newShader.Write(shaderTextTitle);
        newShader.Write(shaderTextStart);
        newShader.Write(vertexFunction);
        newShader.Write(PixelFunction);
        newShader.Write(shaderNecessaryFunctions + '\n');
        newShader.Write(shaderTextEnd);
        newShader.Close();

        if (ShaderEdition.currentInstance.ExitMenu_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   ShaderEdition.currentInstance.ExitApplication();        }

        
    }

    //For now, we will not use this function, as we will only create one document.
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
                             SpellBookFunctions.Phong_Lighting_Vertex +
                             SpellBookFunctions.Lambert_Lighting_Vertex +
                             SpellBookFunctions.HalfLambert_Lighting_Vertex +
                             SpellBookFunctions.Phong_Lighting_Pixel +
                             SpellBookFunctions.Lambert_Lighting_Pixel +
                             SpellBookFunctions.HalfLambert_Lighting_Pixel;

        //Vert and frag functions

        temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Phong +
                             SpellBookFunctions.vert_PerVertexLighting_Lambert +
                             SpellBookFunctions.vert_PerVertexLighting_HalfLambert +
                             SpellBookFunctions.vert_PerPixelLighting +
                             SpellBookFunctions.frag_PerVertexLighting +
                             SpellBookFunctions.frag_PerPixelLighting_Phong +
                             SpellBookFunctions.frag_PerPixelLighting_Lambert +
                             SpellBookFunctions.frag_PerPixelLighting_HalfLambert + 
                             SpellBookFunctions.frag_PerPixelLighting_NoLight;

        string spellBookRoute = Application.dataPath + "/SpellBook.cginc";
        //print(spellBookRoute);
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
        //This means we ain't got no texture!
        if (ShaderEdition.currentInstance.IsNewTextureApplied == false)
        {
            //We ain't got a normal map, either!
            if (ShaderEdition.currentInstance.IsNormalMapApplied == false)
            {
                switch (ShaderEdition.currentInstance._Current_Lighting_Model)
                {
                    case (0):
                        {
                            //print("Entramos en No Light, Pixel, no normal map, no texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //No Light, Pixel, no normal map, no texture
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoTextureNoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_NoLight_NoTextureMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoTextureNoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_NoLight_NoTextureMap";
                                //print(temporalSpellBook);
                            }
                            else //No Light, Vertex, no normal map, no texture
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoLight_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoLight_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_NoLight_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting_NoTextureMap;
                                currentVertexFunction = "vert_PerVertexLighting_NoLight_NoTextureNoNormalMap";
                                currentFragmentFunction = "frag_PerVertexLighting_NoTextureMap";
                            }
                            break;
                        }
                    case (1):
                        {
                            //print("Entramos en phong, Pixel, no normal map, no texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Phong, Pixel, no texture, no normal map
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Phong_Variables;
                                //temporalSpellBook += SpellBookFunctions.Phong_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoTextureNoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Phong_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Phong_NoTextureNoNormalMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoTetureNoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_Phong_NoTextureNoNormalMap";
                            }
                            else //Phong, Vertex, no texture, no normal map
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Phong_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.Phong_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoTextureNoNormalMap_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Phong_Lighting_Vertex_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Phong_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting_NoTextureMap;
                                currentVertexFunction = "vert_PerVertexLighting_Phong_NoNormalMap";
                                currentFragmentFunction = "frag_PerVertexLighting_NoTextureMap";
                            }
                            break;
                        }
                    case (2):
                        {
                            //print("Entramos en lambert, Pixel, no normal map, no texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Lambert, Pixel, no texture, no normal
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoTextureNoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Lambert_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Lambert_NoTextureNoNormalMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoTextureNoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_Lambert_NoTextureNoNormalMap";
                            }
                            else //Lambert, Vertex
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoTextureNoNormalMap_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Lambert_Lighting_Vertex_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Lambert_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting_NoTextureMap;
                                currentVertexFunction = "vert_PerVertexLighting_Lambert_NoNormalMap";
                                currentFragmentFunction = "frag_PerVertexLighting_NoTextureMap";
                            }
                            break;
                        }
                    case (3):
                        {
                            //print("Entramos en HalfLambert, Pixel, no normal map, no texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //HalfLambert, Pixel, no texture, no normal map
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoTextureNoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.HalfLambert_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_HalfLambert_NoTextureMapNoNormalMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoTextureNoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_HalfLambert_NoTextureMapNoNormalMap";
                            }
                            else //HalfLambert, Vertex, no texture, no normal map
                            {
                                temporalSpellBook += Lambert_Variables;
                                temporalSpellBook += NoTextureMap_Variables; 
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoTextureNoNormalMap_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.HalfLambert_Lighting_Vertex_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_HalfLambert_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting_NoTextureMap;
                                currentVertexFunction = "vert_PerVertexLighting_HalfLambert_NoNormalMap";
                                currentFragmentFunction = "frag_PerVertexLighting_NoTextureMap";
                            }
                            break;
                        }
                }
            }
            else //we've got normal map, but not texture!
            {
                switch (ShaderEdition.currentInstance._Current_Lighting_Model)
                {
                    case (0):
                        {
                            //print("Entramos en No Light, Pixel, normal map, no texture?");
                            //If you do not receive light, how could you use normal maps?
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //No Light, Pixel, no texture, normal map
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoLight_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoTextureNoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_NoLight_NoTextureMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoTextureNoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_NoLight_NoTextureMap";
                            }
                            else //No Light, Vertex, no texture. no normal map
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoLight_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoLight_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_NoLight_NoTextureNoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting_NoTextureMap;
                                currentVertexFunction = "vert_PerVertexLighting_NoLight_NoTextureNoNormalMap";
                                currentFragmentFunction = "frag_PerVertexLighting_NoTextureMap";
                            }
                            break;
                        }
                    case (1):
                        {
                            //print("Entramos en Phong, Pixel, normal map, no texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Phong, Pixel, no texture, normal map
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Phong_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Phong_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.Phong_Lighting_Pixel;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Phong_NoTexture;
                                currentVertexFunction = "vert_PerPixelLighting";
                                currentFragmentFunction = "frag_PerPixelLighting_Phong_NoTexture";
                            }
                            else //Phong, Vertex, no texture, normal map
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Phong_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.Phong_Variables;
                                temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.Phong_Lighting_Vertex;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Phong;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting_NoTextureMap;
                                currentVertexFunction = "vert_PerVertexLighting_Phong";
                                currentFragmentFunction = "frag_PerVertexLighting_NoTextureMap";

                            }
                            break;
                        }
                    case (2):
                        {
                            //print("Entramos en Lambert, Pixel, normal map, no texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Lambert, Pixel
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Lambert_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.Lambert_Lighting_Pixel;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Lambert_NoTextureMap;
                                currentVertexFunction = "vert_PerPixelLighting";
                                currentFragmentFunction = "frag_PerPixelLighting_Lambert_NoTextureMap";
                            }
                            else //Lambert, Vertex
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Lambert_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Lambert;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting_NoTextureMap;
                                currentVertexFunction = "vert_PerVertexLighting_Lambert";
                                currentFragmentFunction = "frag_PerVertexLighting_NoTextureMap";
                            }
                            break;
                        }
                    case (3):
                        {
                            //print("Entramos en Half Lambert, Pixel, normal map, no texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //HalfLambert, Pixel
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Lambert_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.HalfLambert_Lighting_Pixel;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_HalfLambert_NoTextureMap;
                                currentVertexFunction = "vert_PerPixelLighting";
                                currentFragmentFunction = "frag_PerPixelLighting_HalfLambert_NoTextureMap";
                            }
                            else //HalfLambert, Vertex
                            {
                                temporalSpellBook += NoTextureMap_Variables;
                                temporalSpellBook += Lambert_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.NoTextureMap_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.HalfLambert_Lighting_Vertex;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_HalfLambert;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting_NoTextureMap;
                                currentVertexFunction = "vert_PerVertexLighting_HalfLambert";
                                currentFragmentFunction = "frag_PerVertexLighting_NoTextureMap";
                            }
                            break;
                        }
                }
            }
        }
        //This means we got a different texture
        else
        {
            //We ain't got a normal map, but we got a texture
            if (ShaderEdition.currentInstance.IsNormalMapApplied == false)
            {
                switch (ShaderEdition.currentInstance._Current_Lighting_Model)
                {
                    case (0):
                        {
                            //print("Entramos en No Light, Pixel, no normal map, texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //No Light, Pixel, texture, no normal map
                            {
                                temporalSpellBook += texture_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_NoLight_NoNormalMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_NoLight_NoNormalMap";
                            }
                            else //No Light, Vertex, texture, no normal map
                            {
                                temporalSpellBook += texture_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_NoLight;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting;
                                currentVertexFunction = "vert_PerVertexLighting_NoLight";
                                currentFragmentFunction = "frag_PerVertexLighting";
                            }
                            break;
                        }
                    case (1):
                        {
                            //print("Entramos en Phong, Pixel, no normal map, texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Phong, Pixel, texture, no normal map
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Phong_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Phong_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.Phong_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Phong_NoNormalMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_Phong_NoNormalMap";
                            }
                            else //Phong, Vertex, texture, no normal map
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Phong_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Phong_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.Phong_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Phong_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting;
                                currentVertexFunction = "vert_PerVertexLighting_Phong_NoNormalMap";
                                currentFragmentFunction = "frag_PerVertexLighting";
                            }
                            break;
                        }
                    case (2):
                        {
                            //print("Entramos en Lambert, Pixel, no normal map, texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Lambert, Pixel
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.Lambert_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Lambert_NoNormalMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_Lambert_NoNormalMap";
                            }
                            else //Lambert, Vertex
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.Lambert_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Lambert_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting;
                                currentVertexFunction = "vert_PerVertexLighting_Lambert_NoNormalMap";
                                currentFragmentFunction = "frag_PerVertexLighting";

                            }
                            break;
                        }
                    case (3):
                        {
                            //print("Entramos en HalfLambert, Pixel, no normal map, texture?");
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //HalfLambert, Pixel
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_NoNormalMap_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.HalfLambert_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_HalfLambert_NoNormalMap;
                                currentVertexFunction = "vert_PerPixelLighting_NoNormalMap";
                                currentFragmentFunction = "frag_PerPixelLighting_HalfLambert_NoNormalMap";
                            }
                            else //HalfLambert, Vertex
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.HalfLambert_Lighting_Pixel_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_HalfLambert_NoNormalMap;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting;
                                currentVertexFunction = "vert_PerVertexLighting_HalfLambert_NoNormalMap";
                                currentFragmentFunction = "frag_PerVertexLighting";

                            }
                            break;
                        }
                }
            }
            else // full combo, full variables
            {
                switch (ShaderEdition.currentInstance._Current_Lighting_Model)
                {
                    case (0):
                        {
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //No Light, Pixel
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_NoLight;
                                currentVertexFunction = "vert_PerPixelLighting";
                                currentFragmentFunction = "frag_PerPixelLighting_NoLight";
                            }
                            else //No Light, Vertex
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_NoLight;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting;
                                currentVertexFunction = "vert_PerVertexLighting_NoLight";
                                currentFragmentFunction = "frag_PerVertexLighting";

                            }
                            break;
                        }
                    case (1):
                        {
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Phong, Pixel
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Phong_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Phong_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.Phong_Lighting_Pixel;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Phong;
                                currentVertexFunction = "vert_PerPixelLighting";
                                currentFragmentFunction = "frag_PerPixelLighting_Phong";
                            }
                            else //Phong, Vertex
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                temporalSpellBook += Phong_Variables;
                                //temporalSpellBook += SpellBookFunctions.Phong_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.Phong_Lighting_Vertex;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Phong;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting;
                                currentVertexFunction = "vert_PerVertexLighting_Phong";
                                currentFragmentFunction = "frag_PerVertexLighting";

                            }
                            break;
                        }
                    case (2):
                        {
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //Lambert, Pixel
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.Lambert_Lighting_Pixel;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Lambert;
                                currentVertexFunction = "vert_PerPixelLighting";
                                currentFragmentFunction = "frag_PerPixelLighting_Lambert";
                            }
                            else //Lambert, Vertex
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.Lambert_Lighting_Vertex;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_Lambert;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting;
                                currentVertexFunction = "vert_PerVertexLighting_Lambert";
                                currentFragmentFunction = "frag_PerVertexLighting";

                            }
                            break;
                        }
                    case (3):
                        {
                            if (ShaderEdition.currentInstance._Is_Pixel_Lighting == 1) //HalfLambert, Pixel
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Lambert_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.HalfLambert_Lighting_Pixel;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Pixel;
                                temporalSpellBook += SpellBookFunctions.vert_PerPixelLighting;
                                temporalSpellBook += SpellBookFunctions.frag_PerPixelLighting_Lambert;
                                currentVertexFunction = "vert_PerPixelLighting";
                                currentFragmentFunction = "frag_PerPixelLighting_HalfLambert";
                            }
                            else //HalfLambert, Vertex
                            {
                                temporalSpellBook += texture_Variables;
                                temporalSpellBook += Normal_Handling_Variables;
                                temporalSpellBook += Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Lambert_Variables;
                                //temporalSpellBook += SpellBookFunctions.Texture_Handling_Variables;
                                //temporalSpellBook += SpellBookFunctions.Normal_Handling_Variables;
                                temporalSpellBook += SpellBookFunctions.vertexInput_AllVariables;
                                temporalSpellBook += SpellBookFunctions.vertexOutput_PerVertexLighting;
                                temporalSpellBook += SpellBookFunctions.Texture_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.HalfLambert_Lighting_Pixel;
                                temporalSpellBook += SpellBookFunctions.Normal_Direction_With_Normal_Map_Handling_Vertex;
                                temporalSpellBook += SpellBookFunctions.vert_PerVertexLighting_HalfLambert;
                                temporalSpellBook += SpellBookFunctions.frag_PerVertexLighting;
                                currentVertexFunction = "vert_PerVertexLighting_HalfLambert";
                                currentFragmentFunction = "frag_PerVertexLighting";
                            }
                            break;
                        }
                }
            }
        }       
    }

    public void StoreDesiredVariables()
    {
        texture_Variables = " uniform sampler2D _CustomTexture; " +
        " uniform fixed4 _TextureTint =  " + FromColorToShaderColor(ShaderEdition.currentInstance._TextureTint) + " ; " +
        " uniform float _TextureTileX = " + ShaderEdition.currentInstance._Base_Texture_Scale_X + " ; " +
        " uniform float _TextureTileY = " + ShaderEdition.currentInstance._Base_Texture_Scale_Y + " ; " +
        " uniform float _OffsetTileX; = " + ShaderEdition.currentInstance._Base_Texture_Offset_X + " ; " +
        " uniform float _OffsetTileY = " + ShaderEdition.currentInstance._Base_Texture_Offset_Y + " ; "+ '\n';

        Normal_Handling_Variables =
        " uniform sampler2D _NormalMap; " +
        " uniform float _NormalTileX = " + ShaderEdition.currentInstance.Normal_Map_Scale_X + " ; " +
        " uniform float _NormalTileY = " + ShaderEdition.currentInstance.Normal_Map_Scale_Y + " ; " +
        " uniform float _NormalOffsetX = " + ShaderEdition.currentInstance.Normal_Map_Offset_X + " ; " +
        " uniform float _NormalOffsetY = " + ShaderEdition.currentInstance.Normal_Map_Offset_X + " ; " +
        " uniform half _NormalMapScale  = " + ShaderEdition.currentInstance._CustomNormalMapScale + " ; " + '\n'
        ;

        Phong_Variables =
        " uniform float _CustomShininess = " + ShaderEdition.currentInstance._CustomShininess + " ; " +
        " uniform float4 _PhongAmbientColor =  " + FromColorToShaderColor(ShaderEdition.currentInstance._PhongAmbientColor) + " ; " +
        " uniform float _PhongAmbientForce = " + ShaderEdition.currentInstance._PhongAmbientForce + " ; " +
        " uniform float4 _PhongSpecularColor =  " + FromColorToShaderColor(ShaderEdition.currentInstance._PhongSpecularColor) + " ; " +
        " uniform float _PhongSpecularForce = " + ShaderEdition.currentInstance._PhongSpecularForce + " ; " +
        " uniform float4 _PhongDiffuseColor =  " + FromColorToShaderColor(ShaderEdition.currentInstance._PhongDiffuseColor) + " ; " +
        " uniform float _PhongDiffuseForce = " + ShaderEdition.currentInstance._PhongDiffuseForce + " ; " + '\n'
        ;

        Lambert_Variables =

        " uniform float _LambertTintForce = " + ShaderEdition.currentInstance._LambertTintForce + " ; " +
        " uniform float4 _LambertTintColor;  = " + FromColorToShaderColor(ShaderEdition.currentInstance._LambertTintColor) + " ; " + '\n';

        NoTextureMap_Variables =
        " uniform fixed4 _TextureTint = " + FromColorToShaderColor(ShaderEdition.currentInstance._TextureTint) + " ; " + '\n';
        
    }

    public string FromColorToShaderColor(Color input)
    {
        string resultBase = "float4(";
        resultBase += input.r + " , " + input.g + " , " + input.b + " , " + input.a + " )";
        return resultBase;
    }


    static string shaderTextStart =       
        " { " + '\n' +
            " SubShader " + '\n' +
            " { " + '\n' +
                "  Blend SrcAlpha OneMinusSrcAlpha  " + '\n' +
                " Pass " + '\n' +
                "{ " + '\n' +
                    " Tags { " + '"' + "LightMode" + '"' + " = " + '"' + LightModeType + '"' + " } " + "  " + '\n' +
                    " LOD 100 " + '\n' +
                    "CGPROGRAM" + '\n' +
                    " #pragma fullforwardshadows " + '\n' ;

    static string shaderTextEnd =
          " ENDCG " + "  " + '\n' +
                " } " +

            " } " +
        " } ";


}