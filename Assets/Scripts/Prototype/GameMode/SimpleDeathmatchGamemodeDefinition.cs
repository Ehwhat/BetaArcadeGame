using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tanks/Gamemodes/New Simple Deathmatch Definition", fileName = "New Simple Deathmatch Definition")]
public class SimpleDeathmatchGamemodeDefinition : GamemodeDefinition
{

    class DeathmatchPlayerData
    {
        public bool isDead = false;
        public float currentDeadTime = 0;
        public int currentDeaths = 0;
        public int currentKills = 0;
    }

    public float respawnTime = 2;
    public int killsToWin;

    [System.NonSerialized]
    private DeathmatchPlayerData[] deathmatchPlayerData = new DeathmatchPlayerData[4];

    public override void OnGameStart(GameManager gameManager)
    {
        for (int i = 0; i < 4; i++)
        {
            if (gameManager.IsPlayerValid(i))
            {
                Debug.Log(gameManager.players[i]);
                gameManager.players[i].onDeath += OnPlayerDeath;
                deathmatchPlayerData[i] = new DeathmatchPlayerData();
            }
        }
    }

    public override WinResult VerifyWin(GameManager gameManager, float deltaTime)
    {
        for (int i = 0; i < 4; i++)
        {
            if (gameManager.IsPlayerValid(i))
            {
                if (deathmatchPlayerData[i].currentKills >= killsToWin)
                {
                    return (WinResult)i;
                }
                if (deathmatchPlayerData[i].isDead)
                {
                    deathmatchPlayerData[i].currentDeadTime += deltaTime;
                    if (deathmatchPlayerData[i].currentDeadTime > respawnTime)
                    {
                        Debug.Log("Respawn");
                        gameManager.RespawnPlayer(i);
                        deathmatchPlayerData[i].currentDeadTime = 0;
                        deathmatchPlayerData[i].isDead = false;
                    }
                }
            }
        }
        return WinResult.None;
    }

    public void OnPlayerDeath(TankManager tank, ProjectileHit hit)
    {
        PlayerTankManager playerTank = (PlayerTankManager)tank;
        if (hit.holder.owningRigidbody)
        {
            PlayerTankManager killer = hit.holder.owningRigidbody.GetComponent<PlayerTankManager>();
            int killerIndex = killer.playerIndex;
            Debug.Log(killerIndex);
            deathmatchPlayerData[killerIndex].currentKills++;
        }
        int playerIndex = playerTank.playerIndex;
        deathmatchPlayerData[playerIndex].isDead = true;
        deathmatchPlayerData[playerIndex].currentDeadTime = 0;
        deathmatchPlayerData[playerIndex].currentDeaths++;
    }
}
