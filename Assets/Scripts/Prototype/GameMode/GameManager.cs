using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public GameDataDefinition gameData;
    public LevelManager levelManager;
    public GameUI uiManager;

    public TMPro.TextMeshProUGUI winUI;

    public PlayerTankManager[] players = new PlayerTankManager[4];
    public List<TankManager> currentTanks = new List<TankManager>();

    public QuipSystemDefinition announcerQuipSystem;

    private GamemodeDefinition gameMode;
    private int latestTankId = 0;

	void Start () {

        for (int i = 0; i < gameData.playersData.Length; i++)
        {
            gameData.playersData[i].SetIsInGame(false);
        }

        CreatePlayers();
        SpawnPlayers();
        gameMode = gameData.gamemode;
        gameMode.OnGameStart(this);
        announcerQuipSystem.SayQuip("Hello Audience!");

    }
	
	void Update () {

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
            if (gameData.IsPlayerJoined(i))
            {
                CreatePlayer(i, gameData.GetCharacter(i));
            }
        }
    }

    private void SpawnPlayers()
    {
        List<Transform> playerSpawners = levelManager.GetSpawnPositions();
        for (int i = 0; i < 4; i++)
        {
            if (gameData.IsPlayerJoined(i))
            {
                int random = Random.Range(0, playerSpawners.Count);
                SpawnPlayer(i, playerSpawners[random].position);
                playerSpawners.RemoveAt(random);
            }
        }
    }

    private void CreatePlayer(int player, CharacterDefinition definiton)
    {
        players[player] = CreateTank(definiton.tankPrefab);
        players[player].gameObject.SetActive(false);
        players[player].tankDisplayName = "Player " + (player + 1);
        players[player].OnCreated(definiton, gameData.playersData[player], player);
        
    }

    private void SpawnPlayer(int player, Vector3 position)
    {
        PlayerTankManager manager = players[player];
        manager.transform.position = position;
        manager.gameObject.SetActive(true);
        manager.ClearTrails();
    }

    public T CreateTank<T>(T tankPrefab) where T : TankManager
    {
        T tank = Instantiate(tankPrefab);
        currentTanks.Add(tank);
        tank.tankID = latestTankId++;
        return tank;
    }

    public void RespawnPlayer(int player)
    {
        List<Transform> playerSpawners = levelManager.GetSpawnPositions();
        int random = Random.Range(0, playerSpawners.Count);
        SpawnPlayer(player, playerSpawners[random].position);
        players[player].Respawn();
    }

    public bool IsPlayerValid(int player)
    {
        if(player >= 0 && player < 4)
        {
            return gameData.IsPlayerJoined(player);
        }
        return false;
    }

    public int GetPlayerCount()
    {
        int playerCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (gameData.IsPlayerJoined(i))
            {
                playerCount++;
            }
        }
        return playerCount;
    }
}
