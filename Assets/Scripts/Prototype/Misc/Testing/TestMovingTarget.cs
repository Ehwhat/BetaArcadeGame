using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TestMovingTarget : MonoBehaviour {

    public float speed;
    public float magnitude;
    public Vector2 vector;
	
	// Update is called once per frame
	void Update () {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = (vector.normalized * magnitude) * Mathf.Sin(Time.time * speed);
	}
}
