using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestTankWeapon", menuName = "Tanks/Weapons/Test/TestTankWeapon")]
public class TestTankWeapon : Weapon
{
    public struct TestTankWeaponData
    {
        public GameObject representationPrefab;
        public System.Action<TestTankProjectile> finishedCallback;
    }

    public GameObject prefab;
    private List<TestTankProjectile> projectiles = new List<TestTankProjectile>();

    public override void Fire(WeaponFiringPoint[] firingPoints, object firingData)
    {
        TestTankWeaponData data = new TestTankWeaponData()
        {
            representationPrefab = prefab,
            finishedCallback = OnProjectileFinished
        };


        TestTankProjectile projectile = new TestTankProjectile();
        projectile.OnFired(firingPoints[0].position, firingPoints[0].direction, data);
        projectiles.Add(projectile);
    }

    public override void UpdateProjectiles(float deltaTime, object firingData)
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
