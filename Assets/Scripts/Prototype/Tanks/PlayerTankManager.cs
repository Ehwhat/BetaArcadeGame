using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankManager : TankManager {

    public SpriteRenderer tankSprite;
    public int playerIndex;
    public PlayerTankData data;
    public QuipSystemDefinition quipSystem;
    PlayerTankControllerState controllerState;

    public void OnCreated(CharacterDefinition definition, PlayerTankData playerData, int playerIndex)
    {
        this.playerIndex = playerIndex;
        data = playerData;
        data.SetIsInGame(true);
        data.SetHealthPercentage(health / maxHealth);
        quipSystem = data.quipSystem;

        controllerState = new PlayerTankControllerState()
        {
            player = playerIndex
        };

        Color colour = data.playerColour;
        Color colourEnd = new Color(colour.r, colour.g, colour.b, 0);


        MaterialPropertyBlock props = new MaterialPropertyBlock();
        tankSprite.GetPropertyBlock(props);
        props.SetColor("_TintColour", colour);
        tankSprite.SetPropertyBlock(props);



        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].startColor = colour;
            trails[i].endColor = colourEnd;
        }

        quipSystem.SetColour(colour);

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
            quipSystem.SayQuip("AHH @&%-");
        }
        else
        {
            quipSystem.SayQuip("Ouch!");
        }
    }


    public override void GiveWeapon(TankWeapon weapon)
    {
        base.GiveWeapon(weapon);
        quipSystem.SayQuip("I just got the " + weapon.displayName+"!");
        
        
    }

}
