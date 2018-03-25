using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour {

    Camera MeshCamera;
    GameObject MeshReference;

    #region ROTATE_AROUND_VARIABLES
    float distanceFromObjectToCamera;
    public float rotationForce;
    Vector3 lastFrameMousePosition;
    #endregion

    #region MOUSE_DETECTION_VARIABLES
    Image selfDetectionImage;
    float timeThresoldInputDetection = 0.05f;
    float timerInputDetection = 0.0f;
    #endregion


    // Use this for initialization
    void Start ()
    {
        selfDetectionImage = GetComponent<Image>();
        if (ShaderEdition.currentInstance != null)
        {
            MeshCamera = ShaderEdition.currentInstance.ActiveCamera;
            MeshReference = ShaderEdition.currentInstance.displayObject;
        }
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timerInputDetection > timeThresoldInputDetection)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (ClickInsideBounds())
                { lastFrameMousePosition = Input.mousePosition; }
            }
            else if (Input.GetMouseButton(0))
            {
                if (ClickInsideBounds())
                { DetectMousePosition(); }
            }

            if (MeshCamera == null || MeshReference == null)
            {
                MeshCamera = ShaderEdition.currentInstance.ActiveCamera;
                MeshReference = ShaderEdition.currentInstance.displayObject;
            }

            timerInputDetection = 0.0f;
        }
        timerInputDetection = timerInputDetection + Time.deltaTime;
	}

    void RotateCameraOnMesh(Vector3 axisRotation)
    {
        MeshCamera.transform.RotateAround(MeshReference.transform.position, new Vector3(1.0f, 0.0f, 0.0f), axisRotation.x);
        MeshCamera.transform.RotateAround(MeshReference.transform.position, new Vector3(0.0f, 1.0f, 0.0f), axisRotation.y);
        MeshCamera.transform.RotateAround(MeshReference.transform.position, new Vector3(0.0f, 0.0f, 1.0f), axisRotation.z);
    }


    public void DetectMousePosition()
    {
        /*print(Input.mousePosition + " --> Mouse Position");
        print((selfDetectionImage.transform.position) + " --> Image Position");*/
        Vector3 AxisMovement;
        AxisMovement.x = Input.mousePosition.x - lastFrameMousePosition.x;
        AxisMovement.y = Input.mousePosition.y - lastFrameMousePosition.y;
        AxisMovement.z = Input.mousePosition.z - lastFrameMousePosition.z;

        RotateCameraOnMesh(AxisMovement);

        lastFrameMousePosition = Input.mousePosition;
    }


    bool ClickInsideBounds()
    {
        return Input.mousePosition.x >= selfDetectionImage.transform.position.x - selfDetectionImage.rectTransform.rect.width / 2 &&
               Input.mousePosition.x <= selfDetectionImage.transform.position.x + selfDetectionImage.rectTransform.rect.width / 2 &&
               Input.mousePosition.y >= selfDetectionImage.transform.position.y - selfDetectionImage.rectTransform.rect.height / 2 &&
               Input.mousePosition.y <= selfDetectionImage.transform.position.y + selfDetectionImage.rectTransform.rect.height / 2;        
    }

}
