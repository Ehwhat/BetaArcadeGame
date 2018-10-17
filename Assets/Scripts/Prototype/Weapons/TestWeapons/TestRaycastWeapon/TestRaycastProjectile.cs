using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycastProjectile : IProjectile
{

    public void OnFired(Vector3 firedPosition, Vector3 firedDirection, object firedData = null)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(firedPosition, firedDirection);
        if (hit)
        {
            IDamageable hitDamageable = hit.collider.GetComponent<IDamageable>();
            if (hitDamageable != null)
            {
                hitDamageable.OnHit();
            }
        }

    }
}
