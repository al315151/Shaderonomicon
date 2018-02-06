using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderEdition : MonoBehaviour {

	public Scrollbar smoothSlider;
	public Scrollbar metallicSlider;





	private void OnPreRender()
	{
		Shader.SetGlobalFloat ("_CustomSmoothness", smoothSlider.value);
		Shader.SetGlobalFloat ("_CustomMetallic", metallicSlider.value);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Shader.SetGlobalFloat ("_CustomSmoothness", smoothSlider.value);
		Shader.SetGlobalFloat ("_CustomMetallic", metallicSlider.value);
	}
}
