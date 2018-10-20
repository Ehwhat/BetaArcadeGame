﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUC : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ave"))
        {
            other.gameObject.GetComponent<ArmourScript>().AddTile();

            Destroy(gameObject);
        }
    }
}
