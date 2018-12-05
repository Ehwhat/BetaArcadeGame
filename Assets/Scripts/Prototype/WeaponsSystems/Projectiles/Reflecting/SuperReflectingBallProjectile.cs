using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperReflectingBallTankProjectileInstance : ReflectingTankProjectileInstance
{
    public int iteration = 0;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/Super/New Super Reflecting Ball Tank Projectile", fileName = "New Super Reflecting Ball Tank Projectile")]
public class SuperReflectingBallProjectile : RefectingBallTankProjectile<SuperReflectingBallTankProjectileInstance>
{

    public int spawnChildHitLimit = 2;
    public int childInstanceLimit = 2;

    protected override void OnProjectileHit(SuperReflectingBallTankProjectileInstance instance, float deltaTime, RaycastHit2D hit)
    {
        base.OnProjectileHit(instance, deltaTime, hit);
        instance.direction = Quaternion.Euler(0, 0, Random.Range(-45f, 45f)) * hit.normal;
        if (instance.reflectionCount < spawnChildHitLimit && instance.iteration < childInstanceLimit)
        {
            SuperReflectingBallTankProjectileInstance newInstance = OnFiredReturnInstance(instance.position, Vector3.Reflect(instance.direction, hit.normal), instance.weaponData);
            newInstance.iteration = instance.iteration + 1;
        }

    }
}
