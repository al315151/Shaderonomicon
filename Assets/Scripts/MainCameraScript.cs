using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {

    public Camera cameraReference;
    public GameObject cameraObjectiveReference;





    public bool rotationEnabled = false;

    [Range(0.0f, 10.0f)]
    public float rotationSpeed = 5.0f;


    // Use this for initialization
    void Start ()
    {
        cameraReference = GetComponent<Camera>();
        if (ShaderEdition.currentInstance != null)
        {
            ShaderEdition.currentInstance.ActiveCamera = cameraReference;
            cameraObjectiveReference = ShaderEdition.currentInstance.displayObject;            
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
