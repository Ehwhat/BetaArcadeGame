using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuipManager : MonoBehaviour {

    public enum Player
    {
        One,
        Two,
        Three,
        Four
    }

    public static QuipManager manager;

    public PlayerQuipDisplayer[] playerQuipDisplayers = new PlayerQuipDisplayer[4];

    public void Awake()
    {
        manager = this;
    }

    public static void SetColour(int player, Color c) 
    {
        manager.playerQuipDisplayers[player].ChangeColour(c);
    }

    public static void SayQuip(int player, string quip)
    {
        SayQuip((Player)Mathf.Clamp(player, 0, 3), quip);
    }

    public static void SayQuip(Player player, string quip)
    {
        if (manager)
        {
            switch (player)
            {
                case Player.One:
                    manager.playerQuipDisplayers[0].SayQuip(quip, true);
                    break;
                case Player.Two:
                    manager.playerQuipDisplayers[1].SayQuip(quip, true);
                    break;
                case Player.Three:
                    manager.playerQuipDisplayers[2].SayQuip(quip, true);
                    break;
                case Player.Four:
                    manager.playerQuipDisplayers[3].SayQuip(quip, true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.LogWarning("No quip manager");
        }
    }


}
