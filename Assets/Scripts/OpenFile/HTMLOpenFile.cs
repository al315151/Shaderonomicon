using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.InteropServices;

public class HTMLOpenFile : MonoBehaviour {


    [DllImport("__Internal")]
    private static extern void ImageDownloader(string str, string fn);

    public static byte[] ssData = null;
    public static string imageFilename = "";



    public WWW HyperTextFile;

    public Object HTMLScriptReference;

	// Use this for initialization
	void Start ()
    {  
    
    
    var uri = new System.Uri(Path.GetFullPath(HTMLScriptReference.name));
    string urlPath = uri.AbsoluteUri;

    print(urlPath);

    urlPath = System.Uri.UnescapeDataString(urlPath);

    print(urlPath);

    HyperTextFile = new WWW(urlPath);

    print(HyperTextFile.url);

    Application.OpenURL(HyperTextFile.url);
    StartCoroutine(GetText());
}

    // El codigo viene de https://forum.unity.com/threads/user-image-download-from-in-webgl-app.474715/#post-3100016
    // Si funciona, dale las putas gracias.
    void DownloadScreenshot()
    {
        if (ssData != null)
        {
            Debug.Log("Downloading..." + imageFilename);
            ImageDownloader(System.Convert.ToBase64String(ssData), imageFilename);
        }
    }




    // Update is called once per frame
    void Update ()
    {
		
	}


    //Copiado para obtener una textura según una URL, faltaria que ejecutara el HTML
    //Para obtener la ruta que buscamos.
    IEnumerator GetText()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(HyperTextFile.url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                print("saved Texture");
            }
        }
    }


}
