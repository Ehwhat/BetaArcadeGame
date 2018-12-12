using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSprite : MonoBehaviour {

    public SpriteRenderer tankBaseRenderer;
    public SpriteRenderer tankBaseOutlineRenderer;

    public SpriteRenderer tankTurretRenderer;
    public SpriteRenderer tankTurretOutlineRenderer;

    public SpriteRenderer tankTreadsRenderer;
    public SpriteRenderer tankTreadsOutlineRenderer;

    public void LoadTankDefinitionSprites(TankDefinition definition)
    {
        tankBaseRenderer.sprite = definition.tankBase.sprite;
        tankBaseOutlineRenderer.sprite = definition.tankBase.outlineSprite;

        tankTurretRenderer.sprite = definition.tankTurret.sprite;
        tankTurretOutlineRenderer.sprite = definition.tankTurret.outlineSprite;

        tankTreadsRenderer.sprite = definition.tankTreads.sprite;
        tankTreadsOutlineRenderer.sprite = definition.tankTreads.outlineSprite;
    }

    public void SetColour(Color colour)
    {
        tankBaseOutlineRenderer.color = colour;
        tankTurretOutlineRenderer.color = colour;
        tankTreadsOutlineRenderer.color = colour;
    }

}
