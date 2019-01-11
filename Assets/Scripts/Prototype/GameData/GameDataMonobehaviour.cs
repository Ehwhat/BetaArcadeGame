using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMonobehaviour : MonoBehaviour {

    public static GameDataMonobehaviour instance;

    public GamemodeDefinition gamemode;
    public int timer = 5;

    public PlayerTankData[] playersData = new PlayerTankData[4];
    public bool[] playersJoined = new bool[4];
    public Color[] playerColour = new Color[4];
    public CharacterDefinition[] selectedCharacter = new CharacterDefinition[4];
    public TankDefinition[] selectedTank = new TankDefinition[4];

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        
        DontDestroyOnLoad(gameObject);
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
