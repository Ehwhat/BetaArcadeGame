using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTankProjectile : IProjectile
{
    public Vector3 position;
    public Vector3 direction;

    public TestTankWeapon.TestTankWeaponData data;
    GameObject representation;
    System.Action<TestTankProjectile> finished;

    public void OnFired(Vector3 firedPosition, Vector3 firedDirection, object firedData = null)
    {
        data = (TestTankWeapon.TestTankWeaponData)firedData;
        position = firedPosition;
        direction = firedDirection;

        representation = GameObject.Instantiate(data.representationPrefab, position, Quaternion.identity);
        finished = data.finishedCallback;
    }

    public void Update(float deltaTime)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, 10 * deltaTime, data.bulletCollisionMask);
        if (hit)
        {
            position = hit.point;
            representation.transform.position = position;

            IDamageable hitDamageable = hit.collider.GetComponent<IDamageable>();
            if (hitDamageable != null)
            {
                ProjectileHit hitData = new ProjectileHit()
                {
                    hitData = hit,
                    projectile = this,
                    damage = 10,
                };
                hitDamageable.OnHit(hitData);
            }

            finished(this);
            GameObject.Destroy(representation, 2);
        }
        else
        {
            position += direction * 10 * deltaTime;
            representation.transform.position = position;
        }
        
    }
}
