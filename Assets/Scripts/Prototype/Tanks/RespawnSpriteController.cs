using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSpriteController : MonoBehaviour {

    public ParticleSystem particles;
    public SpriteRenderer outline;
    public SpriteRenderer center;
    public float alphaLevel = 0.3f;

    public void SetColour(Color colour)
    {
        var main = particles.main;
        main.startColor = colour;
        outline.color = colour;
        colour.a = alphaLevel;
        center.color = colour;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        outline.GetPropertyBlock(block);
        block.SetColor("_EdgeColour", colour);
        outline.SetPropertyBlock(block);
    }
	
}
