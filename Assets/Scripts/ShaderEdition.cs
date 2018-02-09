using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShaderEdition : MonoBehaviour {

    public Scrollbar smoothSlider;
    public Scrollbar metallicSlider;

    public Canvas PickColorCanvas;

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


    public Texture2D _CustomTexture;

    #if UNITY_EDITOR
    ColorPickerEditorVersion chooseColor;
    FileExplorerEditorVersion openFile;
    #endif

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
        Shader.SetGlobalColor("_HUESelected", _CustomColor);
        
        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Shader.SetGlobalFloat ("_CustomSmoothness", smoothSlider.value);
		Shader.SetGlobalFloat ("_CustomMetallic", metallicSlider.value);

        Shader.SetGlobalColor("_CustomColor", _CustomColor);
        Shader.SetGlobalColor("_HUESelected", _CustomColor);

        Shader.SetGlobalTexture("_CustomTexture", _CustomTexture);
    }

    public void OpenPickColorCanvas()
    {
        #if UNITY_EDITOR
        chooseColor = ScriptableObject.CreateInstance<ColorPickerEditorVersion>();
        chooseColor.Show();
        #endif

    }

    public void ClosePickColorCanvas()
    {
        #if UNITY_EDITOR
        chooseColor.Close();
        #endif
    }

    public void OpenFileMenu()
    {
        #if UNITY_EDITOR
        openFile = ScriptableObject.CreateInstance<FileExplorerEditorVersion>();
        openFile.Show();
        #endif
    }

    public Color RGBtoHSVtoRGB(Color colorBase)
    {
        float auxH;
        float auxS;
        float auxV;
        Color.RGBToHSV(colorBase,out auxH, out auxS, out auxV);
        return colorBase;
    }




}
