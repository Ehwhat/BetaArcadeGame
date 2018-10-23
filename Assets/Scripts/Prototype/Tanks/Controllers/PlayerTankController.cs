using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using InControl;


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

        InputDevice inputDevice = InputManager.Devices[(int)convertedState.player];

        Vector2 leftStickInput = inputDevice.LeftStick.Vector;
        Vector2 rightStickInput = inputDevice.RightStick.Vector;

        Debug.Log(leftStickInput);

        if (!manager.isDead)
        {
            manager.tankMovement.targetVector = leftStickInput;
            manager.tankMovement.targetSpeed = leftStickInput.magnitude;
            manager.AimTurrets(rightStickInput);
        }

        if(inputDevice.RightTrigger.IsPressed)
        {
            manager.FireTurrets();
        }

        convertedState.lastState = currentState;
        return convertedState;
    }


}
