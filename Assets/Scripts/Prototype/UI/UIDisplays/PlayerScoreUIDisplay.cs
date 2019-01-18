using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScoreUIDisplay : MonoBehaviour {

    public TMPro.TextMeshProUGUI[] playerScores = new TMPro.TextMeshProUGUI[4];
    public Image[] playerFrames;
    

    public void SetScore(int player, int score)
    {
        if(playerScores[player])
            playerScores[player].text = score.ToString("D2");
    }

    public void SetActive(int player, bool active)
    {
        playerFrames[player].gameObject.SetActive(active);
    }

    public void SetColour(int player, Color colour)
    {
        playerFrames[player].color = colour;
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
