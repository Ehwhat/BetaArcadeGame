using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreSystem : MonoBehaviour {

    public static PlayerScoreSystem scoresystem;

    public TMPro.TextMeshProUGUI[] playerScores;

	// Use this for initialization
	void Start () {
        scoresystem = this;

    }
	
	public static void SetScore(int player, int score)
    {
        scoresystem.playerScores[player].text = score.ToString("D2");
    }
}
