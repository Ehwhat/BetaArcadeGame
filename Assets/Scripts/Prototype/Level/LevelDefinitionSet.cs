using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Levels/New Level Set", fileName = "New Level Set")]
public class LevelDefinitionSet : ScriptableObject {

    public LevelDefinition[] levelDefinitions;
    public int Count
    {
        get { return levelDefinitions.Length; }
    }

    public LevelDefinition GetDefinition(int i)
    {
        if (i >= 0 && i < levelDefinitions.Length)
        {
            return levelDefinitions[i];
        }
        return null;
    }


}
