using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Projectiles/New Basic Tank Projectile Wobbly", fileName = "New Basic Tank Projectile Wobbly")]
public class BasicTankProjectileWobbly : TankProjectile<BasicTankProjectileInstance, TankProjectileData> {

    public float projectileSpeed = 10;

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, BasicTankProjectileInstance instance, TankProjectileData data)
    {
        instance.position = firedPosition;
        instance.direction = firedDirection;
        instance.data = data;
        instance.representation = Instantiate(data.projectileRepresentation, firedPosition, Quaternion.identity);
    }

    public override void UpdateProjectile(float deltaTime, BasicTankProjectileInstance instance)
    {
        RaycastHit2D hit = Physics2D.Raycast(instance.position, instance.direction, projectileSpeed * deltaTime, instance.data.projectileLayerMask);
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
                    holder = instance.data.ownerWeaponHolder,
                    damage = 25
                };
                hitDamageable.OnHit(hitData);
            }

            instance.representation.transform.rotation = Quaternion.FromToRotation(Vector2.up, instance.direction);
            instance.representation.GetComponent<ParticleSystem>().Emit(6);

            GameObject.Destroy(instance.representation, 2);
            instance.finishedCallback();
        }
        else
        {
            instance.position += instance.direction * projectileSpeed * deltaTime;
            instance.direction = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 30)) * instance.direction;
            instance.representation.transform.position = instance.position;
        }
    }

}
