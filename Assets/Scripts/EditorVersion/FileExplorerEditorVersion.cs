using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# if UNITY_EDITOR
using UnityEditor;

public class FileExplorerEditorVersion : EditorWindow {

    [MenuItem("Example/Overwite Texture")]

    static void Apply()
    {
        Texture2D texture = null;
       /* if (texture == null)
        {
            EditorUtility.DisplayDialog("Select Texture", "You must select a texture", "OK...");
            return;
        }
        */
        string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            texture.LoadImage(fileContent);
        }
        
    }




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
#endif