using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserTankProjectileInstance : TankProjectileInstance
{
    public LaserProjectileRepresentation representation;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Laser Tank Projectile", fileName = "New Laser Tank Projectile")]
public class LaserTankProjectile : RepresentedTankProjectile<LaserTankProjectileInstance>
{
    public float projectileRange = 100;

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, LaserTankProjectileInstance instance, WeaponData data)
    {
        instance.position = firedPosition;
        instance.direction = firedDirection;
        instance.representation = Instantiate((LaserProjectileRepresentation)projectileRepresentation, firedPosition, Quaternion.identity);
        instance.representation.OnSpawn(firedPosition, firedDirection);

        Fire(instance);

    }


    public override void UpdateProjectile(float deltaTime, LaserTankProjectileInstance instance)
    {
       
    }

    private void Fire(LaserTankProjectileInstance instance)
    {
        RaycastHit2D hit = Physics2D.Raycast(instance.position, instance.direction, projectileRange, projectileLayerMask);
        if (hit)
        {
            instance.representation.transform.position = hit.point;

            AttemptToDamage(hit.collider, hit);
        }
        else
        {
            instance.representation.transform.position = instance.direction * projectileRange;
        }
        instance.representation.Destroy();
        instance.finishedCallback();
    }

}