using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTankProjectileInstance: TankProjectileInstance
{
    public TankProjectileData data;
    public ProjectileRepresentation representation;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Basic Tank Projectile", fileName = "New Basic Tank Projectile")]
public class BasicTankProjectile : RepresentedTankProjectile<BasicTankProjectileInstance, TankProjectileData> {

    public float projectileSpeed = 10;

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, BasicTankProjectileInstance instance, TankProjectileData data)
    {
        instance.position = firedPosition;
        instance.direction = firedDirection;
        instance.data = data;
        instance.representation = Instantiate(projectileRepresentation, firedPosition, Quaternion.identity);
        instance.representation.OnSpawn(firedPosition, firedDirection);
    }

    public override void UpdateProjectile(float deltaTime, BasicTankProjectileInstance instance)
    {
        RaycastHit2D hit = Physics2D.Raycast(instance.position, instance.direction, projectileSpeed * deltaTime, projectileLayerMask);
        if (hit)
        {
            instance.position = hit.point;
            instance.representation.transform.position = instance.position;

            AttemptToDamage(hit.collider, hit);

            instance.representation.transform.rotation = Quaternion.FromToRotation(Vector2.up, instance.direction);
            instance.representation.Destroy();
            instance.finishedCallback();
        }
        else
        {
            instance.position += instance.direction * projectileSpeed * deltaTime;
            instance.representation.transform.position = instance.position;
        }
    }

}
