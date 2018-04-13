﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SatLumiPicker : MonoBehaviour {

    
    #region CLICK_VARIABLES

    RectTransform SLCanvasReference;
    RenderTexture SLRenderTexture;
    Texture2D SLTexture2DReference;
    Color[] data;
    Texture dummyTexture;

    public int SLWidth() { return (int) SLCanvasReference.rect.width; }
    public int SLHeight() { return (int)SLCanvasReference.rect.height; }

    Vector3 mousePosition;

    GraphicRaycaster raycaster;
    PointerEventData eventData;
    EventSystem eventSystem;

    #endregion

    #region COLOR_VARIABLES

    Color colorPicked;

    public Material SLMaterial;
    //public RawImage testTexture2DCanvasReference;

    #endregion

    /*
     * Página en la que he encontrado el código para que funcione la detección de ratón:
     * https://gamedev.stackexchange.com/questions/117139/how-to-get-a-pixel-color-from-a-specific-sprite-on-touch-unity
     * https://gamedev.stackexchange.com/questions/125371/how-to-write-from-shader-to-a-texture-to-a-png-in-unity
     * 
     * 
     */

    public Image cursorPositionImage;


    bool settersComplete = false;


    private void Awake()
    {
        SLCanvasReference = gameObject.GetComponentInParent<RectTransform>();
        
    }

    void ScriptSetters()
    {
        print("DEBUG TIME!!!!");

        raycaster = gameObject.GetComponent<GraphicRaycaster>();

        eventSystem = GetComponent<EventSystem>();

        SLTexture2DReference = new Texture2D(SLWidth(), SLHeight(), TextureFormat.ARGB32, false);

        //dummyTexture = (Texture) SLTexture2DReference;

        print("ANCHURA DE TEXTURA2D: " + SLTexture2DReference.width); //140
        print("ALTURA DE TEXTURA2D: " + SLTexture2DReference.height); //140

        SLRenderTexture = new RenderTexture(SLWidth(), SLHeight(), 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

        print("ANCHURA DE RENDER TEXTURE: " + SLRenderTexture.width);
        print("ALTURA DE RENDER TEXTURE: " + SLRenderTexture.height);

        Physics.queriesHitTriggers = true;
        Graphics.Blit(dummyTexture, SLRenderTexture, SLMaterial, -1);

        RenderTexture.active = SLRenderTexture;

        SLTexture2DReference.ReadPixels(new Rect(0, 0, SLWidth(), SLHeight()), 0, 0, false);
        SLTexture2DReference.Apply();
        data = SLTexture2DReference.GetPixels();

        //testTexture2DCanvasReference.texture = SLTexture2DReference;

        print("POSICION RECT TRANSFORM" + SLCanvasReference.position);

        settersComplete = true;

    }


    // Use this for initialization
    void Start ()
    {
        //Invoke("ScriptSetters", 0.01f);
    }
	
	// Update is called once per frame
	void Update ()
    {




        #region BULLSHIT
        /*
        if (settersComplete && (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)))
        {
            mousePosition = Input.mousePosition;
            Vector2 screenPos = new Vector2(mousePosition.x, mousePosition.y);

           

            eventData = new PointerEventData(eventSystem);
            eventData.position = mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            //print("Numero de raycast resultantes: " + results.Count);

            //for (int i = 0; i < ray.Length; i++)
            for (int i = 0; i < results.Count; i++)
            {
               // if (ray[i].collider.tag == "SLPicker")
               if (results[i].gameObject.tag == "SLPicker")
                {
                
                    float xMinCanvas = SLCanvasReference.position.x - (SLWidth() / 2);
                    float yMinCanvas = SLCanvasReference.position.y - (SLHeight() / 2);
                    float xMaxCanvas = SLCanvasReference.position.x + (SLWidth() / 2);
                    float yMaxCanvas = SLCanvasReference.position.y + (SLHeight() / 2);

                    int texture2Dx = Mathf.RoundToInt((screenPos.x) % SLTexture2DReference.width);
                    int texture2Dy = Mathf.RoundToInt((screenPos.y) % SLTexture2DReference.height);

                                     

                    if (screenPos.x > xMinCanvas && screenPos.x < xMaxCanvas && screenPos.y > yMinCanvas && screenPos.y < yMaxCanvas)
                    {
                        //print("ESTAMOS DENTRO DE LA FUNCION DE CLICK");

                        cursorPositionImage.rectTransform.position = new Vector3(screenPos.x, screenPos.y, -0.02f);

                        Graphics.Blit(dummyTexture, SLRenderTexture, SLMaterial, -1);
                        SLTexture2DReference.ReadPixels(new Rect(0, 0, SLWidth(), SLHeight()), 0,0, false);
                        SLTexture2DReference.Apply();
                        data = SLTexture2DReference.GetPixels();
                        SLTexture2DReference.EncodeToPNG();

                        
                        colorPicked = SLTexture2DReference.GetPixel(texture2Dx ,texture2Dy) * SLTexture2DReference.height;
                        print(colorPicked);
                        colorPicked = new Color(colorPicked.r / 127.5f, colorPicked.g / 127.5f, colorPicked.b / 127.5f, 1.0f);
                        print(colorPicked);

                        //testTexture2DCanvasReference.texture = SLTexture2DReference;
                        //colorPicked = data[y * SLWidth() + x];
                        //colorPicked = data[yMinCanvas * SLWidth() + xMinCanvas]; DA OUT OF RANGE, NO SIRVE.
                       // ShaderEdition.currentInstance._CustomColor = colorPicked;

                    }
                    break;
                }   
                
            }
        */
        #endregion
    }

    public void DoSomething()
    {
        


        print("AAAAAAAAAAAAAH"); }

}

