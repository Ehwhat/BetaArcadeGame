using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUC : MonoBehaviour {

    public int type = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ave") && type == 0)
        {
            other.gameObject.GetComponent<ArmourPart>().AttachPart(type);

            Destroy(gameObject);
        }
        else if (other.CompareTag("AveW") && type != 0)
        {
            other.gameObject.GetComponent<ArmourPart>().AttachPart(type);

            Destroy(gameObject);
        }
    }
}
