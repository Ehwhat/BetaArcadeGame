using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankManager : TankManager {
    public int playerIndex;
    public PlayerTankData data;
    public QuipSystemDefinition quipSystem;
    public CharacterDialogSystem dialogSystem;
    PlayerTankControllerState controllerState;
    public Color colour;

    public void OnCreated(PlayerTankData playerData, int playerIndex)
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

        colour = GameDataMonobehaviour.instance.playerColour[playerIndex];
        Color colourEnd = new Color(colour.r, colour.g, colour.b, 0);


        //MaterialPropertyBlock props = new MaterialPropertyBlock();
        //tankSprite.GetPropertyBlock(props);
        //props.SetColor("_TintColour", colour);
        //tankSprite.SetPropertyBlock(props);



        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].startColor = colour;
            trails[i].endColor = colourEnd;
        }

        quipSystem.SetColour(colour);
        armourManager.SetColour(colour);
        tankSprite.SetColour(colour);
        var main = deathParticles.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        main.startColor = colour;
        respawnSpriteController.SetColour(colour);

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
            
        }
        else
        {
            dialogSystem.SayDialogFor(playerIndex, GameDataMonobehaviour.instance.selectedCharacter[playerIndex], DialogContext.TakingDamage);
        }
    }


    public override void GiveWeapon(TankWeapon weapon)
    {
        base.GiveWeapon(weapon);
        dialogSystem.SayDialogFor(playerIndex, GameDataMonobehaviour.instance.selectedCharacter[playerIndex], DialogContext.WeaponPickup, weapon.displayName);


    }

}
