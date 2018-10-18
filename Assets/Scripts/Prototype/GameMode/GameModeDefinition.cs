using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamemodeDefinition : ScriptableObject {

    public enum WinResult
    {
        Player1,
        Player2,
        Player3,
        Player4,
        None,
        Loss,
        All
    }

    public virtual void OnGameStart(GameManager gameManager) { }

    public abstract WinResult VerifyWin(GameManager gameManager, float deltaTime);

    public virtual void OnGameEnd(GameManager gameManager) { }


	
}
