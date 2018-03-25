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

    #region AUXILIAR_MAPS
    Texture2D _CustomNormalMap;
    float _CustomNormalMapScale;

    Texture2D _CustomBumpMap;
    float _CustomBumpMapScale;

    #endregion


    #endregion

    #region SHADER_SAVE
    [HideInInspector]
    public string FilePath;
    [HideInInspector]
    public string saveFilePath;
    [HideInInspector]
    public string shaderName;
    #endregion

    #region CHANGE_MESH_VARIABLES
    [HideInInspector]
    public Camera ActiveCamera;
    public Mesh[] availableMeshes;
    public Dropdown optionDropDown_CR;
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
        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
        Shader.SetGlobalFloat("_NormalMapScale", _CustomNormalMapScale );
	}


	// Use this for initialization
	void Start ()
    {
 
    }
	
	// Update is called once per frame
	void Update () 
	{

        #region UPDATE_GLOBAL_SHADER_VARIABLES
        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
        Shader.SetGlobalColor("_TextureTint", _TextureTint);
        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
        Shader.SetGlobalTexture("_BumpMap", _CustomBumpMap);
        #endregion
    }

    #region OBTAIN_IMAGE_FUNCTIONS

    public void GetImageFromFile(string imageType)
    {
        FilePath = FileBrowser.OpenSingleFile("Choose a PNG...", Application.dataPath, "png");
        print(FilePath);
        StartCoroutine(GetTextureFromPNG(imageType));
        
    }

    IEnumerator GetTextureFromPNG(string imageType)
    {
        WWW webObject = new WWW("file:///"+FilePath);
        while (!webObject.isDone)
        {
            yield return null;
        }
        switch (imageType)
        {
            case "DiffuseTexture":
                {
                    _CustomTexture = webObject.texture;
                    Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
                    break;
                }
            case "NormalMap":
                {
                    _CustomNormalMap = webObject.texture;
                    Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
                    break;
                }
            case "BumpMap":
                {
                    _CustomBumpMap = webObject.texture;
                    Shader.SetGlobalTexture("_BumpMap", _CustomBumpMap);
                    break;
                }
            default:
                {
                    print("You should not be here, revisa string de llamada a función");
                    break;
                }

        }
       
        
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
                    float value = float.Parse(reference.GetComponent<InputField>().text);
                    print("Value Obtained: " + value);
                    if (value >= 0f && value <= 1f) { _RTextureTint = value; }
                    break;
                }
            case ("GTextureBase"):
                {
                    float value = float.Parse(reference.GetComponent<InputField>().text);
                    print("Value Obtained: " + value);
                    if (value >= 0f && value <= 1f) { _GTextureTint = value; }
                    break;
                }
            case ("BTextureBase"):
                {
                    float value = float.Parse(reference.GetComponent<InputField>().text);
                    print("Value Obtained: " + value);
                    if (value >= 0f && value <= 1f) { _BTextureTint = value; }
                    break;
                }
            case ("ATextureBase"):
                {
                    float value = float.Parse(reference.GetComponent<InputField>().text);
                    print("Value Obtained: " + value);
                    if (value >= 0f && value <= 1f) { _ATextureTint = value; }
                    break;
                }
            default:
                {
                    print("YOU SHOULD NOT BE HERE");
                    break;
                }

        }
        _TextureTint = new Color(_RTextureTint, _GTextureTint, _BTextureTint, _ATextureTint);


    }




    #endregion

    #region CHANGE_MESH_FUNCTIONS
    public void ChangeMeshFromDropDown()
    {
        switch (optionDropDown_CR.captionText.text)
        {
            case "Sphere":
                {
                    if (availableMeshes[0] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[0];
                    }                    
                    break;
                }
            case "Cube":
                {
                    if (availableMeshes[1] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[1];
                    }
                    break;
                }
            case "Capsule":
                {
                    if (availableMeshes[2] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[2];
                    }
                    break;
                }
            case "Cylinder":
                {
                    if (availableMeshes[3] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[3];
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Selected option not valid. Revisa codigo y DropDown.");
                    break;
                }
        }



    }



    #endregion



}
