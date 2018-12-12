using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Tanks/New Tank Definiton")]
public class TankDefinition : ScriptableObject {

    public TankBaseDefinition tankBase;
    public TankTurretDefinition tankTurret;
    public TankTreadsDefinition tankTreads;

}
