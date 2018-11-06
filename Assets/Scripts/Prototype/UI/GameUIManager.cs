using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour {

    public PlayerHealthUIManager[] healthManagers;
    public TextMeshProUGUI text;

    public void SetupHealthUIMananger(int index, TankManager tank)
    {
        //healthManagers[index].tank = tank;
    }

    public void ShowWin(int player)
    {
        //text.gameObject.SetActive(true);
        //text.text = "Player " + (player+1) + " Wins!";
    }
	
}
