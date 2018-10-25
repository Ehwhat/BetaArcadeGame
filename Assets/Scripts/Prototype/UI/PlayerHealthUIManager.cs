using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIManager : MonoBehaviour {

    public TankManager tank;
    public Image healthBarImage;
    public float maxAmmoSize = 500;
    public float padding = 5;

    private float ammoSize;

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

    void CalculateAmmoSize(float cost,float maxDurability)
    {
        int totalShots = Mathf.FloorToInt(maxDurability / cost);
        float totalPadding = padding * totalShots;
        ammoSize = (maxAmmoSize - totalPadding) / totalShots;
    }
}
