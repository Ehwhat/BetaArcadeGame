﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestRaycastWeapon", menuName = "Tanks/Weapons/Test/TestRaycastWeapons")]
public class TestRaycastWeapon : Weapon
{
    public override void Fire(WeaponFiringPoint[] firingPoints, object firingData)
    {
        for (int i = 0; i < firingPoints.Length; i++)
        {
            TestRaycastProjectile projectile = new TestRaycastProjectile();
            projectile.OnFired(firingPoints[i].position, firingPoints[i].direction);
        }
    }
}