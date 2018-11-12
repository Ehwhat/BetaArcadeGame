using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public struct WeaponFiringPoint
{
    public Vector2 position;
    public Vector2 direction;
    public WeaponFiringPoint(Vector2 position, Vector2 direction)
    {
        this.position = position;
        this.direction = direction;
    }
}

public class TankWeaponHolder : MonoBehaviour {

    public bool attachDefaultAtStart = false;
    public TankWeapon defaultWeapon;

    public TankWeapon weapon;
    public Transform particleSystemHolder;
    public Transform visualisationHolder;
    public AudioSource audioPlayer;

    private ParticleSystem activeParticleSystem;
    private WeaponVisualisation currentVisualisation;

    public float lastFired = 0;
    public float currentDurability = 0;

    public void Start()
    {
        if(defaultWeapon != null && attachDefaultAtStart)
        {
            SetWeapon(defaultWeapon);
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

    public virtual void FireWeapon()
    {
        FireWeapon(transform.up);
    }

    public virtual void FireWeapon(Vector2 direction)
    {
        if (weapon != null)
        {
            TankProjectileData data = new TankProjectileData();
            data.ownerWeaponHolder = this;
            if (lastFired + weapon.firingDelay < Time.time)
            {
                if (weapon.FireProjectile(transform.position, direction, data))
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
                    currentDurability -= weapon.perShotDuribilityCost;
                }
                if (currentDurability <= 0)
                {
                    SetWeapon(defaultWeapon);
                }
            }
        }

    }

    public void OnDrawGizmos()
    {
        if(weapon != null)
            weapon.OnDrawGizmos();
    }

}
