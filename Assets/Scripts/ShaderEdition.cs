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

    #region CHANGE_LIGHTING_MODEL
    
    [Header("Lighting Model References")]
    public Slider lighting_Model_Slider_CR;
    public Text lighting_Model_Text_CR;
    private int _Current_Lighting_Model;
    #endregion

    #region CHANGE_SCALE_OFFSET_BASE_TEXTURE
    public InputField Base_Texture_Scale_X;
    public InputField Base_Texture_Scale_Y;
    public InputField Base_Texture_Offset_X;
    public InputField Base_Texture_Offset_Y;

    private float _Base_Texture_Scale_X = 1.0f;
    private float _Base_Texture_Scale_Y = 1.0f;
    private float _Base_Texture_Offset_X = 0.0f;
    private float _Base_Texture_Offset_Y = 0.0f;

    #endregion

    [Header("Canvas Tools References")]
    public ColorPicker colorPicker_CanvasReference_Script;

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

    public Camera ActiveCamera;
    public Mesh[] availableMeshes;
    public Dropdown optionDropDown_CR;
    #endregion

    #region SHADER_INTERNAL_MANAGEMENT

    public Sprite BaseTexture_Sprite;
    public Sprite BaseNormalMap_Sprite;

    public Color ShaderManagement_Color = Color.white; 
    // r = textureHandling+
    // g = normalMapHandling
    // b = BumpMapHandling
    // a = LightingForce

    #endregion

    public static ShaderEdition currentInstance;

    private void Awake()
    {
        currentInstance = this;
    }

    private void OnPreRender()
	{
        //Shader.SetGlobalFloat ("_CustomSmoothness", smoothSlider.value);
        _CustomTexture = BaseTexture_Sprite.texture;
        _CustomBumpMap = BaseTexture_Sprite.texture;
        _CustomNormalMap = BaseNormalMap_Sprite.texture;

        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
        Shader.SetGlobalFloat("_NormalMapScale", _CustomNormalMapScale );
        Shader.SetGlobalColor("_ShaderManagement_Color", ShaderManagement_Color);
	}


	// Use this for initialization
	void Start ()
    {
 
    }
	
	// Update is called once per frame
	void Update () 
	{
        UpdateScaleOffsetBaseTexture();
        UpdateLightingModel();
        

        #region UPDATE_GLOBAL_SHADER_VARIABLES
        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
        Shader.SetGlobalColor("_TextureTint", _TextureTint);
        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
        Shader.SetGlobalTexture("_BumpMap", _CustomBumpMap);
        Shader.SetGlobalInt("_LightingModel", _Current_Lighting_Model);
        Shader.SetGlobalColor("_ShaderManagement_Color", ShaderManagement_Color);
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
        if (FilePath == "")
        {
            switch (imageType)
            {
                case "DiffuseTexture":
                    {
                        _CustomTexture = BaseTexture_Sprite.texture;
                        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
                        break;
                    }
                case "NormalMap":
                    {
                        _CustomNormalMap = BaseNormalMap_Sprite.texture;
                        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
                        break;
                    }
                case "BumpMap":
                    {
                        _CustomBumpMap = BaseTexture_Sprite.texture;
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
        else
        {
            WWW webObject = new WWW("file:///" + FilePath);
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
    }

    #endregion

    #region CANVAS_RELATED_FUNCTIONS

    public void OpenCloseSubCanvas(CanvasGroup subCanvas)
    {
        if (subCanvas.gameObject.activeInHierarchy)
        { subCanvas.gameObject.SetActive(false); }
        else
        {
            if (subCanvas.name == "ColorPicker")
            {
                SetActiveChangeColor("TextureColor");
            }

            subCanvas.gameObject.SetActive(true);  
        }

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

    #region CHANGE_LIGHTING_MODEL_FUNCTIONS
    public void UpdateLightingModel()
    {
        _Current_Lighting_Model = (int)lighting_Model_Slider_CR.value;
        switch (_Current_Lighting_Model)
        {
            case 0:
                {
                    lighting_Model_Text_CR.text = "No Lighting";
                    break;
                }
            case 1:
                {
                    lighting_Model_Text_CR.text = "Phong Lighting";
                    break;
                }
            case 2:
                {
                    lighting_Model_Text_CR.text = "Lambert Lighting";
                    break;
                }
            case 3:
                {
                    lighting_Model_Text_CR.text = "Half-Lambert Lighting";
                    break;
                }
            case 4:
                {
                    lighting_Model_Text_CR.text = "Phong (No Ambient Lighting)";
                    break;
                }
        }
    }
    #endregion

    #region UPDATE_SCALE_OFFSET_BASE_TEXTURE_FUNCTIONS
    public void UpdateScaleOffsetBaseTexture()
    {
        if (Base_Texture_Scale_X.text != "")
        { _Base_Texture_Scale_X = float.Parse(Base_Texture_Scale_X.text); }
        if (Base_Texture_Scale_Y.text != "")
        { _Base_Texture_Scale_Y = float.Parse(Base_Texture_Scale_Y.text); }
        if (Base_Texture_Offset_X.text != "")
        { _Base_Texture_Offset_X = float.Parse(Base_Texture_Offset_X.text); }
        if (Base_Texture_Offset_Y.text != "")
        { _Base_Texture_Offset_Y = float.Parse(Base_Texture_Offset_Y.text); }        

        Shader.SetGlobalFloat("_TextureTileX", _Base_Texture_Scale_X);
        Shader.SetGlobalFloat("_TextureTileY", _Base_Texture_Scale_Y);
        Shader.SetGlobalFloat("_OffsetTileX", _Base_Texture_Offset_X);
        Shader.SetGlobalFloat("_OffsetTileY", _Base_Texture_Offset_Y);
    }

    #endregion

    #region CHANGE_ACTIVE_COLOR_PICKER
    public void SetActiveChangeColor(string colorName)
    {
        if (colorName == "TextureColor")
        {
            if (colorPicker_CanvasReference_Script.gameObject.activeInHierarchy == false)
            {
                colorPicker_CanvasReference_Script.CurrentColorSelected_Image.color = _TextureTint;
            }
            else
            {
                _TextureTint = colorPicker_CanvasReference_Script.CurrentColorSelected;
                OpenCloseSubCanvas(colorPicker_CanvasReference_Script.gameObject.GetComponent<CanvasGroup>());
            }           
        }



    }




    #endregion

}
