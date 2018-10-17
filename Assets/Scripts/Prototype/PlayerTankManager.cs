﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankManager : TankManager {

    PlayerTankController.PlayerTankControllerState controllerState;

    public void OnCreated(CharacterDefinition definition, int playerIndex)
    {
        controllerState = new PlayerTankController.PlayerTankControllerState()
        {
            player = (XInputDotNetPure.PlayerIndex)playerIndex
        };
    }

    public override void Process()
    {
        controllerState = (PlayerTankController.PlayerTankControllerState)controller.Process(this, controllerState);
    }

}
