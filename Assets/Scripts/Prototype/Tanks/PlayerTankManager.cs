using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankManager : TankManager {

    public int playerIndex;
    PlayerTankControllerState controllerState;

    public void OnCreated(CharacterDefinition definition, int playerIndex)
    {
        this.playerIndex = playerIndex;
        controllerState = new PlayerTankControllerState()
        {
            player = playerIndex
        };
    }

    public override void Process()
    {
        controllerState = (PlayerTankControllerState)controller.Process(this, controllerState);
    }

}
