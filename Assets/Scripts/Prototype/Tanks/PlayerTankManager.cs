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

        Color colour = data.playerColour;
        Color colourEnd = new Color(colour.r, colour.g, colour.b, 0);

        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].startColor = colour;
            trails[i].endColor = colourEnd;
        }

        QuipManager.SetColour(playerIndex, colour);

    }

    public override void Process()
    {
        controllerState = (PlayerTankControllerState)controller.Process(this, controllerState);
        data.SetHealthPercentage(health / maxHealth);
    }

    public override void OnHit(DamageData hit)
    {
        base.OnHit(hit);
        if (isDead)
        {
            QuipManager.SayQuip(playerIndex, "AHH @&%-");
        }
        else
        {
            QuipManager.SayQuip(playerIndex, "Ouch!");
        }
    }


    public override void GiveWeapon(TankWeapon weapon)
    {
        base.GiveWeapon(weapon);
        QuipManager.SayQuip(playerIndex, "I just got the " + weapon.displayName+"!");
        
        
    }

}
