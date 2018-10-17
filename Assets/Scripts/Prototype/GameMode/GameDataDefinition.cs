using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

[CreateAssetMenu(menuName = "Tanks/Gamemodes/New Game Data Definition", fileName = "New Game Data Definition")]
public class GameDataDefinition : ScriptableObject {

    [SerializeField]
    private CharacterDefinition[] characters = new CharacterDefinition[4];
    [SerializeField]
    private bool[] playersJoined = new bool[4];

    public void SetCharacter(int index, CharacterDefinition character)
    {
        characters[index] = character;
    }

    public CharacterDefinition GetCharacter(int index)
    {
        return characters[index];
    }

    public void SetPlayerJoined(int index, bool joined)
    {
        playersJoined[index] = joined;
    }

    public bool IsPlayerJoined(int index)
    {
        return playersJoined[index];
    }
	
}
