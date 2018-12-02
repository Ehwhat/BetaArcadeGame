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

        if (manager.isDead)
        {
            return convertedState;
        }

        InputDevice inputDevice = GameInput.GetPlayerDevice(convertedState.player);

        if(inputDevice == null)
        {
            return convertedState;
        }

        Vector2 leftStickInput = inputDevice.LeftStick.Vector;
        Vector2 rightStickInput = inputDevice.RightStick.Vector;

        if (!manager.isDead)
        {
            manager.tankMovement.targetVector = leftStickInput;
            manager.tankMovement.targetSpeed = leftStickInput.magnitude;
            manager.AimTurrets(rightStickInput);

            if (inputDevice.LeftBumper.WasPressed)
            {
                manager.armourPickupManager.EjectArmourPickups();
            }

            if (inputDevice.RightTrigger.WasPressed)
            {
                manager.FireTurrets(TankManager.FiringInputType.Down);
            }else if (inputDevice.RightTrigger.WasReleased)
            {
                manager.FireTurrets(TankManager.FiringInputType.Up);
            }
            if (inputDevice.RightTrigger.IsPressed)
            {
                manager.FireTurrets(TankManager.FiringInputType.Held);
            }
        }

        

        return convertedState;
    }


}
