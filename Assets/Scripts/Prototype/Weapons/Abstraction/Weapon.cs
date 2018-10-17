using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public abstract class Weapon : ScriptableObject {

    public virtual void OnWeaponActive() { }
    public abstract bool Fire(WeaponFiringPoint[] firingPoints, WeaponHolder holder);
    public virtual void UpdateProjectiles(float deltaTime, WeaponHolder holder) { }
}
