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

    public PlayerQuipDisplayer player1;
    public PlayerQuipDisplayer player2;
    public PlayerQuipDisplayer player3;
    public PlayerQuipDisplayer player4;

    public void Start()
    {
        manager = this;
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
                    manager.player1.SayQuip(quip, true);
                    break;
                case Player.Two:
                    manager.player2.SayQuip(quip, true);
                    break;
                case Player.Three:
                    manager.player3.SayQuip(quip, true);
                    break;
                case Player.Four:
                    manager.player4.SayQuip(quip, true);
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
