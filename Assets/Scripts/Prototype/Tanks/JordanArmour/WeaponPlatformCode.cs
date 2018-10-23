using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlatformCode : MonoBehaviour {

    //public char assignedSpot0 = 'A';
    public char assignedSpot1 = 'B';
    public char assignedSpot2 = 'C';
    public int weaponType = 0;

    // Use this for initialization
    void Start () {
        tag = "Ave";
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void WeaponCheck ()
    {
        if (gameObject.GetComponent<Renderer>().enabled == false)
        {
            //print("No rend");
            int counter = 0;
            foreach (Transform child in transform.parent)
            {
                if (child.name[5] == 'A')
                {
                    print("Child search " + child.name);
                    if ((child.name[11] == assignedSpot1) || (child.name[11] == assignedSpot2))
                    {
                        print("Child found");
                        if (child.Find("Armour2").GetComponent<Renderer>().enabled == true)
                        {
                            print("Counter++");
                            counter++;
                        }
                    }
                }
            }
            if (counter == 2)
            {
                print("Victory");
                GetComponent<Renderer>().enabled = true;
            }
        }
    }
}
