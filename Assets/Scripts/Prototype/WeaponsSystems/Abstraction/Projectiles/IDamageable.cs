using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileHit
{
    public RaycastHit2D hitData;
    public IProjectile projectile;
    public TankWeaponHolder holder;
    public float damage;
    public Vector3 force;
}

public interface IDamageable {

   void OnHit(ProjectileHit hit);
	
}
