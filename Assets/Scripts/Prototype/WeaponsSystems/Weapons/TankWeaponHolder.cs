﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class TankWeaponHolder : MonoBehaviour {

    public TankManager ownerTank;

    public TankWeapon defaultWeapon;

    public TankWeapon weapon;
    public Transform particleSystemHolder;
    public Transform visualisationHolder;
    public AudioSource audioPlayer;

    private ParticleSystem activeParticleSystem;
    private WeaponVisualisation currentVisualisation;

    public float lastFired = 0;
    public float currentDurability = 0;
    public bool equiptOnStart;

    public void Start()
    {
        if(defaultWeapon != null && equiptOnStart)
        {
            EquipDefaultWeapon();
        }
    }

    public void SetWeapon(TankWeapon newWeapon)
    {
        RemoveCurrentWeapon();
        weapon = newWeapon;
        if (activeParticleSystem)
        {
            Destroy(activeParticleSystem.gameObject);
        }
        if (weapon.onFiredParticleSystem)
        {
            activeParticleSystem = Instantiate(weapon.onFiredParticleSystem, particleSystemHolder);
        }
        if (weapon.weaponVisualisation)
        {
            currentVisualisation = Instantiate(weapon.weaponVisualisation, visualisationHolder);
        }
        lastFired = -weapon.firingDelay;
        currentDurability = weapon.maxDurability;

        if (ownerTank is PlayerTankManager) {
            PlayerTankData data = (ownerTank as PlayerTankManager).data;
            Color colour = data.playerColour;
            Color colourEnd = new Color(colour.r, colour.g, colour.b, 0);
            BarrelAimingIndicator barrelAimingIndicator = currentVisualisation.GetComponentInChildren<BarrelAimingIndicator>();
            if (barrelAimingIndicator)
            {
                barrelAimingIndicator.barrelLineRendeer.startColor = colour;
                barrelAimingIndicator.barrelLineRendeer.endColor = colourEnd;
            }
        }
    }

    public void RemoveCurrentWeapon()
    {
        weapon = null;
        if (activeParticleSystem)
        {
            Destroy(activeParticleSystem.gameObject);
        }
        if (currentVisualisation)
        {
            Destroy(currentVisualisation.gameObject);
        }
    }

    public void EquipDefaultWeapon()
    {
        SetWeapon(defaultWeapon);
    }

    public virtual void OnFiringDown()
    {

    }

    public virtual void OnFiringHold()
    {

    }

    public virtual void OnFiringUp()
    {

    }

    public virtual void FireWeapon(Vector2 direction)
    {
        if (weapon != null && gameObject.activeInHierarchy)
        {
            if (lastFired + weapon.firingDelay < Time.time)
            {
                if (weapon.FireProjectile(transform.position, direction, this))
                {
                    if (activeParticleSystem)
                    {
                        activeParticleSystem.Play();
                    }
                    if (weapon.onFiredClip)
                    {
                        audioPlayer.PlayOneShot(weapon.onFiredClip);
                    }
                    lastFired = Time.time;
                    if(weapon.useDuribility)
                        currentDurability -= weapon.perShotDuribilityCost;
                }
                if (currentDurability <= 0 && weapon.useDuribility)
                {
                    SetWeapon(defaultWeapon);
                }
            }
        }

    }

    public void PlayFiredEffects()
    {

    }

    public void OnDrawGizmos()
    {
        if(weapon != null)
            weapon.OnDrawGizmos();
    }

}
