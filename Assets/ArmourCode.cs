using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCode : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if(child.name[0] == 'A')
            {
                child.GetComponent<Renderer>().enabled = false;
                if (child.name[6] == '2')
                {
                    child.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            else if(child.name[0] == 'W')
            {
                child.GetComponent<Renderer>().enabled = false;
                child.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space") == true)
        {
            print("space");
        }
	}
}
