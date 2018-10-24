﻿using System.Collections;
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

    public TankWeapon weapon;
    public Transform particleSystemHolder;
    public AudioSource audioPlayer;

    private ParticleSystem activeParticleSystem;

    public float lastFired = 0;

    public void Start()
    {
        if(weapon != null)
        {
            SetWeapon(weapon);
            weapon.ResetDurability();
        }
    }

    public void SetWeapon(TankWeapon newWeapon)
    {
        weapon = newWeapon;
        if (activeParticleSystem)
        {
            Destroy(activeParticleSystem.gameObject);
        }
        activeParticleSystem = Instantiate(weapon.onFiredParticleSystem, particleSystemHolder);
    }

    public void RemoveCurrentWeapon()
    {
        weapon = null;
        if (activeParticleSystem)
        {
            Destroy(activeParticleSystem.gameObject);
        }
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
            if (weapon.FireProjectile(transform.position, direction, lastFired, data))
            {
                activeParticleSystem.Play();
                audioPlayer.PlayOneShot(weapon.onFiredClip);
                lastFired = Time.time;
            }
            if (weapon.CheckIfBroke())
            {
                RemoveCurrentWeapon();
            }
        }

    }

    public void OnDrawGizmos()
    {
        if(weapon != null)
            weapon.OnDrawGizmos();
    }

}
