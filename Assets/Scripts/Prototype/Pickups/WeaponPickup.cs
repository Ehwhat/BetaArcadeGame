using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    [System.Serializable]
    public struct WeaponPickupItem
    {
        public TankWeapon weapon;
        [Range(0,1)]
        public float chance;
        public bool super;
    }

    public SpriteRenderer representationRenderer;
    public WeaponPickupItem[] weapons;

    private TankWeapon currentWeapon;
    

    private void Start()
    {
        ScaleWeapons();
        OnRespawn();
    }

    private void ScaleWeapons()
    {
        float totalChance = 0;
        for (int i = 0; i < weapons.Length; i++)
        {
            totalChance += weapons[i].chance;
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].chance /= totalChance;
        }

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
        float chanceValue = Random.Range(0f, 1f);
        Debug.Log(chanceValue);
        float currentChance = 0;
        WeaponPickupItem weapon = weapons[0];
        for (int i = 0; i < weapons.Length; i++)
        {
            currentChance += weapons[i].chance;
            if (chanceValue <= currentChance)
            {
                Debug.Log(weapons[i].weapon.displayName);
                weapon = weapons[i];
                break;
            }
        }

        representationRenderer.color = weapon.super ? Color.yellow : Color.white;

        return weapon.weapon;
    }
}
