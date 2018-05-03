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
  
    #region CHANGE_LIGHTING_MODEL
    
    [Header("Lighting Model References")]
    public Slider lighting_Model_Slider_CR;
    public Text lighting_Model_Text_CR;
    private int _Current_Lighting_Model;
    #endregion

    #region CHANGE_BASE_TEXTURE_PARAMETERS
    [Header("Base Texture Parameters")]
    public InputField Base_Texture_Scale_X_InputField_CR;
    public InputField Base_Texture_Scale_Y_inputField_CR;
    public Slider Base_Texture_Offset_X;
    public Slider Base_Texture_Offset_Y;
    public RawImage Dummy_Texture_Image_CR;
    public RawImage Dummy_Color_Texture_Image_CR;

    private float _Base_Texture_Scale_X = 1.0f;
    private float _Base_Texture_Scale_Y = 1.0f;
    private float _Base_Texture_Offset_X = 0.0f;
    private float _Base_Texture_Offset_Y = 0.0f;

    public Texture2D _CustomTexture;
    float _RTextureTint = 1.0f;
    float _GTextureTint = 1.0f;
    float _BTextureTint = 1.0f;
    float _ATextureTint = 1.0f;
    Color _TextureTint = Color.white;

    #endregion

    #region CHANGE_NORMAL_MAP_PARAMETERS
    [Header("Normal Map Parameters")]
    public InputField Normal_Map_Scale_X;
    public InputField Normal_Map_Scale_Y;
    public Slider Normal_Map_Offset_X;
    public Slider Normal_Map_Offset_Y;
    public RawImage Dummy_Normal_Map_Image_CR;

    private float _Normal_Map_Scale_X = 1.0f;
    private float _Normal_Map_Scale_Y = 1.0f;
    private float _Normal_Map_Offset_X = 0.0f;
    private float _Normal_Map_Offset_Y = 0.0f;

    Texture2D _CustomNormalMap;
    public InputField _NormalMapScale_InputField_CR;
    private float _CustomNormalMapScale = 1.0f;

    #endregion

    #region CHANGE_PHONG_LIGHT_MODEL_PARAMETERS
    [Header("Phong Light Model Parameters")]
    public Slider Shininess_Slider_CR;
    public InputField Min_Range_Shininess_InputField_CR;
    public InputField Max_Range_Shininess_InputField_CR;

    private float _CustomShininess;
    private float Min_Range_Shininess;
    private float Max_Range_Shininess;

    Color _PhongAmbientColor = Color.white;
    Color _PhongDiffuseColor = Color.white;
    Color _PhongSpecularColor = Color.white;

    public RawImage Dummy_Phong_Ambient_Color_Image_CR;
    public RawImage Dummy_Phong_Diffuse_Color_Image_CR;
    public RawImage Dummy_Phong_Specular_Color_Image_CR;

    public Slider Phong_Ambient_Force_Slider_CR;
    public Slider Phong_Specular_Force_Slider_CR;
    public Slider Phong_Diffuse_Force_Slider_CR;

    public Text Phong_Ambient_Force_Text_CR;
    public Text Phong_Diffuse_Force_Text_CR;
    public Text Phong_Specular_Force_Text_CR;

    float _PhongAmbientForce = 0.5f;
    float _PhongDiffuseForce = 0.5f;
    float _PhongSpecularForce = 0.5f;

    #endregion

    #region CANVAS_TOOLS_REFERENCES

    [Header("Canvas Tools References")]
    public ColorPicker colorPicker_CanvasReference_Script;

    public CanvasGroup ColorPicker_CanvasGroup_CR;

    public CanvasGroup NormalMapParameters_CanvasGroup_CR;
    public CanvasGroup BaseTextureParameters_CanvasGroup_CR;
    public CanvasGroup LambertLightingParameters_CanvasGroup_CR;
    public CanvasGroup PhongLightingParameters_CanvasGroup_CR;

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

    #endregion

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
       
        _CustomNormalMap = BaseNormalMap_Sprite.texture;

        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
        Shader.SetGlobalFloat("_NormalMapScale", _CustomNormalMapScale );
	}


	// Use this for initialization
	void Start ()
    {
        SetInitialPhongProperties();
        Base_Texture_Scale_X_InputField_CR.text = 1.0f + "";
        Base_Texture_Scale_Y_inputField_CR.text = 1.0f + "";
    }
	
	// Update is called once per frame
	void Update () 
	{
        UpdateScaleOffsetBaseTexture();
        UpdateScaleOffsetNormalMap();
        UpdateLightingModel();
        UpdateNormalMapScale();
        UpdatePhongForces();

        #region UPDATE_GLOBAL_SHADER_VARIABLES
        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
        Shader.SetGlobalColor("_TextureTint", _TextureTint);
        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
        Shader.SetGlobalFloat("_NormalMapScale", _CustomNormalMapScale);
        Shader.SetGlobalInt("_LightingModel", _Current_Lighting_Model);
        Shader.SetGlobalColor("_PhongAmbientColor", _PhongAmbientColor);
        Shader.SetGlobalColor("_PhongDiffuseColor", _PhongDiffuseColor);
        Shader.SetGlobalColor("_PhongSpecularColor", _PhongSpecularColor);
        Shader.SetGlobalFloat("_PhongAmbientForce", _PhongAmbientForce);
        Shader.SetGlobalFloat("_PhongDiffuseForce", _PhongDiffuseForce);
        Shader.SetGlobalFloat("_PhongSpecularForce", _PhongSpecularForce);
        Shader.SetGlobalFloat("_CustomShininess", _CustomShininess);

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
                        Dummy_Texture_Image_CR.texture = _CustomTexture;
                        break;
                    }
                case "NormalMap":
                    {
                        _CustomNormalMap = BaseNormalMap_Sprite.texture;
                        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
                        Dummy_Normal_Map_Image_CR.texture = _CustomNormalMap;
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
                        Dummy_Texture_Image_CR.texture = _CustomTexture;
                        break;
                    }
                case "NormalMap":
                    {
                        _CustomNormalMap = webObject.texture;
                        Shader.SetGlobalTexture("_NormalMap", _CustomNormalMap);
                        Dummy_Normal_Map_Image_CR.texture = _CustomNormalMap;
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

    public void CloseCanvasGroup(CanvasGroup CG)
    { CG.gameObject.SetActive(false);     }
    public void OpenCanvasGroup(CanvasGroup CG)
    { CG.gameObject.SetActive(true);      }

    //Opening a secondary menu means closing the other ones...
    public void OpenSecMenu ( CanvasGroup CG)
    {
        CloseCurrentSecMenu();
        CG.gameObject.SetActive(true);
    }



    public void CloseCurrentSecMenu()
    {
        if(NormalMapParameters_CanvasGroup_CR != null && NormalMapParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   NormalMapParameters_CanvasGroup_CR.gameObject.SetActive(false);        }
        if (BaseTextureParameters_CanvasGroup_CR != null && BaseTextureParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   BaseTextureParameters_CanvasGroup_CR.gameObject.SetActive(false);        }
        if (LambertLightingParameters_CanvasGroup_CR != null && LambertLightingParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   LambertLightingParameters_CanvasGroup_CR.gameObject.SetActive(false);     }
        if (PhongLightingParameters_CanvasGroup_CR != null && PhongLightingParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   PhongLightingParameters_CanvasGroup_CR.gameObject.SetActive(false);       }
        if (ColorPicker_CanvasGroup_CR != null && ColorPicker_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   ColorPicker_CanvasGroup_CR.gameObject.SetActive(false); }

    }

    //[This function is deprecated, as we do not use this kind of reference for now.]
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

    #region UPDATE_SCALE_OFFSET_TEXTURE_FUNCTIONS
    public void UpdateScaleOffsetBaseTexture()
    {
        if (Base_Texture_Scale_X_InputField_CR.text != "")
        { _Base_Texture_Scale_X = float.Parse(Base_Texture_Scale_X_InputField_CR.text); }
        if (Base_Texture_Scale_Y_inputField_CR.text != "")
        { _Base_Texture_Scale_Y = float.Parse(Base_Texture_Scale_Y_inputField_CR.text); }

        _Base_Texture_Offset_X = Base_Texture_Offset_X.value;
        _Base_Texture_Offset_Y = Base_Texture_Offset_Y.value;

        Shader.SetGlobalFloat("_TextureTileX", _Base_Texture_Scale_X);
        Shader.SetGlobalFloat("_TextureTileY", _Base_Texture_Scale_Y);
        Shader.SetGlobalFloat("_OffsetTileX", _Base_Texture_Offset_X);
        Shader.SetGlobalFloat("_OffsetTileY", _Base_Texture_Offset_Y);
    }

    public void UpdateScaleOffsetNormalMap()
    {
        if (Normal_Map_Scale_X.text != "")
        { _Normal_Map_Scale_X = float.Parse(Normal_Map_Scale_X.text); }
        if (Normal_Map_Scale_Y.text != "")
        { _Normal_Map_Scale_Y = float.Parse(Normal_Map_Scale_Y.text); }

        _Normal_Map_Offset_X = Normal_Map_Offset_X.value;
        _Normal_Map_Offset_Y = Normal_Map_Offset_Y.value;
        
        //FALTA EL INPUT POR PARTE DEL MENÚ, TRATA DE HACER ESO ESTA NOCHE.

        Shader.SetGlobalFloat("_NormalTileX", _Normal_Map_Scale_X);
        Shader.SetGlobalFloat("_NormalTileY", _Normal_Map_Scale_Y);
        Shader.SetGlobalFloat("_NormalOffsetX",  _Normal_Map_Offset_X);
        Shader.SetGlobalFloat("_NormalOffsetY", _Normal_Map_Offset_Y);
    }
    
    #endregion

    #region CHANGE_ACTIVE_COLOR_PICKER
    public void SetActiveChangeColor()
    {
            if (colorPicker_CanvasReference_Script.gameObject.activeInHierarchy == false)
            {
                print("HOW DID I GET HERE??!!!");
            }
            else
            {
            switch (colorPicker_CanvasReference_Script.ColorChangeID)
            {
                case "PhongAmbientColor":
                {
                        _PhongAmbientColor = colorPicker_CanvasReference_Script.CurrentColorSelected;
                        Dummy_Phong_Ambient_Color_Image_CR.color = _PhongAmbientColor;
                        Shader.SetGlobalColor("_PhongAmbientColor", _PhongAmbientColor);
                        break;
                }
                case "PhongDiffuseColor":
                    {
                        _PhongDiffuseColor = colorPicker_CanvasReference_Script.CurrentColorSelected;
                        Dummy_Phong_Diffuse_Color_Image_CR.color = _PhongDiffuseColor;
                        Shader.SetGlobalColor("_PhongDiffuseColor", _PhongDiffuseColor);
                        break;
                    }
                case "PhongSpecularColor":
                    {
                        _PhongSpecularColor = colorPicker_CanvasReference_Script.CurrentColorSelected;
                        Dummy_Phong_Specular_Color_Image_CR.color = _PhongSpecularColor;
                        Shader.SetGlobalColor("_PhongSpecularColor", _PhongSpecularColor);
                        break;
                    }
                case "TextureTint":
                    {
                        _TextureTint = colorPicker_CanvasReference_Script.CurrentColorSelected;
                        Dummy_Color_Texture_Image_CR.color = _TextureTint;
                        Shader.SetGlobalColor("_TextureTint", _TextureTint);
                        break;
                    }
                default:
                    {
                        print("You should not be here!!!");
                        break;
                    }
            }
                //print("[TO DO] Color to change depending ID. We arrive here?");
                CloseCanvasGroup(ColorPicker_CanvasGroup_CR);
            }           
    }

    public void setColorPickerColorChangeID(string newID)
    {
        colorPicker_CanvasReference_Script.ColorChangeID = newID;
        OpenCanvasGroup(ColorPicker_CanvasGroup_CR);
    }

    public void ResetColorByID(string colorID)
    {  switch (colorID)
        {
            case "PhongAmbientColor":
                {
                    _PhongAmbientColor = Color.white;
                    break;
                }
            case "PhongDiffuseColor":
                {
                    _PhongDiffuseColor = Color.white;
                    break;
                }
            case "PhongSpecularColor":
                {
                    _PhongSpecularColor = Color.white;
                    break;
                }
            case "TextureTint":
                {
                    _TextureTint = Color.white;
                    break;
                }
            default:
                {
                    print("You should not be here!!!");
                    break;
                }





        }
    }


    #endregion

    #region CHANGE_CUSTOM_PROPERTIES_FUNCTIONS

    public void UpdateNormalMapScale()
    {
        if (_NormalMapScale_InputField_CR.text != "")
        { _CustomNormalMapScale = float.Parse(_NormalMapScale_InputField_CR.text); }
        Shader.SetGlobalFloat("_NormalMapScale", _CustomNormalMapScale);
    }

    public void UpdatePhongForces()
    {
        #region COLOR_FORCES
        _PhongAmbientForce = Phong_Ambient_Force_Slider_CR.value;
        _PhongDiffuseForce = Phong_Diffuse_Force_Slider_CR.value;
        _PhongSpecularForce = Phong_Specular_Force_Slider_CR.value;

        Phong_Ambient_Force_Text_CR.text = Mathf.Round(_PhongAmbientForce * 100f) / 100f + "";
        Phong_Diffuse_Force_Text_CR.text = Mathf.Round(_PhongDiffuseForce * 100f) / 100f + "";
        Phong_Specular_Force_Text_CR.text = Mathf.Round(_PhongSpecularForce * 100f) / 100f + "";

        Shader.SetGlobalFloat("_PhongAmbientForce", _PhongAmbientForce);
        Shader.SetGlobalFloat("_PhongDiffuseForce", _PhongDiffuseForce);
        Shader.SetGlobalFloat("_PhongSpecularForce", _PhongSpecularForce);
        #endregion

        #region SHININESS_SLIDER
        _CustomShininess = Shininess_Slider_CR.value;
        Max_Range_Shininess = float.Parse(Max_Range_Shininess_InputField_CR.text);
        Min_Range_Shininess = float.Parse(Min_Range_Shininess_InputField_CR.text);

        Shininess_Slider_CR.maxValue = Max_Range_Shininess;
        Shininess_Slider_CR.minValue = Min_Range_Shininess;
        Mathf.Clamp(Shininess_Slider_CR.value, Min_Range_Shininess, Max_Range_Shininess);

        Shader.SetGlobalFloat("_CustomShininess", _CustomShininess);
        #endregion
    }

    public void SetInitialPhongProperties()
    {
        Phong_Ambient_Force_Slider_CR.value = 0.5f;
        Phong_Diffuse_Force_Slider_CR.value = 0.5f;
        Phong_Specular_Force_Slider_CR.value = 0.5f;

        Min_Range_Shininess_InputField_CR.text = 0f + "";
        Max_Range_Shininess_InputField_CR.text = 1f + "";
        Shininess_Slider_CR.minValue = 0f;
        Shininess_Slider_CR.maxValue = 1f;
        Min_Range_Shininess = 0f;
        _CustomShininess = 0.25f;
        Max_Range_Shininess = 1f;
        Shininess_Slider_CR.value = _CustomShininess;
    }

    #endregion

}
