using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Reflecting Ball Tank Projectile", fileName = "New Reflecting Ball Tank Projectile")]
public class RefectingBallTankProjectile : RefectingBallTankProjectile<ReflectingTankProjectileInstance>{ }

public class RefectingBallTankProjectile<Instance> : ReflectingTankProjectile<Instance> where Instance : ReflectingTankProjectileInstance, new()
{
    public float radius = 0.5f;

    protected override bool DoHitTest(Instance instance, float deltaTime, out RaycastHit2D hit)
    {
        hit = Physics2D.CircleCast(instance.position, radius, instance.direction, projectileSpeed * deltaTime, projectileLayerMask);
        return hit && IsColliderWeaponHolder(hit.collider, instance);
    }

}
