using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUEPicker : MonoBehaviour {

    Vector3 mousePosition;

    // Use this for initialization
    void Start ()
    {
        Physics.queriesHitTriggers = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
           // mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
       


        }
    }

}
