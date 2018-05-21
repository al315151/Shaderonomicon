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

    #region OBJECTIVE_SHADER_MESH
    public GameObject displayObject;
    #endregion

    #region SHADER_PROPERTIES_REFERENCE
  
    #region CHANGE_LIGHTING_MODEL
    
    [Header("Lighting Model References")]
    public Slider lighting_Model_Slider_CR;
    public Text lighting_Model_Text_CR;
    [HideInInspector]
    public int _Current_Lighting_Model;
    [HideInInspector]
    public int _Is_Pixel_Lighting; // ARE WE USING PIXEL LIGHTING OR NOT?
    public Button Pixel_Lighting_Button_CR;
    public Button Vertex_Lighting_Button_CR;
    
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

    #region CHANGE_LAMBERT_LIGHT_MODEL_PARAMETERS
    [Header("Lambert Light Model Parameters")]
    public Toggle Automatic_Lambert_Light_Toogle_CR;    
    public RawImage Dummy_Lambert_Tint_Image_CR;
    public Slider Lambert_Tint_Force_Slider_CR;
    public Text Lambert_Tint_Text_CR;

    Color _LambertTintColor = Color.white;
    float _LambertTintForce = 0.5f;

    #endregion

    #region CANVAS_TOOLS_REFERENCES

    [Header("Canvas Tools References")]
    public ColorPicker colorPicker_CanvasReference_Script;

    public CanvasGroup ColorPicker_CanvasGroup_CR;

    public CanvasGroup NormalMapParameters_CanvasGroup_CR;
    public CanvasGroup BaseTextureParameters_CanvasGroup_CR;
    public CanvasGroup LambertLightingParameters_CanvasGroup_CR;
    public CanvasGroup PhongLightingParameters_CanvasGroup_CR;
    public Text SecMenu_Title_Text_CR;

    public InputField Change_Shader_Name_InputField_CR;

    [HideInInspector]
    public string ShaderName;

    #endregion

    #region SHADER_SAVE
    [HideInInspector]
    public string FilePath;
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

    #region TEMPORAL_SHADER_EXPORT_VARIABLES
    [HideInInspector]
    public bool IsNewTextureApplied = false;
    [HideInInspector]
    public bool IsNormalMapApplied = false;
    #endregion

    #region FINAL_SHADER_EXPORT_VARIABLES

    [HideInInspector]
    public bool shaderHasTexture;
    [HideInInspector]
    public bool shaderHasNormalMap;

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
        Shader.SetGlobalFloat("_LambertTintForce", _LambertTintForce);
        Shader.SetGlobalColor("_LambertTintColor", _LambertTintColor);
        Shader.SetGlobalInt("_IsPixelLighting", _Is_Pixel_Lighting);
        Shader.SetGlobalFloat("_TextureTileX", _Base_Texture_Scale_X);
        Shader.SetGlobalFloat("_TextureTileY", _Base_Texture_Scale_Y);
        Shader.SetGlobalFloat("_OffsetTileX", _Base_Texture_Offset_X);
        Shader.SetGlobalFloat("_OffsetTileY", _Base_Texture_Offset_Y);
        Shader.SetGlobalFloat("_NormalTileX", _Normal_Map_Scale_X);
        Shader.SetGlobalFloat("_NormalTileY", _Normal_Map_Scale_Y);
        Shader.SetGlobalFloat("_NormalOffsetX", _Normal_Map_Offset_X);
        Shader.SetGlobalFloat("_NormalOffsetY", _Normal_Map_Offset_Y);
        #endregion
    }


    // Use this for initialization
    void Start ()
    {
        SetInitialPhongProperties();
        SetLambertSpecs();

        Base_Texture_Scale_X_InputField_CR.text = 1.0f + "";
        Base_Texture_Scale_Y_inputField_CR.text = 1.0f + "";
        Normal_Map_Scale_X.text = 1.0f + "";
        Normal_Map_Scale_Y.text = 1.0f + "";
        _NormalMapScale_InputField_CR.text = 1.0f + "";

        ChangeToPixelLighting();


        _Current_Lighting_Model = 4; //tHIS IS FOR INITIAL UPDATE ON THE TEXTS.
        UpdateLightingModel();
    }
	
	// Update is called once per frame
	void Update () 
	{
        UpdateShaderNameInputField();
        UpdateScaleOffsetBaseTexture();
        UpdateScaleOffsetNormalMap();
        UpdateLightingModel();
        UpdateNormalMapScale();
        UpdatePhongForces();
        UpdateLambertSpecs();

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
        Shader.SetGlobalFloat("_LambertTintForce", _LambertTintForce);
        Shader.SetGlobalColor("_LambertTintColor", _LambertTintColor);
        Shader.SetGlobalFloat("_TextureTileX", _Base_Texture_Scale_X);
        Shader.SetGlobalFloat("_TextureTileY", _Base_Texture_Scale_Y);
        Shader.SetGlobalFloat("_OffsetTileX", _Base_Texture_Offset_X);
        Shader.SetGlobalFloat("_OffsetTileY", _Base_Texture_Offset_Y);
        Shader.SetGlobalFloat("_NormalTileX", _Normal_Map_Scale_X);
        Shader.SetGlobalFloat("_NormalTileY", _Normal_Map_Scale_Y);
        Shader.SetGlobalFloat("_NormalOffsetX", _Normal_Map_Offset_X);
        Shader.SetGlobalFloat("_NormalOffsetY", _Normal_Map_Offset_Y);
        Shader.SetGlobalInt("_LightingModel", _Current_Lighting_Model);
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
                        IsNewTextureApplied = false;
                        _CustomTexture = BaseTexture_Sprite.texture;
                        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
                        Dummy_Texture_Image_CR.texture = _CustomTexture;
                        break;
                    }
                case "NormalMap":
                    {
                        IsNormalMapApplied = false;
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
                        IsNewTextureApplied = true;
                        _CustomTexture = webObject.texture;
                        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
                        Dummy_Texture_Image_CR.texture = _CustomTexture;
                        break;
                    }
                case "NormalMap":
                    {
                        IsNormalMapApplied = true;
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
        if (CG.name == BaseTextureParameters_CanvasGroup_CR.name)
        { SecMenu_Title_Text_CR.text = "Base Texture Parameters"; }
        else if (CG.name == NormalMapParameters_CanvasGroup_CR.name)
        { SecMenu_Title_Text_CR.text = "Normal Map Parameters"; }
        else if (CG.name == LambertLightingParameters_CanvasGroup_CR.name)
        { SecMenu_Title_Text_CR.text = "Lambert Lighting Model Parameters"; }
        else if (CG.name == PhongLightingParameters_CanvasGroup_CR.name)
        { SecMenu_Title_Text_CR.text = "Phong Lighting Model Parameters"; }
        else
        { Debug.Log("You should not be here at all m8"); }
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

    public void UpdateShaderNameInputField()
    {
        if (Change_Shader_Name_InputField_CR.text != "")
        { ShaderName = Change_Shader_Name_InputField_CR.text; }
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

        if ((int)lighting_Model_Slider_CR.value != _Current_Lighting_Model)
        {
            _Current_Lighting_Model = (int)lighting_Model_Slider_CR.value;
            switch (_Current_Lighting_Model)
            {
                case 0:
                    {
                        lighting_Model_Text_CR.text = "No Lighting";
                        CloseCurrentSecMenu();
                        break;
                    }
                case 1:
                    {
                        lighting_Model_Text_CR.text = "Phong Lighting";
                        OpenSecMenu(PhongLightingParameters_CanvasGroup_CR);
                        break;
                    }
                case 2:
                    {
                        lighting_Model_Text_CR.text = "Lambert Lighting";
                        OpenSecMenu(LambertLightingParameters_CanvasGroup_CR);
                        break;
                    }
                case 3:
                    {
                        lighting_Model_Text_CR.text = "Half-Lambert Lighting";
                        OpenSecMenu(LambertLightingParameters_CanvasGroup_CR);
                        break;
                    }
            }
        }

        Shader.SetGlobalInt("_LightingModel", _Current_Lighting_Model);

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
                case "LambertTint":
                    {
                        _LambertTintColor = colorPicker_CanvasReference_Script.CurrentColorSelected;
                        Dummy_Lambert_Tint_Image_CR.color = _LambertTintColor;
                        Shader.SetGlobalColor("_LambertTintColor", _LambertTintColor);
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
            case "LambertTint":
                {
                    _LambertTintColor = Color.white;
                    break;
                }
            default:
                {
                    print("You should not be here!!!");
                    break;
                }





        }
    }

    public void ResetTextureByID(string TextureID)
    {





    }

    #endregion

    #region CHANGE_CUSTOM_PROPERTIES_FUNCTIONS

    public void UpdateNormalMapScale()
    {
        if (_NormalMapScale_InputField_CR.text != "")
        { _CustomNormalMapScale = float.Parse(_NormalMapScale_InputField_CR.text); }
        Shader.SetGlobalFloat("_NormalMapScale", _CustomNormalMapScale);
    }

    public void UpdateLambertSpecs()
    {
        _LambertTintForce = Lambert_Tint_Force_Slider_CR.value;
        Lambert_Tint_Text_CR.text = Mathf.Round(_LambertTintForce * 100f) / 100f + "";
        Shader.SetGlobalFloat("_LambertTintForce", _LambertTintForce);
    }

    public void SetLambertSpecs()
    {
        _LambertTintForce = 1f;
        Lambert_Tint_Force_Slider_CR.value = _LambertTintForce;
        _LambertTintColor = Color.white;
        Dummy_Lambert_Tint_Image_CR.color = _LambertTintColor;
        Shader.SetGlobalColor("_LambertTintColor", _LambertTintColor);
        Shader.SetGlobalFloat("_LambertTintForce", _LambertTintForce);
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

    public void setInitialLightingModelProperties()
    {
        //We'll start with some sickass Phong Pixel Lighting
        _Is_Pixel_Lighting = 1;
        _Current_Lighting_Model = 1;
        lighting_Model_Slider_CR.value = 1.0f;
    }

    public void ChangeToPixelLighting()
    { _Is_Pixel_Lighting = 1;
        Navigation dummyNavigation = new Navigation();
        dummyNavigation.mode = Navigation.Mode.None;
        Pixel_Lighting_Button_CR.navigation = dummyNavigation;
     Shader.SetGlobalInt("_IsPixelLighting", _Is_Pixel_Lighting); }

    public void ChanteToVertexLighting()
    { _Is_Pixel_Lighting = 0;
        Navigation dummyNavigation = new Navigation();
        dummyNavigation.mode = Navigation.Mode.None;
        Vertex_Lighting_Button_CR.navigation = dummyNavigation;
        Shader.SetGlobalInt("_IsPixelLighting", _Is_Pixel_Lighting); }

    #endregion

}
