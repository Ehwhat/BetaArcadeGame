using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestTankWeapon", menuName = "Tanks/Weapons/Test/TestTankWeapon")]
public class TestTankWeapon : Weapon
{
    public struct TestTankWeaponData
    {
        public GameObject representationPrefab;
        public LayerMask bulletCollisionMask;
        public WeaponHolder holder;
        public System.Action<TestTankProjectile> finishedCallback;
    }

    public GameObject prefab;
    public LayerMask bulletCollisionMask;
    public float firingDelay = 0.5f;

    public AudioClip firingSound;

    private List<TestTankProjectile> projectiles = new List<TestTankProjectile>();

    public override bool Fire(WeaponFiringPoint[] firingPoints, WeaponHolder holder)
    {
        if (holder.lastFired < Time.time-firingDelay)
        {
            TestTankWeaponData data = new TestTankWeaponData()
            {
                representationPrefab = prefab,
                finishedCallback = OnProjectileFinished,
                bulletCollisionMask = bulletCollisionMask,
                holder = holder
            };

            TestTankProjectile projectile = new TestTankProjectile();
            projectile.OnFired(firingPoints[0].position, firingPoints[0].direction, data);
            projectiles.Add(projectile);

            holder.owningRigidbody.AddForce(-firingPoints[0].direction*5, ForceMode2D.Impulse);
            holder.audioSource.pitch = Random.Range(0.7f, 1.8f);
            holder.audioSource.PlayOneShot(firingSound);

            return true;
        }
        return false;

    }

    public override void UpdateProjectiles(float deltaTime, WeaponHolder holder)
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].Update(deltaTime);
        }
        
    }

    public void OnProjectileFinished(TestTankProjectile projectile)
    {
        projectiles.Remove(projectile);
    }
}
