using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicTankBuildupProjectileInstance : BasicTankProjectileInstance
{
    public float velocity = 0;
}

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Basic Tank Projectile Wobbly", fileName = "New Basic Tank Projectile Wobbly")]
public class BasicTankProjectileWobbly : RepresentedTankProjectile<BasicTankBuildupProjectileInstance> {

    public float startSpeed = 10;
    public float maxVelocity = 100;
    public float acceration = 50;

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, BasicTankBuildupProjectileInstance instance, WeaponData data)
    {
        instance.position = firedPosition;
        instance.direction = firedDirection;
        instance.velocity = startSpeed;
        instance.representation = Instantiate(projectileRepresentation, firedPosition, Quaternion.identity);
    }

    public override void UpdateProjectile(float deltaTime, BasicTankBuildupProjectileInstance instance)
    {
        float currentSpeed = instance.velocity;
        RaycastHit2D hit = Physics2D.Raycast(instance.position, instance.direction, currentSpeed * deltaTime, projectileLayerMask);
        if (hit)
        {
            instance.position = hit.point;
            instance.representation.transform.position = instance.position;

            IDamageable hitDamageable = hit.collider.transform.root.GetComponent<IDamageable>();
            if (hitDamageable != null)
            {
                ProjectileHit hitData = new ProjectileHit()
                {
                    hitData = hit,
                    projectile = this,
                    holder = instance.weaponData.holder,
                    damage = damage
                };
                hitDamageable.OnHit(hitData);
            }

            instance.representation.transform.rotation = Quaternion.FromToRotation(Vector2.up, instance.direction);
            instance.representation.Destroy();
            instance.finishedCallback();
        }
        else
        {
            instance.velocity = Mathf.Clamp(instance.velocity + (acceration * instance.velocity * deltaTime), 0, maxVelocity);
            instance.position += instance.direction * currentSpeed * deltaTime;
            instance.direction = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 30)) * instance.direction;
            instance.representation.transform.position = instance.position;
        }
    }

}
