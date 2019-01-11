using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuSelector : VisualMenuSelector<LevelDefinition>
{
    [SerializeField]
    LevelDefinitionSet set;
    [SerializeField]
    Image levelImage;
    [SerializeField]
    Animator animationController;

    int currentIndex = 0;

    private void Start()
    {
        LoadLevelImage();
        displayText.text = set.GetDefinition(currentIndex).levelName;
    }

    public override LevelDefinition GetResult()
    {
        
        return set.GetDefinition(currentIndex);
    }

    public void LoadLevelImage()
    {
        LevelDefinition activeDefinition = set.GetDefinition(currentIndex);
        if (activeDefinition.levelImage)
        {
            levelImage.enabled = true;
            levelImage.sprite = activeDefinition.levelImage;
            levelImage.preserveAspect = true;
        }
        else
        {
            levelImage.enabled = false;
        }
    }

    public override void NextOption()
    {
        currentIndex++;
        if(currentIndex >= set.levelDefinitions.Length)
        {
            currentIndex = 0;
        }
        displayText.text = set.GetDefinition(currentIndex).levelName;
        animationController.SetTrigger("OnLeft");
    }

    public override void PreviousOption()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = set.levelDefinitions.Length-1;
        }
        displayText.text = set.GetDefinition(currentIndex).levelName;
        animationController.SetTrigger("OnRight");
    }
}
