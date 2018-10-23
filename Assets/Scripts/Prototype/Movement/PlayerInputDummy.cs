using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInputDummy : MonoBehaviour {

    public PlayerIndex player;
    public TankMovement move;
    public TurretController turret;

	// Update is called once per frame
	void Update () {

        GamePadState pad = GamePad.GetState(player);

        Vector2 input = new Vector2(
            pad.ThumbSticks.Left.X,
            pad.ThumbSticks.Left.Y
            );
        input = Vector3.ClampMagnitude(input, 1);

        Vector2 rInput = new Vector2(
            pad.ThumbSticks.Right.X,
            pad.ThumbSticks.Right.Y
            );
        rInput = Vector3.ClampMagnitude(rInput, 1);

        move.targetVector = input;
        move.targetSpeed = input.magnitude;

        turret.targetVector = rInput;
        

        if (Input.GetButtonDown("Fire1"))
        {
            for (int i = 0; i < 100; i++)
            {
                turret.Fire();
            }
            
            move.ApplyOneTimeBoost(4, 1.0f);
        }
	}
}
