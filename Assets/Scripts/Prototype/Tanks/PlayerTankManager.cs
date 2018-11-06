using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankManager : TankManager {

    public int playerIndex;
    public PlayerTankData data;
    PlayerTankControllerState controllerState;

    public void OnCreated(CharacterDefinition definition, PlayerTankData playerData, int playerIndex)
    {
        this.playerIndex = playerIndex;
        data = playerData;
        data.SetIsInGame(true);
        data.SetHealthPercentage(health / maxHealth);
        controllerState = new PlayerTankControllerState()
        {
            player = playerIndex
        };
    }

    public override void Process()
    {
        controllerState = (PlayerTankControllerState)controller.Process(this, controllerState);
        data.SetHealthPercentage(health / maxHealth);
    }

}
