using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatLumiPicker : MonoBehaviour {

    Color[] data;

    RectTransform SLCanvasReference;
    RenderTexture SLRenderTexture;
    Texture2D SLTexture2DReference;
    Texture dummyTexture = new Texture();

    public int SLWidth() { return (int) SLCanvasReference.rect.width; }
    public int SLHeight() { return (int) SLCanvasReference.rect.height; }

    Vector3 mousePosition;

    Color colorPicked;

    public Material SLMaterial;

    /*
     * Página en la que he encontrado el código para que funcione la detección de ratón:
     * https://gamedev.stackexchange.com/questions/117139/how-to-get-a-pixel-color-from-a-specific-sprite-on-touch-unity
     * https://gamedev.stackexchange.com/questions/125371/how-to-write-from-shader-to-a-texture-to-a-png-in-unity
     * 
     * 
     */


    private void Awake()
    {
        SLCanvasReference = gameObject.GetComponent<RectTransform>();
    }


    // Use this for initialization
    void Start ()
    {


        SLTexture2DReference = new Texture2D(SLWidth(), SLHeight(), TextureFormat.ARGB32, false);
        SLRenderTexture = new RenderTexture(SLWidth(), SLHeight(), 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

        Physics.queriesHitTriggers = true;
        Graphics.Blit(dummyTexture, SLRenderTexture, SLMaterial, -1);

        RenderTexture.active = SLRenderTexture;

        SLTexture2DReference.ReadPixels(new Rect(0,0,SLWidth(), SLHeight()), 0, 0, false);
        SLTexture2DReference.GetPixels();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            //print("ESTAMOS DENTRO DE LA FUNCION DE CLICK");

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 screenPos = new Vector2(mousePosition.x, mousePosition.y);

           // RectTransformUtility.ScreenPointToLocalPointInRectangle(SLCanvasReference, screenPos, Camera.main, out screenPos);

            print("Supuesto X y supuesto Y: " + screenPos.x + " , " + screenPos.y);

            RaycastHit2D[] ray = Physics2D.RaycastAll(screenPos, Vector2.zero, 0.01f);

            for (int i = 0; i < ray.Length; i++)
            {
                if (ray[i].collider.tag == "SLPicker")
                {
                    screenPos = screenPos - new Vector2(ray[i].collider.gameObject.transform.position.x,
                                                          ray[i].collider.gameObject.transform.position.y);
                    int x = (int)(screenPos.x * SLWidth());
                    int y = (int)(screenPos.y * SLHeight()) + SLHeight();
                   

                    if (x > 0 && x < SLWidth() && y > 0 && y < SLHeight())
                    {
                        print("ESTAMOS DENTRO DE LA FUNCION DE CLICK");

                        Graphics.Blit(dummyTexture, SLRenderTexture, SLMaterial, -1);
                        SLTexture2DReference.ReadPixels(new Rect(0, 0, SLWidth(), SLHeight()), 0, 0, false);
                        data = SLTexture2DReference.GetPixels();

                        colorPicked = data[y * SLWidth() + x];
                        ShaderEdition.currentInstance._CustomColor = colorPicked;

                    }
                    break;
                }   
                
            }

        } 
        
    }
}
