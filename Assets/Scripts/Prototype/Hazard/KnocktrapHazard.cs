using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnocktrapHazard : Hazard {

    public override void OnTankHit(TankManager tank, Collider2D collider, Vector3 point)
    {
        Vector2 direction = (gameObject.transform.position - tank.transform.position).normalized;
        tank.GetComponent<Rigidbody2D>().AddForce(-direction * 100, ForceMode2D.Impulse);
    }

}
