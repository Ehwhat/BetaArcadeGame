using System.Collections;
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
    private WeaponChargingVisualisation weaponChargingVisualisation;

    private TankWeapon lastWeapon;

    public float lastFired = 0;
    public float currentDurability = 0;
    public float chargeUpElapsedTime;
    public bool equiptOnStart;

    public void Start()
    {
    }

    private void Update()
    {
        if(weapon != lastWeapon)
        {
            SetWeapon(weapon);
        }
    }

    public void SetWeapon(TankWeapon newWeapon)
    {
        RemoveCurrentWeapon();
        if (!newWeapon)
            return;
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
        chargeUpElapsedTime = 0;
        currentDurability = weapon.maxDurability;

        if (ownerTank is PlayerTankManager) {
            PlayerTankData data = (ownerTank as PlayerTankManager).data;
            Color colour = data.playerColour;
            Color colourEnd = new Color(colour.r, colour.g, colour.b, 0);
            BarrelAimingIndicator[] barrelAimingIndicators = currentVisualisation.GetComponentsInChildren<BarrelAimingIndicator>();
            for (int i = 0; i < barrelAimingIndicators.Length; i++)
            {
                barrelAimingIndicators[i].barrelLineRendeer.startColor = colour;
                barrelAimingIndicators[i].barrelLineRendeer.endColor = colourEnd;
            }

        }
        lastWeapon = weapon;
        weapon.OnWeaponEquipted(this);
    }

    public WeaponVisualisation GetCurrentWeaponVisualisation()
    {
        return currentVisualisation;
    }

    public void RemoveCurrentWeapon()
    {
        if (weapon)
        {
            weapon.OnWeaponUnequipted(this);
        }
        weapon = null;
        if (activeParticleSystem)
        {
            Destroy(activeParticleSystem.gameObject);
        }
        if (currentVisualisation)
        {
            Destroy(currentVisualisation.gameObject);
        }
        chargeUpElapsedTime = 0;
    }

    public void EquipDefaultWeapon()
    {
        SetWeapon(defaultWeapon);
        Sprite s = ownerTank.tankDefinition.tankTurret.sprite;
        currentVisualisation.GetComponentInChildren<SpriteRenderer>().sprite = s;
    }

    public virtual void OnFiringDown(Vector2 direction)
    {
        if(weapon != null && gameObject.activeInHierarchy)
            weapon.OnFiringDown(transform.position, direction, this);
    }

    public virtual void OnFiring(Vector2 direction)
    {
        if (weapon != null && gameObject.activeInHierarchy)
            weapon.OnFiringHold(transform.position, direction, this);
    }

    public virtual void OnFiringUp(Vector2 direction)
    {
        if (weapon != null && gameObject.activeInHierarchy)
            weapon.OnFiringUp(transform.position, direction, this);
    }

    public void OnWeaponFired()
    {
        chargeUpElapsedTime = 0;
        if (activeParticleSystem)
        {
            activeParticleSystem.Play();
        }
        if (weapon.onFiredClip)
        {
            AudioPlayer.PlayOneOff(weapon.onFiredClip);
        }
        lastFired = Time.time;
        if (weapon.useDuribility)
        {
            currentDurability -= weapon.perShotDuribilityCost;
            if (currentDurability <= 0)
            {
                SetWeapon(defaultWeapon);
            }
        }
    }

    public void OnDrawGizmos()
    {
        if(weapon != null)
            weapon.OnDrawGizmos();
    }



}
