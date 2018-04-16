using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour {

    
    GameObject MeshReference = null;

    #region ROTATE_AROUND_VARIABLES
    float distanceFromObjectToCamera;
    Vector3 lastFrameMousePosition;
    #endregion

    #region CAMERA_VARIABLES
    Camera MeshCamera = null;
    #endregion

    #region VARIABLES_MOVEMENT_CAMERA
    // Mira a ver si se puede hacer  https://answers.unity.com/questions/1257281/how-to-rotate-camera-orbit-around-a-game-object-on.html

    public float distance;
    public float xSpeed = 3.0f;
    public float ySpeed = 3.0f;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    public float distanceMin = 10f;
    public float distanceMax = 10f;
    public float smoothTime = 2f;
    private float rotationYAxis = 0.0f;
    private float rotationXAxis = 0.0f;
    private float velocityX = 0.0f;
    private float velocityY = 0.0f;

    #endregion

    // Use this for initialization
    void Start ()
    {       
        /*if (ShaderEdition.currentInstance != null && (MeshCamera == null || MeshReference == null))
        {
            print("Llegamos a entrar aqui?");
            MeshCamera = ShaderEdition.currentInstance.ActiveCamera;
            MeshReference = ShaderEdition.currentInstance.displayObject;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && MeshCamera != null)
        {
            MeshCamera.transform.localPosition = new Vector3(MeshCamera.transform.localPosition.x,
                                                             MeshCamera.transform.localPosition.y, 
                                                             MeshCamera.transform.localPosition.z + Input.GetAxis("Mouse ScrollWheel"));

        }
		*/

        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;

	}
	
	void LateUpdate()
	{
		if (Input.GetMouseButton(0))
		{
			


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

        float differenceX = Mathf.Abs(Input.mousePosition.x - lastFrameMousePosition.x);
        float differenceY = Mathf.Abs(Input.mousePosition.y - lastFrameMousePosition.y); 
        float differenceZ = Mathf.Abs(Input.mousePosition.z - lastFrameMousePosition.z);

        if (differenceX >= differenceY && differenceX >= differenceZ)
        {
            AxisMovement.x = 0;
            AxisMovement.y = differenceZ;
            AxisMovement.z = differenceY;
        }
        else if (differenceY >= differenceX && differenceY >= differenceZ)
        {
            AxisMovement.x = differenceX;
            AxisMovement.y = 0;
            AxisMovement.z = differenceY;
        }
        else
        {
            AxisMovement.x = differenceX;
            AxisMovement.y = differenceZ;
            AxisMovement.z = 0;
        }


        RotateCameraOnMesh(AxisMovement);

        lastFrameMousePosition = Input.mousePosition;
    }


    /* Copia de cube wars, camara: revisa y rapiñea lo que veas
     * 
     *  private float EDGE_PANNING_MAX_X = 290;
    private float EDGE_PANNING_MIN_X = -20;
    private float EDGE_PANNING_MAX_Y = 290;
    private float EDGE_PANNING_MIN_Y = -20;

    private float EDGE_PANNING_LIMIT_ABSOLUTE_Y = 30;
    private const float EDGE_PANNING_SPEED = 210f /*90f*/                           // Velocidad del edge panning
   /* private const float RIGHT_CLICK_TURN_SPEED = 100f;                      // Velocidad de giro de la camara
    private const float EDGE_PANNING_THS_HORIZONTAL = 0.4f/*0.475f*/               // Posicion del raton necesaria para mover la pantalla en X (de 0 a 0.5)
   /* private const float EDGE_PANNING_THS_VERTICAL = 0.375f/*0.45f*/                  // Posicion del raton necesaria para mover la pantalla en Y (de 0 a 0.5)
   /* private const float ZOOM_SPEED = 2f;                                    // Velocidad de zoom
    private const float ZOOM_MIN_HEIGHT = 3.5f;                                // Altura minima del zoom
    private const float ZOOM_HEIGHT_SCALING = 25f;                          // Escalado de altura del zoom (el zoom maximo sera, posicion minima + escalado)
    private const float ZOOM_MIN_ANGLE = 30f;                               // Angulo minimo de la camara de zoom
    private const float ZOOM_ANGLE_SCALING = 30f;                           // Escalado de angulo del zoom (el angulo maximo sera, angulo minimo + escalado)

    private float zoomTarget_T = 0.5f;
    private float zoomCurrent_T = 0.5f;

    private float mousePositionPercentX;
    private float mousePositionPercentY;

    public Transform zoomTransform;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            UpdateRightClickTurning();
        }
        else
        {
            UpdateEdgePanning();
        }
        UpdateZoom();

    }
    void UpdateZoom()
    {
        zoomTarget_T -= Input.GetAxis("Mouse ScrollWheel");
        zoomTarget_T = Mathf.Clamp01(zoomTarget_T);
        zoomCurrent_T = Mathf.MoveTowards(zoomCurrent_T, zoomTarget_T, Time.deltaTime * ZOOM_SPEED);
        zoomTransform.localPosition = new Vector3(0, ZOOM_MIN_HEIGHT + (zoomCurrent_T * ZOOM_HEIGHT_SCALING), 0);
        zoomTransform.localRotation = Quaternion.Euler(ZOOM_MIN_ANGLE + zoomCurrent_T * ZOOM_ANGLE_SCALING, 0, 0);
    }
    void UpdateRightClickTurning()
    {
        transform.Rotate(0, RIGHT_CLICK_TURN_SPEED * Input.GetAxis("Mouse X") * Time.deltaTime, 0);
    }
    void UpdateEdgePanning()
    {
        mousePositionPercentX = (Input.mousePosition.x / Screen.width) - 0.5f;
        mousePositionPercentY = (Input.mousePosition.y / Screen.height) - 0.5f;
        if (Mathf.Abs(mousePositionPercentX) > EDGE_PANNING_THS_HORIZONTAL)
        {
            mousePositionPercentX = Mathf.MoveTowards(mousePositionPercentX, 0, EDGE_PANNING_THS_HORIZONTAL);
            transform.Translate(Vector3.right * mousePositionPercentX * EDGE_PANNING_SPEED * Time.deltaTime);
        }
        if (Mathf.Abs(mousePositionPercentY) > EDGE_PANNING_THS_VERTICAL)
        {
            mousePositionPercentY = Mathf.MoveTowards(mousePositionPercentY, 0, EDGE_PANNING_THS_VERTICAL);
            transform.Translate(Vector3.forward * mousePositionPercentY * EDGE_PANNING_SPEED * Time.deltaTime);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, EDGE_PANNING_MIN_X, EDGE_PANNING_MAX_X), 0, Mathf.Clamp(transform.position.z, EDGE_PANNING_MIN_Y, EDGE_PANNING_MAX_Y));
    }
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * */







}
