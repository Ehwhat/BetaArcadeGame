using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReflectingTankProjectileInstance : BasicTankProjectileInstance
{
    public int reflectionCount = 0;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Reflecting Tank Projectile", fileName = "New Reflecting Tank Projectile")]
public class ReflectingTankProjectile : RepresentedTankProjectile<ReflectingTankProjectileInstance>
{
    public float projectileSpeed = 10;
    public int reflectionLimit = 1;

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, ReflectingTankProjectileInstance instance, WeaponData data)
    {
        instance.position = firedPosition;
        instance.direction = firedDirection;
        instance.representation = Instantiate(projectileRepresentation, firedPosition, Quaternion.identity);
        instance.representation.OnSpawn(firedPosition, firedDirection);
    }

    public override void UpdateProjectile(float deltaTime, ReflectingTankProjectileInstance instance)
    {
        RaycastHit2D hit = Physics2D.Raycast(instance.position, instance.direction, projectileSpeed * deltaTime, projectileLayerMask);
        if (hit)
        {
            instance.position = hit.point;
            instance.representation.transform.position = instance.position;
            instance.direction = Vector2.Reflect(instance.direction, hit.normal);

            instance.reflectionCount++;

            if (instance.reflectionCount > reflectionLimit)
            {
                AttemptToDamage(hit.collider, hit, instance);

                instance.representation.transform.rotation = Quaternion.FromToRotation(Vector2.up, instance.direction);
                instance.representation.Destroy();
                instance.finishedCallback();
            }
        }
        else
        {
            instance.position += instance.direction * projectileSpeed * deltaTime;
            instance.representation.transform.position = instance.position;
        }
    }

}
