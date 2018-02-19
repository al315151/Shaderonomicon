using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShaderExport : MonoBehaviour {

    byte[] sequence;

	// Use this for initialization
	void Start ()
    {
        SaveFile();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SaveFile()
    {
      
         StreamWriter newShader = new StreamWriter(ShaderEdition.currentInstance.FilePath, true, System.Text.Encoding.UTF8);
         newShader.Write(shaderTitle);
         
         newShader.Close();  
    }

    string shaderTitle = "Shader "+ '"' + "Shaderonomicon/" + ShaderEdition.currentInstance.shaderName + '"';


       
        
        
        
        
        
        
        
        
        
        
        
        
        
        
       







}
