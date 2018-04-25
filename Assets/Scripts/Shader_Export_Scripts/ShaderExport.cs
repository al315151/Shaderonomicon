using System.Collections;
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

    public static string currentVertexFunction = "vert_PerPixelLighting";
    public static string currentFragmentFunction = "frag_PerPixelLighting_Lambert";

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
       " Shader" + '"' + "Custom/NewShaderExported" + '"' +
        " { " +
            " SubShader " +
            " { " +
        " Blend One OneMinusSrcAlpha" +
        " Pass " +
        "{ " +
            " Tags { " + '"' + "LightMode" + '"' + " = " + '"' + "ForwardBase" + '"' + " } " +
            " LOD 100 " +
			"CGPROGRAM" + 
            "  #pragma vertex " +  currentVertexFunction +
            "  #pragma fragment " + currentFragmentFunction +  

            "  # include " + '"' + "SpellBook.cginc" + '"' +			
        " ENDCG " +
		" } " +
	
            " } " +
        " } ";




 }