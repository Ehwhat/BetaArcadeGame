using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepresentedTankProjectile<ProjectileInstance, ProjectileFiredDataType> : TankProjectile<ProjectileInstance, ProjectileFiredDataType> where ProjectileFiredDataType : TankProjectileData where ProjectileInstance : TankProjectileInstance, new()
{

    public ProjectileRepresentation projectileRepresentation;

}
