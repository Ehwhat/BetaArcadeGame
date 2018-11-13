using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReflectingTankProjectileInstance : BasicTankProjectileInstance
{
    public int reflectionCount = 0;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Reflecting Tank Projectile", fileName = "New Reflecting Tank Projectile")]
public class ReflectingTankProjectile : BasicRepresentedTankProjectile<ReflectingTankProjectileInstance>
{
    public int reflectionLimit = 1;
    public bool destroyOnDamagableHit = true;

    public override void UpdateProjectile(float deltaTime, ReflectingTankProjectileInstance instance)
    {
        base.UpdateProjectile(deltaTime, instance);
    }

    protected override void OnProjectileHit(ReflectingTankProjectileInstance instance, float deltaTime, RaycastHit2D hit)
    {
        instance.position = hit.point;
        instance.representation.transform.position = instance.position;
        instance.direction = Vector2.Reflect(instance.direction, hit.normal);

        instance.reflectionCount++;

        if (instance.reflectionCount > reflectionLimit || damageableLayerMask == (damageableLayerMask | (1 << hit.collider.gameObject.layer)) && destroyOnDamagableHit)
        {
            base.OnProjectileHit(instance, deltaTime, hit);
        }
    }

}
