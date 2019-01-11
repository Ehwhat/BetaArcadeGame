﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Gamemodes/New Game Data Definition", fileName = "New Game Data Definition")]
public class GameDataDefinition : ScriptableObject {

    public GamemodeDefinition gamemode;

    public PlayerTankData[] playersData = new PlayerTankData[4];
    public bool[] playersJoined = new bool[4];

    public void SetPlayerJoined(int index, bool joined)
    {
        playersJoined[index] = joined;
    }

    public bool IsPlayerJoined(int index)
    {
        return playersJoined[index];
    }
	
}