using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HueBarManager : MonoBehaviour {

    public Image selector;
    public Image selectorTint;
    public float width;
    public Vector2 offset;

    public void SetSelector(float hueAmount, Color colour)
    {
        selectorTint.color = colour;
        Vector2 position = ((transform as RectTransform).anchoredPosition + offset) - (Vector2.right * width/2) + Vector2.right * hueAmount * width;
        selector.rectTransform.anchoredPosition = position;
    }

	
}
