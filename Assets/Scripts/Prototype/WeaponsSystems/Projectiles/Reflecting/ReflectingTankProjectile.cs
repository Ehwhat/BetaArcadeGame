using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReflectingTankProjectileInstance : BasicTankProjectileInstance
{
    public int reflectionCount = 0;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Reflecting Tank Projectile", fileName = "New Reflecting Tank Projectile")]
public class ReflectingTankProjectile : ReflectingTankProjectile<ReflectingTankProjectileInstance> { }

public class ReflectingTankProjectile<Instance> : BasicRepresentedTankProjectile<Instance> where Instance: ReflectingTankProjectileInstance, new()
{
    public int reflectionLimit = 1;
    public bool destroyOnDamagableHit = true;
    public AudioObject onReflectClip;

    public override void UpdateProjectile(float deltaTime, Instance instance)
    {
        base.UpdateProjectile(deltaTime, instance);
    }

    protected override void OnProjectileHit(Instance instance, float deltaTime, RaycastHit2D hit)
    {
        instance.position += instance.direction;
        instance.representation.transform.position = instance.position;

        if (instance.reflectionCount > reflectionLimit || damageableLayerMask == (damageableLayerMask | (1 << hit.collider.gameObject.layer)) && destroyOnDamagableHit)
        {
            AttemptToDamage(hit.collider, hit, instance);

            instance.representation.transform.rotation = Quaternion.FromToRotation(Vector2.up, -hit.normal);
            instance.representation.Destroy(() => StoreRepresentationInstance(instance.representation));
            instance.finishedCallback();
            return;
        }

        instance.direction = Vector2.Reflect(instance.direction, hit.normal);

        AudioPlayer.PlayOneOff(onReflectClip);

        instance.reflectionCount++;
        instance.canHitSelf = true;

        
    }


}
