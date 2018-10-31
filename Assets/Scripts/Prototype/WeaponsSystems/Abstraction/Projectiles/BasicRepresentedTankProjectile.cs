using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTankProjectileInstance : TankProjectileInstance
{
    public ProjectileRepresentation representation;
}

public abstract class BasicRepresentedTankProjectile<ProjectileInstance> : RepresentedTankProjectile<ProjectileInstance> where ProjectileInstance : BasicTankProjectileInstance, new()
{
    public float projectileSpeed = 10;

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, ProjectileInstance instance, WeaponData weaponData)
    {
        instance.position = firedPosition;
        instance.direction = firedDirection;
        instance.representation = Instantiate(projectileRepresentation, firedPosition, Quaternion.identity);
        instance.representation.OnSpawn(firedPosition, firedDirection);
    }

    public override void UpdateProjectile(float deltaTime, ProjectileInstance instance)
    {
        RaycastHit2D hit;
        if (DoHitTest(instance, deltaTime, out hit))
        {
            OnProjectileHit(instance, deltaTime, hit);
        }
        else
        {
            OnProjectileNonHit(instance, deltaTime);
        }
    }

    protected virtual void OnProjectileHit(ProjectileInstance instance, float deltaTime, RaycastHit2D hit)
    {
        instance.position = hit.point;
        instance.representation.transform.position = instance.position;

        AttemptToDamage(hit.collider, hit, instance);

        instance.representation.transform.rotation = Quaternion.FromToRotation(Vector2.up, instance.direction);
        instance.representation.Destroy();
        instance.finishedCallback();
    }

    protected virtual void OnProjectileNonHit(ProjectileInstance instance, float deltaTime)
    {
        instance.position += instance.direction * projectileSpeed * deltaTime;
        instance.representation.transform.position = instance.position;
    }

    protected virtual bool DoHitTest(ProjectileInstance instance, float deltaTime, out RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(instance.position, instance.direction, projectileSpeed * deltaTime, projectileLayerMask);
        return hit && hit.collider.transform.root != instance.weaponData.holder.transform.root;
    }

}
