using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {

    public Camera cameraReference;

    public GameObject cameraObjectiveReference;

   


    // Use this for initialization
    void Start ()
    {
        cameraReference = GetComponent<Camera>();
        if (ShaderEdition.currentInstance != null)
        {
            cameraObjectiveReference = ShaderEdition.currentInstance.displayObject;
            ShaderEdition.currentInstance.ActiveCamera = cameraReference;
        }
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (cameraReference != null && cameraObjectiveReference != null)
        {
            cameraReference.transform.LookAt(cameraObjectiveReference.transform);
        }
      
	}

}
