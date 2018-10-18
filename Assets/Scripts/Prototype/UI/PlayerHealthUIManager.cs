using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIManager : MonoBehaviour {

    public TankManager tank;
    public Image healthBarImage;

	// Update is called once per frame
	void Update () {
        if (tank)
        {
            healthBarImage.fillAmount = tank.health / tank.maxHealth;
        }
        else
        {
            healthBarImage.fillAmount = 0;
        }
	}
}
