using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankRepresentation : MonoBehaviour {

    public Image tankBase;
    public Image tankTreads;
    public Image tankTurret;
    public Image tankBaseOutline;
    public Image tankTreadsOutline;
    public Image tankTurretOutline;

    public void LoadTankDefinition(TankDefinition definition)
    {
        tankBase.sprite = definition.tankBase.sprite;
        tankTreads.sprite = definition.tankTreads.sprite;
        tankTurret.sprite = definition.tankTurret.sprite;

        tankBaseOutline.sprite = definition.tankBase.outlineSprite;
        tankTreadsOutline.sprite = definition.tankTreads.outlineSprite;
        tankTurretOutline.sprite = definition.tankTurret.outlineSprite;
    }

    public void SetColour(Color colour)
    {
        tankBaseOutline.color = colour;
        tankTreadsOutline.color = colour;
        tankTurretOutline.color = colour;
    }

}
