using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject {

    public string displayName;
    public virtual void OnWeaponEquipted() { }
    public virtual void OnWeaponUnequipted() { }

    public virtual void OnDrawGizmos()
    {

    }

}
