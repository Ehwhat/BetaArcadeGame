﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int numberOfPlayers = 0;

    [SerializeField]
    public GameDataDefinition gameData;
    public LevelManager levelManager;
    public GameUI uiManager;

    public TankProjectileSetup tankProjectileSetup;

    public TankManager defaultTank;
    public PlayerTankController playerDefaultController;

    public TMPro.TextMeshProUGUI winUI;

    public PlayerTankManager[] players = new PlayerTankManager[4];
    public List<TankManager> currentTanks = new List<TankManager>();

    public QuipSystemDefinition announcerQuipSystem;

    private GamemodeDefinition gameMode;
    private int latestTankId = 0;

    public float RespawnTime
    {
        get { return gameMode.respawnTime; }
    }

	void Start () {
        tankProjectileSetup.Setup();
        gameMode = GameDataMonobehaviour.instance.gamemode;
        for (int i = 0; i < GameDataMonobehaviour.instance.playersData.Length; i++)
        {
            GameDataMonobehaviour.instance.playersData[i].SetIsInGame(false);
        }

        numberOfPlayers = GetPlayerCount();
        CreatePlayers();
        SpawnPlayers();
        gameMode.OnGameStart(this);
        announcerQuipSystem.SayQuip("Hello Audience!");

        Shader.SetGlobalVectorArray("_PlayerColours", new Vector4[] {
            GameDataMonobehaviour.instance.playerColour[0],
            GameDataMonobehaviour.instance.playerColour[1],
            GameDataMonobehaviour.instance.playerColour[2],
            GameDataMonobehaviour.instance.playerColour[3]});

    }
	
	void Update () {

        if (Input.GetKey(KeyCode.K))
        {
            StageSpotlight.FollowTransform(players[0].transform);
        }

        GamemodeDefinition.WinResult winResult = gameMode.VerifyWin(this, Time.deltaTime);
        if(winResult.finished)
        {
            gameMode.OnGameEnd(this);
            winUI.gameObject.SetActive(true);
            if (winResult.winners.Count > 1)
            {
                winUI.SetText("EVERYONE'S A WINNER!");
            }
            else
            {
                winUI.SetText(players[winResult.winners[0]].tankDisplayName + " Wins!");
            }
            StartCoroutine(GoBackToMainMenu());

        }

        if (Input.GetKey(KeyCode.Space))
        {
            winUI.gameObject.SetActive(true);
            if (winResult.winners.Count > 1)
            {
                winUI.SetText("EVERYONE'S A WINNER!");
            }
            else
            {
                winUI.SetText(players[winResult.winners[0]].tankDisplayName + " Wins!");
            }
            StartCoroutine(GoBackToMainMenu());
        }

        Vector4[] playerPositions = new Vector4[4];
        for (int i = 0; i < 4; i++)
        {
            if (IsPlayerValid(i))
            {
                playerPositions[i] = new Vector4(players[i].transform.position.x, players[i].transform.position.y, players[i].transform.position.z, 1);
            }
            else
            {
                playerPositions[i] = Vector4.zero;
            }
        }
        Shader.SetGlobalVectorArray("_PlayerPositions", playerPositions);

    }

    IEnumerator GoBackToMainMenu()
    {
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void CreatePlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            if (GameDataMonobehaviour.instance.IsPlayerJoined(i))
            {
                CreatePlayer(i, GameDataMonobehaviour.instance.playersData[i]);
            }
        }
    }

    private void SpawnPlayers()
    {
        List<Transform> playerSpawners = levelManager.GetSpawnPositions();
        for (int i = 0; i < 4; i++)
        {
            if (GameDataMonobehaviour.instance.IsPlayerJoined(i))
            {
                int random = Random.Range(0, playerSpawners.Count);
                SpawnPlayer(i, playerSpawners[random].position);
                playerSpawners.RemoveAt(random);
            }
        }
    }

    private void CreatePlayer(int player, PlayerTankData playerData)
    {
        players[player] = CreateTank<PlayerTankManager>(playerDefaultController, GameDataMonobehaviour.instance.selectedTank[player]);
        players[player].gameObject.SetActive(false);
        players[player].tankDisplayName = "Player " + (player + 1);
        players[player].OnCreated(GameDataMonobehaviour.instance.playersData[player], player);
        players[player].SetTurretOwners();
        
    }

    private void SpawnPlayer(int player, Vector3 position)
    {
        PlayerTankManager manager = players[player];
        manager.transform.position = position;
        manager.gameObject.SetActive(true);
        manager.SetRespawnParameters(gameMode.respawnTime, GetRespawnLocation);
        manager.ClearTrails();
    }

    public T CreateTank<T>(TankController controller, TankDefinition definition) where T : TankManager
    {
        T tank = (T)Instantiate(defaultTank);
        tank.LoadTankDefinition(definition);
        defaultTank.controller = controller;
        currentTanks.Add(tank);
        tank.tankID = latestTankId++;
        return tank;
    }

    private Vector3 GetRespawnLocation()
    {
        var spawnLocations = levelManager.spawnPoints;

        Vector3 bestPosition = spawnLocations[0].position;
        float bestScore = 0;
        for (int j = 0; j < currentTanks.Count; j++)
        {
            bestScore += (currentTanks[j].transform.position - bestPosition).sqrMagnitude;
        }

        for (int i = 0; i < spawnLocations.Length; i++)
        {
            Vector3 point = spawnLocations[i].position;
            float score = 0;
            for (int j = 0; j < currentTanks.Count; j++)
            {
                score += (currentTanks[j].transform.position - point).sqrMagnitude;
            }
            if(score > bestScore)
            {
                bestPosition = point;
                bestScore = score;
            }
        }
        return bestPosition;
    }

    public bool IsPlayerValid(int player)
    {
        if(player >= 0 && player < 4)
        {
            return GameDataMonobehaviour.instance.IsPlayerJoined(player);
        }
        return false;
    }

    public int GetPlayerCount()
    {
        int playerCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (GameDataMonobehaviour.instance.IsPlayerJoined(i))
            {
                playerCount++;
            }
        }
        return playerCount;
    }
}
