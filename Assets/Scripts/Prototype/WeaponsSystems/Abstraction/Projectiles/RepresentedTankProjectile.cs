using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepresentedTankProjectile<ProjectileInstance> : TankProjectile<ProjectileInstance> where ProjectileInstance : TankProjectileInstance, new()
{

    public ProjectileRepresentation projectileRepresentation;

}
