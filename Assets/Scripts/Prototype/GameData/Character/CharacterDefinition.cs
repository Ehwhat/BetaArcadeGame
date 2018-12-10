using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Characters/New Character Definition",fileName = "New Character Definition")]
public class CharacterDefinition : ScriptableObject {
    public string name;
    [TextArea]
    public string description;
    public PlayerTankManager tankPrefab;
    public GameObject tankDisplayPrefab;
    public Sprite portrait;

    public CharacterDialogDefinition dialogDefinition;
}
