using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderEdition : MonoBehaviour {

	public Scrollbar smoothSlider;
	public Scrollbar metallicSlider;
   
    public Canvas PickColorCanvas;

    public Color backUpColor;

    Color _CustomColor;


	private void OnPreRender()
	{
		Shader.SetGlobalFloat ("_CustomSmoothness", smoothSlider.value);
		Shader.SetGlobalFloat ("_CustomMetallic", metallicSlider.value);
        
        Shader.SetGlobalColor("_CustomColor", _CustomColor);
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
    }

    public void OpenPickColorCanvas()
    { }

    public void ClosePickColorCanvas()
    { }






}
