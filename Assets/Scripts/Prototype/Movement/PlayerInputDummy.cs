using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputDummy : MonoBehaviour {

    public TankMovement move;

	// Update is called once per frame
	void Update () {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
            );
        input = Vector3.ClampMagnitude(input, 1);
        move.targetVector = input;
        move.targetSpeed = input.magnitude;

        if (Input.GetButtonDown("Fire1"))
        {
            move.ApplyOneTimeBoost(3, 0.3f);
        }
	}
}
