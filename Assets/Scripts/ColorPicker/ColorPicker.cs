using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour {

    public Image currentChosenColorCanvasReference;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentChosenColorCanvasReference.color = ShaderEdition.currentInstance._CustomColor;
	}





}
