using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public abstract class Weapon : ScriptableObject {

    public virtual void OnWeaponActive() { }
    public abstract void Fire(WeaponFiringPoint[] firingPoints, object firingData);
    public virtual void UpdateProjectiles(float deltaTime, object firingData) { }
}
