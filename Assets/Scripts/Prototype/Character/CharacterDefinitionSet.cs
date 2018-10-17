using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Characters/New Character Definiton Set")]
public class CharacterDefinitionSet : ScriptableObject {

    public CharacterDefinition[] characterDefinitions;

    public int Count
    {
        get { return characterDefinitions.Length; }
    }

    public CharacterDefinition GetDefinition(int i)
    {
        if(i >= 0 && i < characterDefinitions.Length)
        {
            return characterDefinitions[i];
        }
        return null;
    }

}
