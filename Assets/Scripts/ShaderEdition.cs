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
   
    

    #region OBJECTIVE_SHADER_MESH
    public GameObject displayObject;
    #endregion

    #region SHADER_PROPERTIES_REFERENCE

    #region BASE_TEXTURE
    public Texture2D _CustomTexture;
    float _RTextureTint = 1.0f;
    float _GTextureTint = 1.0f;
    float _BTextureTint = 1.0f;
    float _ATextureTint = 1.0f;
    Color _TextureTint = Color.white;
    #endregion




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
        _TextureTint = new Color(_RTextureTint, _GTextureTint, _BTextureTint, _ATextureTint);


    }
	
	// Update is called once per frame
	void Update () 
	{

        #region UPDATE_GLOBAL_SHADER_VARIABLES
        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
        Shader.SetGlobalColor("_TextureTint", _TextureTint);
        #endregion
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

    #region CANVAS_RELATED_FUNCTIONS

    public void OpenCloseSubCanvas(CanvasGroup subCanvas)
    {
        if (subCanvas.gameObject.activeInHierarchy)
        { subCanvas.gameObject.SetActive(false); }
        else
        { subCanvas.gameObject.SetActive(true); }

    }

    public void ChangeValueInputField(GameObject reference)
    {
        switch (reference.name)
        {
            case ("RTextureBase"):
                {

                    break;
                }
            case ("GTextureBase"):
                {

                    break;
                }
            case ("BTextureBase"):
                {

                    break;
                }
            case ("ATextureBase"):
                {

                    break;
                }
            default:
                {
                    print("YOU SHOULD NOT BE HERE");
                    break;
                }

        }



    }




    #endregion







}
