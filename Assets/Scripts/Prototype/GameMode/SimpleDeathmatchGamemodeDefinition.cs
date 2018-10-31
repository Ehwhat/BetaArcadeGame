using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[CreateAssetMenu(menuName = "Tanks/Gamemodes/New Simple Deathmatch Definition", fileName = "New Simple Deathmatch Definition")]
public class SimpleDeathmatchGamemodeDefinition : GamemodeDefinition
{
    [System.Serializable]
    public class DeathmatchPlayerData
    {
        public bool isDead = false;
        public float currentDeadTime = 0;
        public int currentDeaths = 0;
        public int currentKills = 0;
    }

    public float respawnTime = 2;
    public int killsToWin;

    [SerializeField]
    private Dictionary<int, DeathmatchPlayerData> deathmatchPlayerData = new Dictionary<int, DeathmatchPlayerData>();

    public override void OnGameStart(GameManager gameManager)
    {
        foreach (TankManager tank in gameManager.currentTanks)
        {
            tank.onDeath += OnPlayerDeath;
            deathmatchPlayerData.Add(tank.tankID, new DeathmatchPlayerData());
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

    public void OnPlayerDeath(TankManager tank, DamageData hit)
    {
        
        PlayerTankManager playerTank = (PlayerTankManager)tank;
        if (hit is TankProjectileDamageData)
        {
            TankProjectileDamageData tankHit = (TankProjectileDamageData)hit;
            TankManager killer = tankHit.projectileInstance.weaponData.ownerTank;
            if (killer)
            {
                int killerIndex = killer.tankID;
                Debug.Log(killerIndex);
                deathmatchPlayerData[killerIndex].currentKills++;
                Josh.EventSystem.EventResponder.TriggerEvent("DisplayText", "PLAYER " + playerTank.tankID + " WAS KILLED BY PLAYER " + killer.tankID);
            }
        }
        int playerIndex = playerTank.tankID;
        deathmatchPlayerData[playerIndex].isDead = true;
        deathmatchPlayerData[playerIndex].currentDeadTime = 0;
        deathmatchPlayerData[playerIndex].currentDeaths++;
    }
}
