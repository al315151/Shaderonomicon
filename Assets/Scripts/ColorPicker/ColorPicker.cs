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
    private Color CurrentColorSelected;

    [Header("Update Colors and HUE Canvas References")]
    public Slider HUESlider;
    private Color HUESelected;
    public Image NewColorSelected_Image;
    public Image CurrentColorSelected_Image;



    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateHUEColor();
        CheckSliderColorStatus();
        UpdateColorFromSliders();
	}

    public void CheckSliderColorStatus()
    {
        Width_ColorSelection_Image_Float = ColorSelection_TopRight_Corner.transform.position.x - ColorSelection_TopLeft_Corner.transform.position.x;
        Height_ColorSelection_Image_Float = ColorSelection_TopLeft_Corner.transform.position.y - ColorSelection_BottomLeft_Corner.transform.position.y;

        float PosX = (ColorHorizontal_Slider.value * Width_ColorSelection_Image_Float) + ColorSelection_BottomLeft_Corner.transform.position.x;

        float PosY = (ColorVertical_Slider.value * Height_ColorSelection_Image_Float) + ColorSelection_BottomLeft_Corner.transform.position.y;

        //print(PosX + " x , " + PosY + " y");
        cursorColorSelected_Image.gameObject.transform.position =
            new Vector3(PosX, PosY, -1f);
    }

    private List<float> RGBToHSV(Color RGBColor)
    {
        List<float> result = new List<float>();

        //Code translated from https://stackoverflow.com/questions/3018313/algorithm-to-convert-rgb-to-hsv-and-hsv-to-rgb-in-range-0-255-for-both
        float min, max, delta;
        float h, s, v;

        //Calculamos el menor de los tres componentes
        min = RGBColor.r < RGBColor.g ? RGBColor.r : RGBColor.g;
        min = min < RGBColor.b ? min : RGBColor.b;

        max = RGBColor.r > RGBColor.g ? RGBColor.r : RGBColor.g;
        max = max > RGBColor.b ? max : RGBColor.b;

        v = max;

        delta = max - min;

        if (delta < 0.00001)
        {
            s = 0;
            h = 0;

            result.Add(h); result.Add(s); result.Add(v);
            return result;
        }
        if (max > 0.0f)
        {   s = (delta / max);      }
        else
        {
            s = 0.0f;
            h = 0.0f;

            result.Add(h); result.Add(s); result.Add(v);
            return result;
        }
        if (RGBColor.r >= max)
        {   h = (RGBColor.g - RGBColor.b) / delta;  }
        else if (RGBColor.g >= max)
        { h = 2.0f + (RGBColor.b - RGBColor.r) / delta; }
        else
        { h = 4.0f + (RGBColor.r - RGBColor.g) / delta; }

        h *= 60.0f;

        if (h < 0.0f)
        {   h += 360.0f;   }

        result.Add(h); result.Add(s); result.Add(v);
        return result;

    }

    private Color HSVToRGB(List<float> HSVColor)
    {
        //Code translated from https://stackoverflow.com/questions/3018313/algorithm-to-convert-rgb-to-hsv-and-hsv-to-rgb-in-range-0-255-for-both
        double hh, p, q, t, ff;
        long i;
        Color result = Color.white; // para que deje compilar mas que anda

        if (HSVColor[1] < 0.0f)
        {
            result.r = HSVColor[0];
            result.g = HSVColor[0];
            result.b = HSVColor[0];

            return result;
        }
        hh = HSVColor[0];
        if (hh > 360.0f) { hh = 0f; }
        hh /= 60.0f;

        i = (long)hh;
        ff = hh - i;
        p = HSVColor[2] * (1.0 - HSVColor[1]);
        q = HSVColor[2] * (1.0 - (HSVColor[1] * ff));
        t = HSVColor[2] * (1.0 - (HSVColor[1] * (1.0 - ff)));

        switch (i)
        {
            case 0:
                {
                    result.r = HSVColor[2];
                    result.g = (float)t;
                    result.b = (float)p;
                    break;
                }
            case 1:
                {
                    result.r = (float)q;
                    result.g = HSVColor[2];
                    result.b = (float)p;
                    break;
                }
            case 2:
                {
                    result.r = (float)p;
                    result.g = HSVColor[2];
                    result.b = (float)t;
                    break;
                }
            case 3:
                {
                    result.r = (float)p;
                    result.g = (float)q;
                    result.b = HSVColor[2];
                    break;
                }
            case 4:
                {
                    result.r = (float)t;
                    result.g = (float)p;
                    result.b = HSVColor[2];
                    break;
                }
            default:
                {
                    result.r = HSVColor[2];
                    result.g = (float)p;
                    result.b = (float)q;
                    break;
                }           
        }
        return result;
    }

    private void UpdateColorFromSliders()
    {
        List<float> HSVColor = new List<float>();
        float Hue = HUESlider.value; HSVColor.Add(Hue * 360f);
        float Saturation = ColorHorizontal_Slider.value; HSVColor.Add(Saturation);
        float Value = ColorVertical_Slider.value; HSVColor.Add(Value);

        CurrentColorSelected = HSVToRGB(HSVColor);
        CurrentColorSelected_Image.color = CurrentColorSelected;

    }

    private void UpdateHUEColor()
    {
        List<float> HSVColor = new List<float>();
        HSVColor.Add(HUESlider.value * 360f);
        HSVColor.Add(1.0f);
        HSVColor.Add(1.0f);
        HUESelected = HSVToRGB(HSVColor);
        //print(HUESlider.value);
        Shader.SetGlobalColor("_HUESelected", HUESelected);
    }  


}
