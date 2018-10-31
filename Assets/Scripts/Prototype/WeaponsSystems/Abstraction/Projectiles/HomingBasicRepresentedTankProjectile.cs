using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBasicTankProjectileInstance : BasicTankProjectileInstance
{
    public object target;
}

public abstract class HomingBasicRepresentedTankProjectile<ProjectileInstance> : BasicRepresentedTankProjectile<ProjectileInstance> where ProjectileInstance : HomingBasicTankProjectileInstance, new()
{
    public enum HomingTracking
    {
        None,
        Positional,
        Velocity
    }

    [Space(10)]
    [Header("Homing Properties")]
    public float maxHomingDegreesDelta;
    public HomingTracking homingTrackingType;

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, ProjectileInstance instance, WeaponData data)
    {
        Debug.Log(data);

        if (data.weapon is AutoAimTankWeapon)
        {
            Collider2D lastTarget = (data.weapon as AutoAimTankWeapon).GetLastTarget();
            if (lastTarget)
            {
                if (lastTarget.attachedRigidbody)
                {
                    instance.target = lastTarget.attachedRigidbody;
                }
                else
                {
                    instance.target = lastTarget;
                }
            }
        }
        base.OnFired(firedPosition, firedDirection, instance, data);

    }

    public override void UpdateProjectile(float deltaTime, ProjectileInstance instance)
    {
        base.UpdateProjectile(deltaTime, instance);
    }

    protected override void OnProjectileNonHit(ProjectileInstance instance, float deltaTime)
    {
        if (instance.target != null && homingTrackingType != HomingTracking.None)
        {
            Vector2 targetPosition = Vector2.zero;
            if(homingTrackingType == HomingTracking.Velocity && instance.target is Rigidbody2D)
            {
                Rigidbody2D rb = instance.target as Rigidbody2D;
                targetPosition = rb.position + rb.velocity * deltaTime;
            }
            else
            {
                targetPosition = (instance.target as Component).transform.position;
            }

            Vector2 directionToTarget = (targetPosition - (Vector2)instance.position).normalized;
            instance.direction = Vector3.RotateTowards(instance.direction, directionToTarget, maxHomingDegreesDelta * Mathf.Deg2Rad * deltaTime, 1);
        }
        base.OnProjectileNonHit(instance, deltaTime);
    }

}
