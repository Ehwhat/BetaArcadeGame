using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        tag = "Ave";
	}
	
	// Update is called once per frame
	void Update () {

    }
    
    public void AddTile()
    {
        tag = "Full";
        GetComponent<Renderer>().enabled = true;
        if (name[6] == '1')
        {
            transform.parent.Find("Armour2").GetComponent<Collider2D>().enabled = true;
        }
    }

    /*void OnTriggerEnter(Collider other)
    {
        print("collision");
        if (other.gameObject.CompareTag("Collect"))
        {
            print("collision2");
            other.gameObject.SetActive(false);
        }
    }*/
}
