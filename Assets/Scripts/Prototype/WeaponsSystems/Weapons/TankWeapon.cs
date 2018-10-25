﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Weapons/New Tank Weapon", fileName = "New Tank Weapon")]
public class TankWeapon : Weapon
{
    [Header("Weapon Settings")]
    [Space(5)]
    [Header("Firing Projectile")]
    public TankProjectile projectile;

    [Space(10)]
    [Header("Weapon Firing Settings")]
    public Vector2[] weaponFiringOffsets = new Vector2[] { Vector2.zero };

    public float firingDelay;
    public bool useDuribility = true;
    public float maxDurability = 100;
    public float perShotDuribilityCost = 10;
    [Range(0,1)]
    public float rarity = 0.5f;

    public WeaponVisualisation weaponVisualisation;

    public AudioClip onFiredClip;
    public AudioClip onHitClip;

    public ParticleSystem onFiredParticleSystem;


    public virtual bool FireProjectile(Vector2 position, Vector2 direction, TankProjectileData firingData = null)
    {
        for (int i = 0; i < weaponFiringOffsets.Length; i++)
        {
            firingData.ownerWeapon = this;
            Vector2 offset = firingData.ownerWeaponHolder.transform.rotation * weaponFiringOffsets[i];
            projectile.OnFired(position+ offset, direction, firingData);
            return true;
        }
        return false;
    }

    

    public float GetRateOfFire()
    {
        return (1f / firingDelay);
    }

    public float CalculateWeaponCoefficent()
    {
        return ((projectile.damage * (GetRateOfFire()*60) * (maxDurability/perShotDuribilityCost)) / ((1-rarity)*100))/100;
    }

}