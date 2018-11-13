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
    public float radius = 0.5f;

    public override void UpdateProjectile(float deltaTime, ReflectingTankProjectileInstance instance)
    {
        base.UpdateProjectile(deltaTime, instance);
    }

    protected override void OnProjectileHit(ReflectingTankProjectileInstance instance, float deltaTime, RaycastHit2D hit)
    {
        instance.position += instance.direction * radius;
        instance.representation.transform.position = instance.position;

        if (instance.reflectionCount > reflectionLimit || damageableLayerMask == (damageableLayerMask | (1 << hit.collider.gameObject.layer)) && destroyOnDamagableHit)
        {
            AttemptToDamage(hit.collider, hit, instance);

            instance.representation.transform.rotation = Quaternion.FromToRotation(Vector2.up, -hit.normal);
            instance.representation.Destroy();
            instance.finishedCallback();
            return;
        }

        instance.direction = Vector2.Reflect(instance.direction, hit.normal);

        instance.reflectionCount++;
        instance.canHitSelf = true;

        
    }

    protected override bool DoHitTest(ReflectingTankProjectileInstance instance, float deltaTime, out RaycastHit2D hit)
    {
        hit = Physics2D.CircleCast(instance.position, radius, instance.direction, projectileSpeed * deltaTime, projectileLayerMask);
        return hit && IsColliderWeaponHolder(hit.collider, instance);
    }

}
