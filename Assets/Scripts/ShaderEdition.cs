using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Crosstales.FB;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class ShaderEdition : MonoBehaviour {

   
    public Shader modifiableShaderReference;
    TextAsset shaderTextAsset;
    string shaderCurrentText;
   
    public Texture2D _CustomTexture;

    #region OBJECTIVE_SHADER_MESH
    public GameObject displayObject;
    #endregion


    #region SHADER_SAVE
    [HideInInspector]
    public string FilePath;
    [HideInInspector]
    public string shaderName;
    #endregion

    public static ShaderEdition currentInstance;

    private void Awake()
    {
        currentInstance = this;
    }

    private void OnPreRender()
	{
		//Shader.SetGlobalFloat ("_CustomSmoothness", smoothSlider.value);
		
        
        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
	}


	// Use this for initialization
	void Start ()
    {
        
       
	}
	
	// Update is called once per frame
	void Update () 
	{
		

        //Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
    }

    #region OBTAIN_IMAGE_FUNCTIONS

    public void GetImageFromFile()
    {
        FilePath = FileBrowser.OpenSingleFile("Choose a PNG...", Application.dataPath, "png");
        print(FilePath);
        StartCoroutine(GetTextureFromPNG());
        
    }

    IEnumerator GetTextureFromPNG()
    {
        WWW webObject = new WWW("file:///"+FilePath);
        while (!webObject.isDone)
        {
            yield return null;
        }
        _CustomTexture = webObject.texture;
        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
    }

    #endregion











}
