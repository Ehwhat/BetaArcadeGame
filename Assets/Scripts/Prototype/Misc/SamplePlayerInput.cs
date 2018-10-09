using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerInput : MonoBehaviour {

    public static System.Action clickAction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("fire1"))
        {
            clickAction();
        }
	}
}
