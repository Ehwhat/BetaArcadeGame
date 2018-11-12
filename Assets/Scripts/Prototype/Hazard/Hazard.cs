using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float damage = 10;

        //if(hitCollider.Length != 0)
        //{
        //    for (int i = 0; i < hitCollider.Length; i++)
        //    {
                
        //    }
        //}
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        TankManager tank = collision.transform.root.GetComponent<TankManager>();
        if (tank != null)
        {
            Vector2 direction = (hazard.transform.position - tank.transform.position).normalized;
            DamageData data = new DamageData(damage, direction, tank.transform.position);
            tank.OnHit(data);
        }
    }

    public virtual void OnTankHit(TankManager tank) { }

    private void Update()
    {
        HazardDetection();
    }
}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        TankManager tank = collision.transform.root.GetComponent<TankManager>();
            Vector2 direction = (gameObject.transform.position - tank.transform.position).normalized;
            DamageData data = new DamageData(damage, direction, tank.transform.position);

    public virtual void OnTankHit(TankManager tank) { }