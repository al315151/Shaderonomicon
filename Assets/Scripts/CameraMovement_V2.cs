using UnityEngine;
using UnityEngine.UI;

public class CameraMovement_V2 : MonoBehaviour {

  
    public Transform displayMesh_Transform_CR;

    //This code is from: https://www.youtube.com/watch?v=bVo0YLLO43s

    protected Transform _xForm_Camera;
    protected Transform _XForm_Parent;
    protected Vector3 _XForm_displayMesh_Pos;

    protected Vector3 _LocalRotation;
    //Initial distance between camera and point of interest.
    protected float _CameraDistance = 21.5f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = true;

	// Use this for initialization
	void Start () {
        _xForm_Camera = transform;
        _XForm_Parent = transform.parent;
        CameraDisabled = true;
        _XForm_displayMesh_Pos = displayMesh_Transform_CR.position;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {

        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {   CameraDisabled = !CameraDisabled;        }
        */

        if (!CameraDisabled)
        {
            //Rotation of the camera
            if (Input.GetAxis("Mouse X") != 0 ||
                Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;


               // _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0, 90f);
            }

            //Scrolling Input
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

                ScrollAmount *= (_CameraDistance * 0.3f);

                _CameraDistance += ScrollAmount * -1f;

                _CameraDistance = Mathf.Clamp(_CameraDistance, 1.5f, 100f);
            }

            //camera rig transformation

            Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);

            _XForm_Parent.rotation = Quaternion.Lerp(_XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

            if (_xForm_Camera.localPosition.z != _CameraDistance * -1f)
            {
                Vector3 newPosition = new Vector3(0f, 0f, Mathf.Lerp(_xForm_Camera.localPosition.z, _CameraDistance * -1f, Time.deltaTime * ScrollDampening));
                _xForm_Camera.localPosition = newPosition;
                _XForm_displayMesh_Pos = -1 * newPosition;
            }


        }

        displayMesh_Transform_CR.rotation = Quaternion.Euler(90f, -90f, 90f);
        //displayMesh_Transform_CR.position = _XForm_displayMesh_Pos;

    }

    public void DisableMovement()
    {
        CameraDisabled = true;
    }

    public void EnableMovement()
    {
        CameraDisabled = false;
    }



}
