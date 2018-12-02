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
    public float radius = 0.2f;
    public float projectileRange = 100;
    public float damageReduction = 5f;

    public override void OnCharge(Vector3 firedPosition, Vector3 firedDirection, WeaponData weaponData, float chargeAmount)
    {
        
    }

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, LaserTankProjectileInstance instance, WeaponData data)
    {
        instance.position = firedPosition;
        instance.direction = firedDirection;
        instance.representation = Instantiate((LaserProjectileRepresentation)projectileRepresentation, firedPosition, Quaternion.identity);
        instance.representation.OnSpawn(firedPosition, firedDirection);
        if (data.useCustomColour)
        {
            instance.representation.SetColour(data.shotCustomColour);
        }
        Fire(instance);

    }


    public override void UpdateProjectile(float deltaTime, LaserTankProjectileInstance instance)
    {
       
    }

    private void Fire(LaserTankProjectileInstance instance)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(instance.position, radius, instance.direction, projectileRange, projectileLayerMask);
        if (hits.Length > 0)
        {
            float tempDamage = damage;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.transform.root == instance.weaponData.holder.transform.root)
                    continue;
                AttemptToDamage(hit.collider, hit, instance, tempDamage);
                tempDamage -= damageReduction;
                if(tempDamage <= 0)
                {
                    break;
                }
                
            }
        }
        instance.representation.Destroy();
        instance.finishedCallback();
    }

}