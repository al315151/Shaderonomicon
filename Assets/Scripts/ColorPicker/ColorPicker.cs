using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour {

    [Header("ColorSelection Canvas References")]    
    public Slider ColorHorizontal_Slider;
    public Slider ColorVertical_Slider;

    public Image cursorColorSelected_Image;
    public RectTransform ColorSelection_Image_CanvasReference;

    public GameObject ColorSelection_TopLeft_Corner; // 0 , 1
    public GameObject ColorSelection_TopRight_Corner; // 1 , 1
    public GameObject ColorSelection_BottomLeft_Corner; // 0 , 0 
    public GameObject ColorSelection_BottomRight_Corner; // 1 , 0

    private float Width_ColorSelection_Image_Float;
    private float Height_ColorSelection_Image_Float;

    [Header("Update Colors and HUE Canvas References")]
    public Slider HUESlider;


    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckSliderColorStatus();
	}

    public void CheckSliderColorStatus()
    {
        Width_ColorSelection_Image_Float = ColorSelection_TopRight_Corner.transform.position.x - ColorSelection_TopLeft_Corner.transform.position.x;
        Height_ColorSelection_Image_Float = ColorSelection_TopLeft_Corner.transform.position.y - ColorSelection_BottomLeft_Corner.transform.position.y;

        float PosX = (ColorHorizontal_Slider.value * Width_ColorSelection_Image_Float) + ColorSelection_BottomLeft_Corner.transform.position.x;

        float PosY = (ColorVertical_Slider.value * Height_ColorSelection_Image_Float) + ColorSelection_BottomLeft_Corner.transform.position.y;

        print(PosX + " x , " + PosY + " y");
        cursorColorSelected_Image.gameObject.transform.position =
            new Vector3(PosX, PosY, -1f);
    }






}
