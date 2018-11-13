using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreUIDisplay : MonoBehaviour {

    public TMPro.TextMeshProUGUI[] playerScores = new TMPro.TextMeshProUGUI[4];

    public void SetScore(int player, int score)
    {
        if(playerScores[player])
            playerScores[player].text = score.ToString("D2");
    }

    public void Reset()
    {
        for (int i = 0; i < 4; i++)
        {
            if(playerScores[i])
                playerScores[i].text = (0).ToString("D2");
        }
    }
}
