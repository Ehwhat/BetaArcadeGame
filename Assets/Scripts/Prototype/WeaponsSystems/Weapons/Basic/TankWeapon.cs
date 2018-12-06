using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    public TankManager ownerTank;
    public TankWeapon weapon;
    public TankWeaponHolder holder;
    public Color shotCustomColour;
    public bool useCustomColour = false;
}

[System.Serializable]
public struct WeaponFiringOffset
{
    public Vector2 position;
    public float angle;
    public WeaponFiringOffset(Vector2 position, float angle)
    {
        this.position = position;
        this.angle = angle;
    }
}


[CreateAssetMenu(menuName = "Tanks/Weapons/New Tank Weapon", fileName = "New Tank Weapon")]
public class TankWeapon : Weapon
{
    [Header("Weapon Settings")]
    [Space(5)]
    [Header("Firing Projectile")]
    public TankProjectile projectile;

    [Space(10)]
    [Header("Weapon Firing Settings")]
    public WeaponFiringOffset[] weaponFiringOffsets = new WeaponFiringOffset[] { new WeaponFiringOffset(Vector3.zero,0) };

    public float firingDelay;
    public bool useDuribility = true;
    public float maxDurability = 100;
    public float perShotDuribilityCost = 10;
    [Range(0,1)]
    public float rarity = 0.5f;
    public float shakeAmount = 1;
    public float chargeUpDelay = 1;

    public LayerMask CollideOnSpawnLayermask;

    public WeaponVisualisation weaponVisualisation;
    public WeaponChargingVisualisation weaponChargingVisualisation;

    public AudioObject onFiredClip;
    public AudioClip onHitClip;

    public ParticleSystem onFiredParticleSystem;

    public virtual void OnFiringDown(Vector2 position, Vector2 direction, TankWeaponHolder holder)
    {
        holder.chargeUpElapsedTime = 0;
    }

    public virtual void OnFiringHold(Vector2 position, Vector2 direction, TankWeaponHolder holder)
    {
        if (holder.lastFired + firingDelay < Time.time)
        {
            if (holder.chargeUpElapsedTime < chargeUpDelay)
            {
                OnCharge(position, direction, holder, holder.chargeUpElapsedTime / chargeUpDelay);
                holder.chargeUpElapsedTime += Time.deltaTime;
            }
            else
            {
                FireProjectile(position, direction, holder);
                holder.OnWeaponFired();
                holder.chargeUpElapsedTime = 0;
            }
        }
        
    }

    public virtual void OnWeaponEquipted(TankWeaponHolder holder) { }
    public virtual void OnWeaponUnequipted(TankWeaponHolder holder) { }

    public virtual void OnFiringUp(Vector2 position, Vector2 direction, TankWeaponHolder holder)
    {
        holder.chargeUpElapsedTime = 0;
        OnCharge(position, direction, holder, holder.chargeUpElapsedTime / chargeUpDelay);
    }

    protected virtual bool OnCharge(Vector2 position, Vector2 direction, TankWeaponHolder holder, float chargeAmount)
    {
        bool canCharge = false;
        for (int i = 0; i < weaponFiringOffsets.Length; i++)
        {
            Quaternion rotation = holder.transform.rotation;
            Vector2 offsetPosition = position + (Vector2)(rotation * weaponFiringOffsets[i].position);
            Vector2 offsetDirection = Quaternion.Euler(0, 0, weaponFiringOffsets[i].angle) * direction;

            Collider2D overlapCollider = Physics2D.OverlapPoint(offsetPosition, CollideOnSpawnLayermask);
            if (overlapCollider)
                continue;

            WeaponData weaponData = new WeaponData() { ownerTank = holder.ownerTank, weapon = this, holder = holder };
            projectile.OnCharge(position, direction, weaponData, chargeAmount);
            canCharge = true;
        }
        return canCharge;
    }

    protected virtual bool FireProjectile(Vector2 position, Vector2 direction, TankWeaponHolder holder)
    {
        bool wasFired = false;
        for (int i = 0; i < weaponFiringOffsets.Length; i++)
        {
            Quaternion rotation = holder.transform.rotation;
            Vector2 offsetPosition = position + (Vector2)(rotation * weaponFiringOffsets[i].position);
            Vector2 offsetDirection = Quaternion.Euler(0, 0, weaponFiringOffsets[i].angle) * direction;

            wasFired = true;

            Collider2D overlapCollider = Physics2D.OverlapPoint(offsetPosition, CollideOnSpawnLayermask);
            if (overlapCollider)
                continue;
            

            WeaponData weaponData = new WeaponData() { ownerTank = holder.ownerTank, weapon = this, holder = holder };
            if(holder.ownerTank is PlayerTankManager)
            {
                weaponData.useCustomColour = true;
                weaponData.shotCustomColour = (holder.ownerTank as PlayerTankManager).data.playerColour;
            }

            projectile.OnFired(offsetPosition, offsetDirection, weaponData);
            
            
        }
        CameraShake.AddShake(shakeAmount, Quaternion.identity); // Sorry jordan, I broke this and I have no idea what the value should be :(
        return wasFired;
    }

    public float GetRateOfFire()
    {
        return (1f / firingDelay);
    }

    public float CalculateWeaponCoefficent()
    {
        if(projectile)
            return (((projectile.damage * weaponFiringOffsets.Length) * (GetRateOfFire()*60) * (maxDurability/perShotDuribilityCost)) / ((1-rarity)*100))/100;
        return 0;
    }

}
