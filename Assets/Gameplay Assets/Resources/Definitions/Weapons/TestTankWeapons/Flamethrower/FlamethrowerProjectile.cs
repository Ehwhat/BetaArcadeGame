using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerProjectileInstance : BasicTankBuildupProjectileInstance {
    public float lifetime = 0;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Flame Tank Projectile", fileName = "New Flame Tank Projectile")]
public class FlamethrowerProjectile : BasicRepresentedTankProjectile<FlamethrowerProjectileInstance> {

    public float radius = 0.5f;
    public float maxLifetime = 2f;

    protected override void OnProjectileHit(FlamethrowerProjectileInstance instance, float deltaTime, RaycastHit2D hit)
    {
        instance.position += instance.direction * projectileSpeed * deltaTime;
        instance.representation.transform.position = instance.position;
        if (!hit.collider.transform.root.GetComponent<TankManager>())
        {
            instance.representation.Destroy(() => StoreRepresentationInstance(instance.representation));
            instance.finishedCallback();
        }
        AttemptToDamage(hit.collider, hit, instance, damage * Time.deltaTime);
    }

    public override void UpdateProjectile(float deltaTime, FlamethrowerProjectileInstance instance)
    {
        base.UpdateProjectile(deltaTime, instance);

        instance.lifetime += deltaTime;

        if(instance.lifetime >= maxLifetime)
        {
            instance.representation.Destroy(() => StoreRepresentationInstance(instance.representation));
            instance.finishedCallback();
        }
    }

    protected override bool DoHitTest(FlamethrowerProjectileInstance instance, float deltaTime, out RaycastHit2D hit)
    {
        hit = Physics2D.CircleCast(instance.position, radius, instance.direction, projectileSpeed * deltaTime, projectileLayerMask);
        return hit && IsColliderWeaponHolder(hit.collider, instance);
    }

}
