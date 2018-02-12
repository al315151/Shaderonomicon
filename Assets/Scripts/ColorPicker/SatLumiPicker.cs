using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    GraphicRaycaster raycaster;
    PointerEventData eventData;
    EventSystem eventSystem;

    private void Awake()
    {
        SLCanvasReference = gameObject.GetComponent<RectTransform>();
    }


    // Use this for initialization
    void Start ()
    {
        print("DEBUG TIME!!!!");

        raycaster = gameObject.GetComponent<GraphicRaycaster>();

        eventSystem = GetComponent<EventSystem>();

        SLTexture2DReference = new Texture2D(SLWidth(), SLHeight(), TextureFormat.ARGB32, false);

        print("ANCHURA DE TEXTURA2D: " + SLTexture2DReference.width); //140
        print("ALTURA DE TEXTURA2D: " + SLTexture2DReference.height); //140

        SLRenderTexture = new RenderTexture(SLWidth(), SLHeight(), 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

        print("ANCHURA DE RENDER TEXTURE: " + SLRenderTexture.width);
        print("ALTURA DE RENDER TEXTURE: " + SLRenderTexture.height);

        Physics.queriesHitTriggers = true;
        Graphics.Blit(dummyTexture, SLRenderTexture, SLMaterial, -1);

        RenderTexture.active = SLRenderTexture;

        SLTexture2DReference.ReadPixels(new Rect(0,0,SLWidth(), SLHeight()), 0, 0, false);
        SLTexture2DReference.Apply();
        SLTexture2DReference.GetPixels();

        print("POSICION RECT TRANSFORM" + SLCanvasReference.position);
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            //print("ESTAMOS DENTRO DE LA FUNCION DE CLICK");

            //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = Input.mousePosition;
            Vector2 screenPos = new Vector2(mousePosition.x, mousePosition.y);

            //print("Supuesto X y supuesto Y: " + screenPos.x + " , " + screenPos.y);

            //RaycastHit[] ray = Physics.RaycastAll(screenPos, Vector2.zero, 0.01f);

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
                    //print("Really collides??");
                    //screenPos = screenPos - new Vector2(ray[i].collider.gameObject.transform.position.x,
                    //                                     ray[i].collider.gameObject.transform.position.y);
                    //int x = (int)(screenPos.x * SLWidth());
                    //int y = (int)(screenPos.y * SLHeight()) + SLHeight();

                    int xMinCanvas = (int) (SLCanvasReference.position.x - (SLCanvasReference.rect.width / 2)) ;
                    int yMinCanvas = (int) (SLCanvasReference.position.y - (SLCanvasReference.rect.height / 2));
                    int xMaxCanvas = (int) (SLCanvasReference.position.x + (SLCanvasReference.rect.width / 2));
                    int yMaxCanvas = (int) (SLCanvasReference.position.y + (SLCanvasReference.rect.height / 2));

                    //print("Limites de rect canvas: " + "MinX: " + xMinCanvas + " , Max X: " + xMaxCanvas);
                    //print("Limites de rect canvas: " + "MinY: " + yMinCanvas + " , Max Y: " + yMaxCanvas);

                    int texture2Dx = Mathf.FloorToInt(screenPos.x / SLCanvasReference.rect.width * SLTexture2DReference.width);
                    int texture2Dy = Mathf.FloorToInt(screenPos.y / SLCanvasReference.rect.height * SLTexture2DReference.height);

                    print("Coordenada TEX2D X: " + texture2Dx + " , TEX2D Y: " + texture2Dy);

                    if (screenPos.x > xMinCanvas && screenPos.x < xMaxCanvas && screenPos.y > yMinCanvas && screenPos.y < yMaxCanvas)
                    {
                        //print("ESTAMOS DENTRO DE LA FUNCION DE CLICK");

                        Graphics.Blit(dummyTexture, SLRenderTexture, SLMaterial, -1);
                        SLTexture2DReference.ReadPixels(new Rect(0, 0, SLWidth(), SLHeight()), 0,0, false);
                        SLTexture2DReference.Apply();
                        data = SLTexture2DReference.GetPixels();
                        SLTexture2DReference.EncodeToPNG();

                        
                        colorPicked = SLTexture2DReference.GetPixel(texture2Dx, texture2Dy) * SLTexture2DReference.height;
                        print(colorPicked);

                        //colorPicked = data[y * SLWidth() + x];
                        //colorPicked = data[yMinCanvas * SLWidth() + xMinCanvas]; DA OUT OF RANGE, NO SIRVE.
                        ShaderEdition.currentInstance._CustomColor = colorPicked;

                    }
                    break;
                }   
                
            }

        } 
        
    }
}
