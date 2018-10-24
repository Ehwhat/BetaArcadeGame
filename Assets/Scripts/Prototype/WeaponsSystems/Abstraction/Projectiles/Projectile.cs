using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile {

    void OnFired(Vector3 firedPosition, Vector3 firedDirection, object firedData = null);

}
