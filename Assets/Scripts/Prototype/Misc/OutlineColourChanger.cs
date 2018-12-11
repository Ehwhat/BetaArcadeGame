using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineColourChanger : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    
    public void SetColour(Color colour)
    {
        spriteRenderer.color = colour;
    }
	
}
