using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamemodeDefinition : ScriptableObject {

    public GameObject gamemodeUIPrefab;
    protected GameObject gamemodeUIGameobject;
    public float respawnTime = 2;

    public class WinResult
    {
        public bool finished;
        public List<int> winners = new List<int>();
        public List<int> losers = new List<int>();
        public bool globalLoss;
    }

    public virtual void OnGameStart(GameManager gameManager) {
        if (gamemodeUIPrefab)
        {
            gamemodeUIGameobject = gameManager.uiManager.SpawnGamemodeUI(gamemodeUIPrefab);
        }
    }

    public abstract WinResult VerifyWin(GameManager gameManager, float deltaTime);

    public virtual void OnGameEnd(GameManager gameManager) {

    }


	
}
