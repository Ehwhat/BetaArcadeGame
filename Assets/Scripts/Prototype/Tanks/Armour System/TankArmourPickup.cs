using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankArmourPickup : MonoBehaviour {

    public float cooldownTime = 2;

    private bool useCooldown = false;
    private float whenDropped;

    public void OnDrop()
    {
        useCooldown = true;
        whenDropped = Time.time;
    }

    public bool isPickupValid()
    {
        return (!useCooldown || Time.time > whenDropped + cooldownTime);
    }

}
