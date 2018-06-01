using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyBoxPropertiesSetter : MonoBehaviour {

	#region CUSTOM_PROCEDURAL_SKYBOX_VARIABLES
	[HideInInspector]
	public float _AtmosphereThickness;
	[HideInInspector]
	public Color _SkyTint = Color.black;
	[HideInInspector]
	public Color _GroundColor = Color.black;
	[HideInInspector]
	public Color _LightColor = Color.black;

	[HideInInspector]
	public float _Exposure;

	#endregion

	#region CANVAS_REFERENCES
	 [Header("Canvas and Scene References")]
	public Slider Exposure_Slider_CR;

	public Slider Atmosphere_Thickness_Slider_CR;
	public Button Sky_Tint_Button_CR;
	public Button Ground_Color_Button_CR;

	public Light Scene_Light_CR;

    public RawImage Sky_Tint_Dummy_Image_CR;
    public RawImage Ground_Color_Dummy_Image_CR;
    public RawImage Scene_Light_Dummy_Image_CR;


	#endregion


	// Use this for initialization
	void Start () 
	{
		Exposure_Slider_CR.value = 1.16f;
		Atmosphere_Thickness_Slider_CR.value = 0.6f;
	}
	
	// Update is called once per frame
	void Update () 
	{
        UpdateSkyboxVariables();
        UpdateLightVariables();
	}

	public void UpdateSkyboxVariables()
	{
		_Exposure = Exposure_Slider_CR.value;
		_AtmosphereThickness = Atmosphere_Thickness_Slider_CR.value;

        Ground_Color_Dummy_Image_CR.color = _GroundColor;
        Sky_Tint_Dummy_Image_CR.color = _SkyTint;

		Shader.SetGlobalColor("_SkyTint", _SkyTint);
		Shader.SetGlobalFloat("_AtmosphereThickness", _AtmosphereThickness);
		Shader.SetGlobalFloat("_Exposure", _Exposure);
		Shader.SetGlobalColor("_GroundColor", _GroundColor);
	}

	public void UpdateLightVariables()
	{
        Scene_Light_CR.color = _LightColor;
        Scene_Light_Dummy_Image_CR.color = _LightColor;
    }

    public void SetInitialSceneProperties()
    {
        Exposure_Slider_CR.value = 1.16f;
        Atmosphere_Thickness_Slider_CR.value = 0.6f;
        _SkyTint = Color.black;
        _GroundColor = Color.black;
        _LightColor = Color.white;

    }

}
