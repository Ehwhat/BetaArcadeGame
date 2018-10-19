using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Weapons/New Tank Weapon", fileName = "New Tank Weapon")]
public class TankWeapon : Weapon
{
    public TankProjectile projectile;

    public Vector2[] weaponFiringOffsets = new Vector2[] { Vector2.zero };

    public float firingDelay;
    public GameObject projectileRepresentation;
    public LayerMask projectileLayerMask;

    public AudioClip onFiredClip;
    public AudioClip onHitClip;

    public ParticleSystem onFiredParticleSystem;

    public virtual bool FireProjectile(Vector2 position, Vector2 direction, float lastFiredTime, TankProjectileData firingData = null)
    {
        if (lastFiredTime + firingDelay < Time.time)
        {
            for (int i = 0; i < weaponFiringOffsets.Length; i++)
            {
                firingData.ownerWeapon = this;
                firingData.projectileLayerMask = projectileLayerMask;
                firingData.projectileRepresentation = projectileRepresentation;
                Vector2 offset = firingData.ownerWeaponHolder.transform.rotation * weaponFiringOffsets[i];
                projectile.OnFired(position+ offset, direction, firingData);
                return true;
            }
        }
        return false;
    }

}
