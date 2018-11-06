using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageData {

    public float damage;
    public Vector2 normal;
    public Vector2 point;
    public Collider2D collider;

    public DamageData(float damage, Vector2 normal, Vector2 point, Collider2D collider)
    {
        this.damage = damage;
        this.normal = normal;
        this.point = point;
        this.collider = collider;
    }

}

public class TankProjectileDamageData : DamageData
{
    public RaycastHit2D raycastHit;
    public TankProjectile projectile;
    public Vector2 force;
    public TankProjectileInstance projectileInstance;

    public TankProjectileDamageData(float damage, RaycastHit2D hit, TankProjectile projectile, TankProjectileInstance instance, Vector2 force = default(Vector2)) : base(damage, hit.normal, hit.point, hit.collider)
    {
        this.raycastHit = hit;
        this.projectile = projectile;
        this.projectileInstance = instance;
        this.force = force;
    }

}
