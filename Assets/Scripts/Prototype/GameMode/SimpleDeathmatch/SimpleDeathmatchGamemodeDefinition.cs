using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[CreateAssetMenu(menuName = "Tanks/Gamemodes/New Simple Deathmatch Definition", fileName = "New Simple Deathmatch Definition")]
public class SimpleDeathmatchGamemodeDefinition : GamemodeDefinition
{
    [System.Serializable]
    public class DeathmatchTankData
    {
        public bool isDead = false;
        public float currentDeadTime = 0;
        public int currentDeaths = 0;
        public int currentKills = 0;
    }

    public bool useTimer = false;
    public float timerMinutes = 2;
    public float respawnTime = 2;
    public int killsToWin;

    private float timeElapsed;
    private SimpleDeathmatchUI ui;

    [SerializeField]
    private Dictionary<int, DeathmatchTankData> deathmatchPlayerData = new Dictionary<int, DeathmatchTankData>();

    public override void OnGameStart(GameManager gameManager)
    {
        base.OnGameStart(gameManager);
        ui = gamemodeUIGameobject.GetComponent<SimpleDeathmatchUI>();
        foreach (TankManager tank in gameManager.currentTanks)
        {
            tank.onDeath += OnPlayerDeath;
            deathmatchPlayerData.Add(tank.tankID, new DeathmatchTankData());
        }
        ui.playerScoreUIDisplay.Reset();
        timeElapsed = 0;
    }

    public override WinResult VerifyWin(GameManager gameManager, float deltaTime)
    {
        WinResult win = new WinResult() { finished = false };

        List<int> bestPlayers = new List<int>();
        int bestScore = 0;

        for (int i = 0; i < deathmatchPlayerData.Count; i++)
        {
            if(deathmatchPlayerData[i].currentKills > bestScore)
            {
                bestPlayers = new List<int>() { i };
                bestScore = deathmatchPlayerData[i].currentKills;
            }else if (deathmatchPlayerData[i].currentKills == bestScore)
            {
                bestPlayers.Add(i);
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

        if (timeElapsed > System.TimeSpan.FromMinutes(timerMinutes).TotalSeconds)
        {
            win.finished = true;
            win.winners = bestPlayers;
        }
        else
        {
            timeElapsed += deltaTime;
        }

        ui.timerUIDisplay.SetTimer(Mathf.Max((float)(System.TimeSpan.FromMinutes(timerMinutes).TotalSeconds - timeElapsed), 0));

        return win;
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
                Josh.EventSystem.EventResponder.TriggerEvent("DisplayText", playerTank.tankDisplayName.ToUpper() + " WAS KILLED BY " + killer.tankDisplayName.ToUpper());

                PlayerTankManager killerPlayerTank = (PlayerTankManager)killer;

                if (killerPlayerTank) {
                    ui.playerScoreUIDisplay.SetScore(killerIndex,deathmatchPlayerData[killerIndex].currentKills);
                    killerPlayerTank.quipSystem.SayQuip("Ha! Suck it, " + playerTank.tankDisplayName + "!");
                }
                
            }
        }
        int playerIndex = playerTank.tankID;
        deathmatchPlayerData[playerIndex].isDead = true;
        deathmatchPlayerData[playerIndex].currentDeadTime = 0;
        deathmatchPlayerData[playerIndex].currentDeaths++;
    }
}
