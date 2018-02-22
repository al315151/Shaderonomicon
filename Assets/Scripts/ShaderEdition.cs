using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class ShaderEdition : MonoBehaviour {

    public Scrollbar smoothSlider;
    public Scrollbar metallicSlider;

    public RectTransform PickColorCanvas;

    [HideInInspector]
    public Color backUpColor;

    #region COLOR
    public Color _CustomColor;
    public Color _HUESelected;

    [HideInInspector]
    public float _CustomR;
    [HideInInspector]
    public float _CustomG;
    [HideInInspector]
    public float _CustomB;
    [HideInInspector]
    public float _CustomA;
    #endregion

    public Shader modifiableShaderReference;
    TextAsset shaderTextAsset;
    string shaderCurrentText;
   
    public Texture2D _CustomTexture;

    #if UNITY_EDITOR
    ColorPickerEditorVersion chooseColor;
    FileExplorerEditorVersion openFile;
    #endif


    #region SHADER_SAVE
    [HideInInspector]
    public string FilePath;
    [HideInInspector]
    public string shaderName;
    #endregion

    #region COLOR_PICKER
    public Image newChosenColorCanvasReference;
    public Image previousChosenColorCanvasReference;
    #endregion



    public static ShaderEdition currentInstance;

    private void Awake()
    {
        currentInstance = this;
    }

    private void OnPreRender()
	{
		Shader.SetGlobalFloat ("_CustomSmoothness", smoothSlider.value);
		Shader.SetGlobalFloat ("_CustomMetallic", metallicSlider.value);
        
        Shader.SetGlobalColor("_CustomColor", _CustomColor);
        Shader.SetGlobalColor("_HUESelected", _HUESelected);
        
        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
	}


	// Use this for initialization
	void Start ()
    {
        
       
	}
	
	// Update is called once per frame
	void Update () 
	{
		Shader.SetGlobalFloat ("_CustomSmoothness", smoothSlider.value);
		Shader.SetGlobalFloat ("_CustomMetallic", metallicSlider.value);

        Shader.SetGlobalColor("_CustomColor", _CustomColor);
        Shader.SetGlobalColor("_HUESelected", _HUESelected);

        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);


        newChosenColorCanvasReference.color = ShaderEdition.currentInstance._CustomColor;
    }

    public void OpenPickColorCanvas()
    {
        #if UNITY_EDITOR
        // chooseColor = ScriptableObject.CreateInstance<ColorPickerEditorVersion>();
        // chooseColor.Show();
        #endif

        PickColorCanvas.gameObject.SetActive(true);


    }

    public void ClosePickColorCanvas()
    {
#if UNITY_EDITOR
        //chooseColor.Close();
#endif

        PickColorCanvas.gameObject.SetActive(false);

    }


   
   public void GetImageFromFile()
    {
        var extensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
                new ExtensionFilter("Sound Files", "mp3", "wav" ),
                new ExtensionFilter("All Files", "*" ),
            };
       FilePath = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false)[0];
        print(FilePath);
    }

    private void OnMouseDrag()
    {
       



    }


}
