using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform followTransform;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 target = new Vector3(followTransform.position.x, followTransform.position.y, -10);
        transform.position = Vector3.MoveTowards(transform.position, target, 10);
	}
}
