using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankPartDefinition : ScriptableObject {

    public Sprite sprite;
    public Sprite outlineSprite;
    public float strength = 1;
    public float weight = 1;

}
