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
   
    
    #endregion

    #region CHANGE_BASE_TEXTURE_PARAMETERS
    [Header("Base Texture Parameters")]
    public InputField Base_Texture_Scale_X_InputField_CR;
    public InputField Base_Texture_Scale_Y_inputField_CR;
    public Slider Base_Texture_Offset_X;
    public Slider Base_Texture_Offset_Y;
    public RawImage Dummy_Texture_Image_CR;
    public RawImage Dummy_Color_Texture_Image_CR;

    [HideInInspector]
    public float _Base_Texture_Scale_X = 1.0f;
    [HideInInspector]
    public float _Base_Texture_Scale_Y = 1.0f;
    [HideInInspector]
    public float _Base_Texture_Offset_X = 0.0f;
    [HideInInspector]
    public float _Base_Texture_Offset_Y = 0.0f;

    [HideInInspector]
    public Texture2D _CustomTexture;

    [HideInInspector]
    public Color _TextureTint = Color.white;

    #endregion

    #region CHANGE_NORMAL_MAP_PARAMETERS
    [Header("Normal Map Parameters")]
    public InputField Normal_Map_Scale_X;
    public InputField Normal_Map_Scale_Y;
    public Slider Normal_Map_Offset_X;
    public Slider Normal_Map_Offset_Y;
    public RawImage Dummy_Normal_Map_Image_CR;

    [HideInInspector]
    public float _Normal_Map_Scale_X = 1.0f;
    [HideInInspector]
    public float _Normal_Map_Scale_Y = 1.0f;
    [HideInInspector]
    public float _Normal_Map_Offset_X = 0.0f;
    [HideInInspector]
    public float _Normal_Map_Offset_Y = 0.0f;

    Texture2D _CustomNormalMap;
    public InputField _NormalMapScale_InputField_CR;
    [HideInInspector]
    public float _CustomNormalMapScale = 1.0f;

    #endregion

    #region CHANGE_PHONG_LIGHT_MODEL_PARAMETERS
    [Header("Phong Light Model Parameters")]
    public Slider Shininess_Slider_CR;
    public InputField Min_Range_Shininess_InputField_CR;
    public InputField Max_Range_Shininess_InputField_CR;

    [HideInInspector]
    public float _CustomShininess;
    private float Min_Range_Shininess;
    private float Max_Range_Shininess;

    [HideInInspector]
    public Color _PhongAmbientColor = Color.white;
    [HideInInspector]
    public Color _PhongDiffuseColor = Color.white;
    [HideInInspector]
    public Color _PhongSpecularColor = Color.white;

    public RawImage Dummy_Phong_Ambient_Color_Image_CR;
    public RawImage Dummy_Phong_Diffuse_Color_Image_CR;
    public RawImage Dummy_Phong_Specular_Color_Image_CR;

    public Slider Phong_Ambient_Force_Slider_CR;
    public Slider Phong_Specular_Force_Slider_CR;
    public Slider Phong_Diffuse_Force_Slider_CR;

    public Text Phong_Ambient_Force_Text_CR;
    public Text Phong_Diffuse_Force_Text_CR;
    public Text Phong_Specular_Force_Text_CR;

    [HideInInspector]
    public float _PhongAmbientForce = 0.5f;
    [HideInInspector]
    public float _PhongDiffuseForce = 0.5f;
    [HideInInspector]
    public float _PhongSpecularForce = 0.5f;

    #endregion

    #region CHANGE_LAMBERT_LIGHT_MODEL_PARAMETERS
    [Header("Lambert Light Model Parameters")]
    public Toggle Automatic_Lambert_Light_Toogle_CR;    
    public RawImage Dummy_Lambert_Tint_Image_CR;
    public Slider Lambert_Tint_Force_Slider_CR;
    public Text Lambert_Tint_Text_CR;

    [HideInInspector]
    public Color _LambertTintColor = Color.white;
    [HideInInspector]
    public float _LambertTintForce = 0.5f;

    #endregion

    #region CANVAS_RELATED_REFERENCES

    [Header("Canvas Tools References")]
    public ColorPicker colorPicker_CanvasReference_Script;

    public CanvasGroup ColorPicker_CanvasGroup_CR;

    public CanvasGroup NormalMapParameters_CanvasGroup_CR;
    public CanvasGroup BaseTextureParameters_CanvasGroup_CR;
    public CanvasGroup LambertLightingParameters_CanvasGroup_CR;
    public CanvasGroup PhongLightingParameters_CanvasGroup_CR;
    public CanvasGroup SceneSettingsParameters_CanvasGroup_CR;
    public CanvasGroup ExitMenu_CanvasGroup_CR;

    [Header("Canvas Buttons And Parameters References")]

    public Text SecMenu_Title_Text_CR;
    public InputField Change_Shader_Name_InputField_CR;

    public Button Pixel_Lighting_Button_CR;
    public Button Vertex_Lighting_Button_CR;

    public Button Sphere_Mesh_Button_CR;
    public Button Cube_Mesh_Button_CR;
    public Button Torus_Mesh_Button_CR;
    public Button Cylinder_Mesh_Button_CR;
    public Button LowPoly_Sphere_Mesh_Button_CR;

    public Button Base_Texture_Menu_Button_CR;
    public Button Normal_Map_Menu_Button_CR;
    public Button Open_Current_Lighting_Menu_Button_CR;

    public Material ActiveButton_Material;
   
    public Button ExitMenu_Button_CR;
    public Button OpenTextureMenu_Button_CR;
    public Button OpenNormalMapMenu_Button_CR;
    public Button OpenLightingModelMenu_Button_CR;

    Material previousSelected_Button_Material;
    string previousButton_String = "";

    [HideInInspector]
    public string ShaderName;

    public Slider Transparency_Slider_CR;
    [HideInInspector]
    public float _CustomAlpha = 1.0f;

    #endregion

    #region SHADER_SAVE
    [HideInInspector]
    public string FilePath;
    #endregion

    #region CHANGE_MESH_VARIABLES

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

        Transparency_Slider_CR.value = 1.0f;

        #region UPDATE_GLOBAL_SHADER_VARIABLES
        UpdateGlobalShaderStats();
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


        _Current_Lighting_Model = 0; //tHIS IS FOR INITIAL UPDATE ON THE TEXTS.
        UpdateLightingModel();

        SceneSettingsParameters_CanvasGroup_CR.GetComponent<SkyBoxPropertiesSetter>().SetInitialSceneProperties();
        CloseCurrentSecMenu();
        Change_Shader_Name_InputField_CR.text = "New Shader";
        //print(_TextureTint.ToString());
    }
	
	// Update is called once per frame
	void Update () 
	{
        print("Current lighting model: " + _Current_Lighting_Model);
        print("Is pixel lighting applied? " + _Is_Pixel_Lighting);

        UpdateManagerVariablesShader();
        UpdateShaderNameInputField();
        UpdateScaleOffsetBaseTexture();
        UpdateScaleOffsetNormalMap();
        UpdateLightingModel();
        UpdateNormalMapScale();
        UpdatePhongForces();
        UpdateLambertSpecs();

        #region UPDATE_GLOBAL_SHADER_VARIABLES
        UpdateGlobalShaderStats();
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
    {
        if (CG.name == ExitMenu_CanvasGroup_CR.name && ExitMenu_CanvasGroup_CR.gameObject.activeInHierarchy)
        { CG.gameObject.SetActive(false); }
        else { CG.gameObject.SetActive(true); }

        UpdateButtonMaterials(CG);
    }

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
        else if (CG.name == SceneSettingsParameters_CanvasGroup_CR.name)
        { SecMenu_Title_Text_CR.text = "Scene Settings Parameters"; }
        else
        { Debug.Log("You should not be here at all m8"); }
        CG.gameObject.SetActive(true);
        UpdateButtonMaterials(CG);
    }

    public void CloseCurrentSecMenu()
    {
        SecMenu_Title_Text_CR.text = "Secondary Menu Parameters";
        if (NormalMapParameters_CanvasGroup_CR != null && NormalMapParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   NormalMapParameters_CanvasGroup_CR.gameObject.SetActive(false);        }
        if (BaseTextureParameters_CanvasGroup_CR != null && BaseTextureParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   BaseTextureParameters_CanvasGroup_CR.gameObject.SetActive(false);        }
        if (LambertLightingParameters_CanvasGroup_CR != null && LambertLightingParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   LambertLightingParameters_CanvasGroup_CR.gameObject.SetActive(false);     }
        if (PhongLightingParameters_CanvasGroup_CR != null && PhongLightingParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   PhongLightingParameters_CanvasGroup_CR.gameObject.SetActive(false);       }
        if (ColorPicker_CanvasGroup_CR != null && ColorPicker_CanvasGroup_CR.gameObject.activeInHierarchy)
        {   ColorPicker_CanvasGroup_CR.gameObject.SetActive(false); }
        if (SceneSettingsParameters_CanvasGroup_CR != null && SceneSettingsParameters_CanvasGroup_CR.gameObject.activeInHierarchy)
        { SceneSettingsParameters_CanvasGroup_CR.gameObject.SetActive(false); } 

    }

    public void UpdateShaderNameInputField()
    {
        if (Change_Shader_Name_InputField_CR.text != "")
        { ShaderName = Change_Shader_Name_InputField_CR.text; }
    }

    public void OpenCurrentLightingModelMenu()
    {
        switch (_Current_Lighting_Model)
        {
            case 0:
                {
                    CloseCurrentSecMenu();
                    break;
                }
            case 1:
                {
                    OpenSecMenu(PhongLightingParameters_CanvasGroup_CR);
                    break;
                }
            case 2:
                {
                    OpenSecMenu(LambertLightingParameters_CanvasGroup_CR);
                    break;
                }
            case 3:
                {
                    OpenSecMenu(LambertLightingParameters_CanvasGroup_CR);
                    break;
                }
            default:
                {
                    Debug.Log("YOU SHOULD NOT BE HERE M8");
                    break;
                }
        }
        UpdateButtonMaterialsLightingMenu();
    }

    public void UpdateButtonMaterials(CanvasGroup CG)
    {
        if (CG.gameObject.activeInHierarchy) // queremos sustituir el material existente por otro, pero...
        {
            if (previousButton_String != "") //no habiamos seleccionado otro boton antes: perfecto.
            {
                RestorePreviousMaterialByControlString();
                previousButton_String = "";
            }
            if (CG.name == ExitMenu_CanvasGroup_CR.name)
            {
                previousSelected_Button_Material = ExitMenu_Button_CR.gameObject.GetComponent<Image>().material;
                previousButton_String = "Exit Menu";
                ExitMenu_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;
            }
            else if (CG.name == BaseTextureParameters_CanvasGroup_CR.name)
            {
                previousSelected_Button_Material = Base_Texture_Menu_Button_CR.gameObject.GetComponent<Image>().material;
                previousButton_String = "Base Texture";
                Base_Texture_Menu_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;
            }
            else if (CG.name == NormalMapParameters_CanvasGroup_CR.name)
            {
                previousSelected_Button_Material = Normal_Map_Menu_Button_CR.gameObject.GetComponent<Image>().material;
                previousButton_String = "Normal Map";
                Normal_Map_Menu_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;
            }
        }
        else // queremos reponer el anterior material.
        {
            RestorePreviousMaterialByControlString();
            previousButton_String = "";
        }       
    }

    void RestorePreviousMaterialByControlString()
    {
        if (previousButton_String == "Exit Menu")
        { ExitMenu_Button_CR.gameObject.GetComponent<Image>().material = previousSelected_Button_Material; }

        else if (previousButton_String == "Lighting Model")
        { OpenLightingModelMenu_Button_CR.gameObject.GetComponent<Image>().material = previousSelected_Button_Material; }

        else if (previousButton_String == "Base Texture")
        { Base_Texture_Menu_Button_CR.GetComponent<Image>().material = previousSelected_Button_Material; }

        else if (previousButton_String == "Normal Map")
        { Normal_Map_Menu_Button_CR.gameObject.GetComponent<Image>().material = previousSelected_Button_Material; }

        else if (previousButton_String == "Sphere")
        {
            Sphere_Mesh_Button_CR.gameObject.GetComponent<Image>().material = previousSelected_Button_Material;
            Sphere_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.black;
        }

        else if (previousButton_String == "LowPolySphere")
        {
            LowPoly_Sphere_Mesh_Button_CR.gameObject.GetComponent<Image>().material = previousSelected_Button_Material;
            LowPoly_Sphere_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.black;
        }

        else if (previousButton_String == "Cube")
        {
            Cube_Mesh_Button_CR.gameObject.GetComponent<Image>().material = previousSelected_Button_Material;
            Cube_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.black;
        }

        else if (previousButton_String == "Torus")
        {
            Torus_Mesh_Button_CR.gameObject.GetComponent<Image>().material = previousSelected_Button_Material;
            Torus_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.black;
        }

        else if (previousButton_String == "Cylinder")
        {
            Cylinder_Mesh_Button_CR.gameObject.GetComponent<Image>().material = previousSelected_Button_Material;
            Cylinder_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.black;
        }

        else
        { print("No deberias estar aqui, revisa las strings"); }
    }

    public void UpdateButtonMaterialsMeshDisplay(string meshID)
    {
        if (previousButton_String != "") //no habiamos seleccionado otro boton antes: perfecto.
        {
            RestorePreviousMaterialByControlString();
            previousButton_String = "";
        }
        switch(meshID)
        {
            case "Sphere":
                {
                    previousSelected_Button_Material = Sphere_Mesh_Button_CR.gameObject.GetComponent<Image>().material;
                    previousButton_String = "Sphere";
                    Sphere_Mesh_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;
                    Sphere_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.white;
                    break;
                }
            case "Cube":
                {
                    previousSelected_Button_Material = Cube_Mesh_Button_CR.gameObject.GetComponent<Image>().material;
                    previousButton_String = "Cube";
                    Cube_Mesh_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;
                    Cube_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.white;
                    break;
                }
            case "Torus":
                {
                    previousSelected_Button_Material = Torus_Mesh_Button_CR.gameObject.GetComponent<Image>().material;
                    previousButton_String = "Torus";
                    Torus_Mesh_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;
                    Torus_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.white;
                    break;
                }
            case "Cylinder":
                {
                    previousSelected_Button_Material = Cylinder_Mesh_Button_CR.gameObject.GetComponent<Image>().material;
                    previousButton_String = "Cylinder";
                    Cylinder_Mesh_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;
                    Cylinder_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.white;
                    break;
                }
            case "LowPolySphere":
                {
                    previousSelected_Button_Material = LowPoly_Sphere_Mesh_Button_CR.gameObject.GetComponent<Image>().material;
                    previousButton_String = "LowPolySphere";
                    LowPoly_Sphere_Mesh_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;
                    LowPoly_Sphere_Mesh_Button_CR.GetComponentInChildren<Text>().color = Color.white;
                    break;
                }
        }
    }

    public void UpdateButtonMaterialsLightingMenu()
    {
        if (previousButton_String != "") //no habiamos seleccionado otro boton antes: perfecto.
        {
            RestorePreviousMaterialByControlString();
            previousButton_String = "";
        }

        previousSelected_Button_Material = OpenLightingModelMenu_Button_CR.gameObject.GetComponent<Image>().material;
        previousButton_String = "Lighting Model";
        OpenLightingModelMenu_Button_CR.gameObject.GetComponent<Image>().material = ActiveButton_Material;

    }

    #endregion

    #region CHANGE_MESH_FUNCTIONS
    public void ChangeMeshFromDropDown()
    {
        switch (optionDropDown_CR.captionText.text)
        {
            case "HighPolySphere":
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
            case "Torus":
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
            case "LowPolySphere":
                {
                    if (availableMeshes[4] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[4];
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

    public void ChangeMeshFromButtons(string meshID)
    {
        switch (meshID)
        {
            case "Sphere":
                {
                    if (availableMeshes[0] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[0];
                        Sphere_Mesh_Button_CR.interactable = false;
                        Cube_Mesh_Button_CR.interactable = true;
                        Torus_Mesh_Button_CR.interactable = true;
                        Cylinder_Mesh_Button_CR.interactable = true;
                        LowPoly_Sphere_Mesh_Button_CR.interactable = true;
                    }
                    break;
                }
            case "Cube":
                {
                    if (availableMeshes[1] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[1];
                        Sphere_Mesh_Button_CR.interactable = true;
                        Cube_Mesh_Button_CR.interactable = false;
                        Torus_Mesh_Button_CR.interactable = true;
                        Cylinder_Mesh_Button_CR.interactable = true;
                        LowPoly_Sphere_Mesh_Button_CR.interactable = true;
                    }
                    break;
                }
            case "Torus":
                {
                    if (availableMeshes[2] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[2];
                        Sphere_Mesh_Button_CR.interactable = true;
                        Cube_Mesh_Button_CR.interactable = true;
                        Torus_Mesh_Button_CR.interactable = false;
                        Cylinder_Mesh_Button_CR.interactable = true;
                        LowPoly_Sphere_Mesh_Button_CR.interactable = true;
                    }
                    break;
                }
            case "Cylinder":
                {
                    if (availableMeshes[3] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[3];
                        Sphere_Mesh_Button_CR.interactable = true;
                        Cube_Mesh_Button_CR.interactable = true;
                        Torus_Mesh_Button_CR.interactable = true;
                        Cylinder_Mesh_Button_CR.interactable = false;
                        LowPoly_Sphere_Mesh_Button_CR.interactable = true;
                    }
                    break;
                }
            case "LowPolySphere":
                {
                    if (availableMeshes[4] != null)
                    {
                        displayObject.GetComponent<MeshFilter>().mesh = availableMeshes[4];
                        Sphere_Mesh_Button_CR.interactable = true;
                        Cube_Mesh_Button_CR.interactable = true;
                        Torus_Mesh_Button_CR.interactable = true;
                        Cylinder_Mesh_Button_CR.interactable = true;
                        LowPoly_Sphere_Mesh_Button_CR.interactable = false;
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Selected option not valid. Revisa codigo y DropDown.");
                    break;
                }
        }

        UpdateButtonMaterialsMeshDisplay(meshID);
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
                        SecMenu_Title_Text_CR.text = "No Lighting Model Selected";
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
        {
            float obtainedValue_X;
            if (float.TryParse(Base_Texture_Scale_X_InputField_CR.text, out obtainedValue_X))
            {   _Base_Texture_Scale_X = obtainedValue_X;            }           
        }
        if (Base_Texture_Scale_Y_inputField_CR.text != "")
        {
            float obtainedValue_Y;
            if (float.TryParse(Base_Texture_Scale_Y_inputField_CR.text, out obtainedValue_Y))
            { _Base_Texture_Scale_Y = obtainedValue_Y; }            
        }

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
        {
            float obtainedValue_X;
            if (float.TryParse(Normal_Map_Scale_X.text, out obtainedValue_X))
            { _Normal_Map_Scale_X = obtainedValue_X; }
        }
        if (Normal_Map_Scale_Y.text != "")
        {
            float obtainedValue_Y;
            if (float.TryParse(Normal_Map_Scale_Y.text, out obtainedValue_Y))
            { _Normal_Map_Scale_Y = obtainedValue_Y; }
        }

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
                case "SkyTint":
                    {
                        Color dummyColor = colorPicker_CanvasReference_Script.CurrentColorSelected;
                        SceneSettingsParameters_CanvasGroup_CR.GetComponent<SkyBoxPropertiesSetter>()._SkyTint = dummyColor;
                        break;
                    }
                case "GroundColor":
                    {
                        Color dummyColor = colorPicker_CanvasReference_Script.CurrentColorSelected;
                        SceneSettingsParameters_CanvasGroup_CR.GetComponent<SkyBoxPropertiesSetter>()._GroundColor = dummyColor;
                        break;
                    }
                case "LightColor":
                    {
                        Color dummyColor = colorPicker_CanvasReference_Script.CurrentColorSelected;
                        SceneSettingsParameters_CanvasGroup_CR.GetComponent<SkyBoxPropertiesSetter>()._LightColor = dummyColor;
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
                    Dummy_Phong_Ambient_Color_Image_CR.color = _PhongAmbientColor;
                    break;
                }
            case "PhongDiffuseColor":
                {
                    _PhongDiffuseColor = Color.white;
                    Dummy_Phong_Diffuse_Color_Image_CR.color = _PhongDiffuseColor;
                    break;
                }
            case "PhongSpecularColor":
                {
                    _PhongSpecularColor = Color.white;
                    Dummy_Phong_Specular_Color_Image_CR.color = _PhongSpecularColor;
                    break;
                }
            case "TextureTint":
                {
                    _TextureTint = Color.white;
                    Dummy_Color_Texture_Image_CR.color = _TextureTint;
                    break;
                }
            case "LambertTint":
                {
                    _LambertTintColor = Color.white;
                    Dummy_Lambert_Tint_Image_CR.color = _LambertTintColor;
                    break;
                }
            case "SkyTint":
                {
                    SceneSettingsParameters_CanvasGroup_CR.GetComponent<SkyBoxPropertiesSetter>()._SkyTint = Color.black;
                    break;
                }
            case "GroundColor":
                {
                    SceneSettingsParameters_CanvasGroup_CR.GetComponent<SkyBoxPropertiesSetter>()._GroundColor = Color.black;
                    break;
                }
            case "LightColor":
                {
                    SceneSettingsParameters_CanvasGroup_CR.GetComponent<SkyBoxPropertiesSetter>()._LightColor = Color.white;
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
        switch (TextureID)
        {
            case "NormalMap":
                {
                    _CustomNormalMap = BaseNormalMap_Sprite.texture;
                    Dummy_Normal_Map_Image_CR.texture = _CustomNormalMap;
                    IsNormalMapApplied = false;
                    break;
                }
            case "BaseTexture":
                {
                    _CustomTexture = BaseTexture_Sprite.texture;
                    Dummy_Texture_Image_CR.texture = _CustomTexture;
                    IsNewTextureApplied = false;
                    break;
                }
            default:
                {
                    Debug.Log("You should not be here m9");
                    break;
                }
        }
        UpdateGlobalShaderStats();
    }

    #endregion

    #region CHANGE_CUSTOM_PROPERTIES_FUNCTIONS

    public void UpdateNormalMapScale()
    {
        if (_NormalMapScale_InputField_CR.text != "")
        {
            float obtainedValue;
            if (float.TryParse(_NormalMapScale_InputField_CR.text, out obtainedValue))
            { _CustomNormalMapScale = obtainedValue; }
        }
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
        if (Max_Range_Shininess_InputField_CR.text != "")
        {
            float maxRange;
            if (float.TryParse(Max_Range_Shininess_InputField_CR.text, out maxRange))
            { Max_Range_Shininess = maxRange;  }
        }
        if (Min_Range_Shininess_InputField_CR.text != "")
        {
            float minRange;
            if (float.TryParse(Min_Range_Shininess_InputField_CR.text, out minRange))
            { Min_Range_Shininess = minRange; }
        }      

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
    {
        _Is_Pixel_Lighting = 1;
        Pixel_Lighting_Button_CR.interactable = false;
        Vertex_Lighting_Button_CR.interactable = true;
        Pixel_Lighting_Button_CR.GetComponentInChildren<Text>().color = Color.black;
        Vertex_Lighting_Button_CR.GetComponentInChildren<Text>().color = Color.white;       

        Shader.SetGlobalInt("_IsPixelLighting", _Is_Pixel_Lighting); }

    public void ChanteToVertexLighting()
    {
        _Is_Pixel_Lighting = 0;
        Vertex_Lighting_Button_CR.interactable = false;
        Pixel_Lighting_Button_CR.interactable = true;
        Pixel_Lighting_Button_CR.GetComponentInChildren<Text>().color = Color.white;
        Vertex_Lighting_Button_CR.GetComponentInChildren<Text>().color = Color.black;

        Shader.SetGlobalInt("_IsPixelLighting", _Is_Pixel_Lighting); }

    public void UpdateTransparency()
    {   _CustomAlpha = Transparency_Slider_CR.value;    }

    public void UpdateGlobalShaderStats()
    {
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
        Shader.SetGlobalFloat("_CustomAlpha", _CustomAlpha);
    }

    public void UpdateManagerVariablesShader()
    {
        if (IsNewTextureApplied)
        {  Shader.SetGlobalFloat("_IsTextureApplied", 1.0f);  }
        else {  Shader.SetGlobalFloat("_IsTextureApplied", 0.0f);      }
        if (IsNormalMapApplied)
        {   Shader.SetGlobalFloat("_IsNormalMapApplied", 1.0f);     }
        else { Shader.SetGlobalFloat("_IsNormalMapApplied", 0.0f);       }

        print("Normal map applied variable: " + IsNormalMapApplied);
        print("Texture applied variable: " + IsNewTextureApplied);
    }


    #endregion

    public void ExitApplication()
    { Application.Quit();  }



}
