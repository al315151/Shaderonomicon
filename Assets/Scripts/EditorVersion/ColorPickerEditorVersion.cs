using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# if UNITY_EDITOR
using UnityEditor;



public class ColorPickerEditorVersion : EditorWindow{


    Color auxColor = Color.white;

 
    [MenuItem("ColorPickerEditorVersion / Mass Color Change")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(ColorPickerEditorVersion));
        window.Show();
    }
    
    private void OnGUI()
    {

        auxColor = EditorGUILayout.ColorField("Choose color", auxColor);

        if (GUILayout.Button("DO IT"))
        {
            ShaderEdition.currentInstance._CustomColor = auxColor;
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