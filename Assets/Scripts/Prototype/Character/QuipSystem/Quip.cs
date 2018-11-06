using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Characters/QuipSystem/Quip")]
public class Quip : ScriptableObject {

    public enum Filter
    {
        None,
        ByCharacter
    }

    public string[] contexts;
    public Filter quipFilterMode;
    public CharacterDefinition[] characterFilter;

    public QuipStage[] stages;
}
