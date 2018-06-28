using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderTextureProperties : MonoBehaviour {

    public RenderTexture TargetTexture_RenderTexture;
    public Camera CameraOfRenderTexture_Camera;
    public RawImage RenderTextureHolder_RawImage_CR;

    [Header("Render Texture Corners")]

    public GameObject RenderTexture_Corner_Up_Left_CR;
    public GameObject RenderTexture_Corner_Up_Right_CR;
    public GameObject RenderTexture_Corner_Down_Left_CR;
    public GameObject RenderTexture_Corner_Down_Right_CR;

    float Corners_Width_Float;
    float Corners_Height_Float;

    // Use this for initialization
    void Start ()
    {

        Corners_Height_Float = Mathf.Abs(RenderTexture_Corner_Down_Left_CR.transform.position.y - RenderTexture_Corner_Up_Left_CR.transform.position.y);
        Corners_Width_Float = Mathf.Abs(RenderTexture_Corner_Up_Left_CR.transform.position.x - RenderTexture_Corner_Up_Right_CR.transform.position.x);

	}
	
	// Update is called once per frame
	void Update ()
    {
        Corners_Height_Float = Mathf.Abs(RenderTexture_Corner_Down_Left_CR.transform.position.y - RenderTexture_Corner_Up_Left_CR.transform.position.y);
        Corners_Width_Float = Mathf.Abs(RenderTexture_Corner_Up_Left_CR.transform.position.x - RenderTexture_Corner_Up_Right_CR.transform.position.x);

        if (CameraOfRenderTexture_Camera.targetTexture != null) { CameraOfRenderTexture_Camera.targetTexture.Release(); }

        CameraOfRenderTexture_Camera.targetTexture = new RenderTexture((int)Corners_Width_Float, (int)Corners_Height_Float, 16);

        RenderTextureHolder_RawImage_CR.texture = CameraOfRenderTexture_Camera.targetTexture;

    }
}
