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
        if (name[6] == '2')
        {
            foreach (Transform child in transform.parent.parent)
            {
                if (child.name[0] == 'W')
                {
                    child.GetComponent<WeaponPlatformCode>().WeaponCheck();
                }
            }
        }
    }
    
}
