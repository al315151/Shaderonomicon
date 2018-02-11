using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatLumiPicker : MonoBehaviour {


    Vector3 mousePosition;

    // Use this for initialization
    void Start ()
    {
        Physics.queriesHitTriggers = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }


}
