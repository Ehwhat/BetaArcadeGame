using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public GameDataDefinition gameData;
    public LevelManager levelManager;

    public PlayerTankManager[] players = new PlayerTankManager[4];

	void Start () {
        CreatePlayers();
        SpawnPlayers();
	}
	
	void Update () {
		
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
         players[player] = Instantiate(definiton.tankPrefab);
        players[player].gameObject.SetActive(false);
        players[player].OnCreated(definiton, player);
    }

    private void SpawnPlayer(int player, Vector3 position)
    {
        PlayerTankManager manager = players[player];
        manager.transform.position = position;
        manager.gameObject.SetActive(true);
        manager.ClearTrails();
    }

    public bool IsPlayerValid(int player)
    {
        if(player > 0 && player < 4)
        {
            return gameData.IsPlayerJoined(player);
        }
        return false;
    }
}
