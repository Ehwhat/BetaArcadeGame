using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    public TankWeapon[] weapons;

    public override void OnPickup(TankManager tank)
    {
        if(weapons.Length > 0)
        {
            tank.GiveWeapon(PickRandomWeapon());
        }
    }

    private TankWeapon PickRandomWeapon()
    {
        return weapons[Random.Range(0, weapons.Length - 1)];
    }
}
