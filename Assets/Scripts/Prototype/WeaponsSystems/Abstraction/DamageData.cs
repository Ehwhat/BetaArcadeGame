using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageData {

    public float damage;
    public Vector2 normal;
    public Vector2 point;

    public DamageData(float damage, Vector2 normal, Vector2 point)
    {
        this.damage = damage;
        this.normal = normal;
        this.point = point;
    }

}

public class TankProjectileDamageData : DamageData
{
    public RaycastHit2D raycastHit;
    public TankProjectile projectile;
    public Vector2 force;
    public TankProjectileInstance projectileInstance;

    public TankProjectileDamageData(float damage, RaycastHit2D hit, TankProjectile projectile, TankProjectileInstance instance, Vector2 force = default(Vector2)) : base(damage, hit.normal, hit.point)
    {
        this.raycastHit = hit;
        this.projectile = projectile;
        this.projectileInstance = instance;
        this.force = force;
    }

}
