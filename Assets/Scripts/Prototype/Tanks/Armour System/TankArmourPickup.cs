using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankArmourPickup : MonoBehaviour {

    public float cooldownTime = 2;

    private bool useCooldown = false;
    private float whenDropped;
    private TankArmourManager droppedManager;

    public void OnDrop(TankArmourManager manager)
    {
        useCooldown = true;
        whenDropped = Time.time;
        droppedManager = manager;
    }

    public bool IsPickupValid(TankArmourManager manager)
    {
        return (!useCooldown || manager != droppedManager || (Time.time > whenDropped + cooldownTime));
    }

}
