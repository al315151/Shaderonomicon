using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour {

    Camera MeshCamera;
    GameObject MeshReference;

    #region ROTATE_AROUND_VARIABLES
    float distanceFromObjectToCamera;
    Vector3 lastFrameMousePosition;
    #endregion

    #region MOUSE_DETECTION_VARIABLES
  
    #endregion


    // Use this for initialization
    void Start ()
    {       
        if (ShaderEdition.currentInstance != null)
        {
            MeshCamera = ShaderEdition.currentInstance.ActiveCamera;
            MeshReference = ShaderEdition.currentInstance.displayObject;
        }		
	}
	
	// Update is called once per frame
	void Update ()
    {
       if (MeshCamera == null || MeshReference == null)
       {
           MeshCamera = ShaderEdition.currentInstance.ActiveCamera;
           MeshReference = ShaderEdition.currentInstance.displayObject;
       }
	}

    void RotateCameraOnMesh(Vector3 axisRotation)
    {
        MeshCamera.transform.RotateAround(MeshReference.transform.position, new Vector3(1.0f, 0.0f, 0.0f), axisRotation.x);
        MeshCamera.transform.RotateAround(MeshReference.transform.position, new Vector3(0.0f, 1.0f, 0.0f), axisRotation.y);
        MeshCamera.transform.RotateAround(MeshReference.transform.position, new Vector3(0.0f, 0.0f, 1.0f), axisRotation.z);
    }


    public void DetectMousePosition()
    {
        print("We're in!!!");
        /*print(Input.mousePosition + " --> Mouse Position");
        print((selfDetectionImage.transform.position) + " --> Image Position");*/
        Vector3 AxisMovement;
        AxisMovement.x = Input.mousePosition.x - lastFrameMousePosition.x;
        AxisMovement.y = Input.mousePosition.y - lastFrameMousePosition.y;
        AxisMovement.z = Input.mousePosition.z - lastFrameMousePosition.z;

        RotateCameraOnMesh(AxisMovement);

        lastFrameMousePosition = Input.mousePosition;
    }










}
