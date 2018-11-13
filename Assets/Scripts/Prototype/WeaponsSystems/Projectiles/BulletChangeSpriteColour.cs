using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletChangeSpriteColour : MonoBehaviour {

    public ProjectileRepresentation projectileRepresentation;
    public SpriteRenderer sprite;

    public void Awake()
    {
        projectileRepresentation.onCustomColourAdded += OnChangeColour;
    }

    private void OnChangeColour(Color obj)
    {
        sprite.color = obj;
    }
}
