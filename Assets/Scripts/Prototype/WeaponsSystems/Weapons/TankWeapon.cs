using System.Collections;
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

    public GameObject projectileRepresentation;
    public LayerMask projectileLayerMask;

    public AudioClip onFiredClip;
    public AudioClip onHitClip;

    public ParticleSystem onFiredParticleSystem;

    private float duribility = 0;

    public void ResetDurability()
    {
        duribility = maxDurability;
    }

    public virtual bool FireProjectile(Vector2 position, Vector2 direction, float lastFiredTime, TankProjectileData firingData = null)
    {
        if (lastFiredTime + firingDelay < Time.time && !CheckIfBroke())
        {
            for (int i = 0; i < weaponFiringOffsets.Length; i++)
            {
                firingData.ownerWeapon = this;
                firingData.projectileLayerMask = projectileLayerMask;
                firingData.projectileRepresentation = projectileRepresentation;
                Vector2 offset = firingData.ownerWeaponHolder.transform.rotation * weaponFiringOffsets[i];
                projectile.OnFired(position+ offset, direction, firingData);
                duribility -= perShotDuribilityCost;
                return true;
            }
        }
        return false;
    }

    public virtual bool CheckIfBroke()
    {
        return (duribility <= 0 && useDuribility);
    }

}
