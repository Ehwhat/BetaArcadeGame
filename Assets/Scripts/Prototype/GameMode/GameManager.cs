using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public GameDataDefinition gameData;
    public LevelManager levelManager;
    public GameUIManager uiManager;

    public PlayerTankManager[] players = new PlayerTankManager[4];
    public List<TankManager> currentTanks = new List<TankManager>();

    private GamemodeDefinition gameMode;
    private int latestTankId = 0;

	void Start () {
        CreatePlayers();
        SpawnPlayers();
        gameMode = gameData.gamemode;
        gameMode.OnGameStart(this);
	}
	
	void Update () {

        GamemodeDefinition.WinResult winResult = gameMode.VerifyWin(this, Time.deltaTime);
        if(winResult != GamemodeDefinition.WinResult.None)
        {
            uiManager.ShowWin((int)winResult);
            Debug.Log(winResult);
            StartCoroutine(GoBackToMainMenu());
            
        }
    }

    IEnumerator GoBackToMainMenu()
    {
        yield return new WaitForSeconds(3);
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
        players[player].OnCreated(definiton, player);
        
        uiManager.SetupHealthUIMananger(player, players[player]);
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
}
