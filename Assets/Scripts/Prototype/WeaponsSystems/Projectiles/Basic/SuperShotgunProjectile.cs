using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperShotgunProjectileInstance : BasicTankProjectileInstance
{
    public float livingTime = 0;
    public int iteration = 0;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/Super/New Super Shotgun Tank Projectile", fileName = "New Super Shotgun Tank Projectile")]
public class SuperShotgunProjectile : BasicRepresentedTankProjectile<SuperShotgunProjectileInstance> {

    public float splitTime = 1;
    public int iterationLimit = 3;
    public int spawnCount = 3;
    public float spawnAngle = 15;

    public override void UpdateProjectile(float deltaTime, SuperShotgunProjectileInstance instance)
    {
        base.UpdateProjectile(deltaTime, instance);

        if (instance.iteration < iterationLimit)
        {
            instance.livingTime += deltaTime;
            if (instance.livingTime >= splitTime)
            {
                float startAngle = -15 / 2;
                float anglePerSpawn = 15 / spawnCount;
                float angle = startAngle;
                for (int i = 0; i < spawnCount; i++)
                {
                    Vector3 direction = Quaternion.Euler(0, 0, angle) * instance.direction;
                    var newinstance = OnFiredReturnInstance(instance.position, direction, instance.weaponData);
                    newinstance.iteration = instance.iteration + 1;
                    angle += anglePerSpawn;
                }
                instance.representation.transform.position = instance.position;
                instance.representation.Destroy(() =>StoreRepresentationInstance(instance.representation), false);
                instance.finishedCallback();
            }
        }

    }
}
