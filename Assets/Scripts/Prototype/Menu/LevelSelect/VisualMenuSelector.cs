using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class VisualMenuSelector<T> : MenuSelector<T> {

    public TextMeshProUGUI displayText;
    public TextMeshProUGUI optionText;
    public Image[] selectionImages;
    public Color selectionColour;
    public Color normalColour;

    public override void OnSelected()
    {
        ChangeColour(selectionColour);
    }

    public override void OnDeselected()
    {
        ChangeColour(normalColour);
    }

    private void ChangeColour(Color colour)
    {
        optionText.color = colour;
        foreach (var image in selectionImages)
        {
            image.color = colour;
        }
    }


}
