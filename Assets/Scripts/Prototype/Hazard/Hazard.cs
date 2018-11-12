using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float damage = 10;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        TankManager tank = collision.transform.root.GetComponent<TankManager>();
        if (tank != null)
        {
            Vector2 direction = (gameObject.transform.position - tank.transform.position).normalized;
            DamageData data = new DamageData(damage, direction, tank.transform.position, collision);
            tank.OnHit(data);
            OnTankHit(tank);
        }
    }

    public virtual void OnTankHit(TankManager tank) { }
}
