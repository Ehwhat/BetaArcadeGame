using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;



[CreateAssetMenu(menuName = "Tanks/Controllers/New Player Tank Controller", fileName = "New Player Tank Controller")]
public class PlayerTankController : TankController {
    public override object Process(TankManager manager, object state = null)
    {
        PlayerTankControllerState convertedState = (state as PlayerTankControllerState);
        if(convertedState == null)
        {
            convertedState = new PlayerTankControllerState();
        }
        GamePadState lastState = convertedState.lastState;
        GamePadState currentState = GamePad.GetState(convertedState.player);

        Vector2 leftStickInput = new Vector2(currentState.ThumbSticks.Left.X, currentState.ThumbSticks.Left.Y);
        Vector2 rightStickInput = new Vector2(currentState.ThumbSticks.Right.X, currentState.ThumbSticks.Right.Y);

        if (!manager.isDead)
        {
            manager.tankMovement.targetVector = leftStickInput;
            manager.tankMovement.targetSpeed = leftStickInput.magnitude;
            manager.AimTurrets(rightStickInput);
        }

        if(currentState.Triggers.Right > 0.2f)
        {
            manager.FireTurrets();
        }

        convertedState.lastState = currentState;
        return convertedState;
    }


}
