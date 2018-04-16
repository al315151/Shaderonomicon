using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour {
    
    #region MESHES_VARIABLES
    GameObject MeshReference = null;
    Camera MeshCamera = null;
    #endregion

    #region VARIABLES_MOVEMENT_CAMERA
    // Sauce sacada de aqui https://www.youtube.com/watch?v=bVo0YLLO43s
    private Vector3 _LocalRotation;
    private float _CameraDistance = 10f;
    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 2f;
    public float OrbitalDampening = 10f;
    public float ScrollDampening = 6f;
    #endregion

    // Use this for initialization
    void Start ()
    {       
        if (ShaderEdition.currentInstance != null && (MeshCamera == null || MeshReference == null))
        {
            print("Llegamos a entrar aqui?");
            MeshCamera = ShaderEdition.currentInstance.ActiveCamera;
            MeshReference = ShaderEdition.currentInstance.displayObject;
        }      
	}
	
	void LateUpdate()
	{
       
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

                //Scroll out to object faster, scroll in slower.
                ScrollAmount *= (_CameraDistance * 0.3f);

                _CameraDistance += ScrollAmount * -1f;

                _CameraDistance = Mathf.Clamp(_CameraDistance, 1.5f, 500f);
            }

        Quaternion qt = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        MeshReference.transform.rotation = Quaternion.Lerp(MeshReference.transform.rotation,
                                                        qt, Time.deltaTime * OrbitalDampening);

        if (MeshCamera.transform.localPosition.z != _CameraDistance * -1f)
        {
            MeshCamera.transform.localPosition = new Vector3(0f, 0f, 
                                                 Mathf.Lerp(MeshCamera.transform.localPosition.z,
                                                 _CameraDistance * -1f, Time.deltaTime * ScrollDampening));


        }

    }

    public void NootNoot()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _LocalRotation.x -= Input.GetAxis("Mouse X") * MouseSensitivity;
            _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

            //Clamp de Y rotation to horizon, and to top of object?
            //_LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0f, 90f);
        }
    }

}
