using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    public SpriteRenderer representationRenderer;
    public TankWeapon[] weapons;

    private TankWeapon currentWeapon;
   

    private void Start()
    {
        OnRespawn();
    }

    public override void OnPickup(TankManager tank)
    {
        if(weapons.Length > 0)
        {
            tank.GiveWeapon(currentWeapon);
            representationRenderer.enabled = false;
        }
    }

    public override void OnRespawn()
    {
        currentWeapon = PickRandomWeapon();
        representationRenderer.sprite = currentWeapon.weaponVisualisation.representedSprite;
        representationRenderer.enabled = true;
    }

    private TankWeapon PickRandomWeapon()
    {
        return weapons[Random.Range(0, weapons.Length)];
    }
}
