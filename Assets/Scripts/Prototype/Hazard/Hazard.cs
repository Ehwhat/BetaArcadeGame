using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float damage = 10;

    public void Update()
    {
        Collider2D[] hits = new Collider2D[30];
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = LayerMask.NameToLayer("Tank");
        int hitCount = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hits);
        if(hitCount > 0)
        {
            TankManager bestHit = null;
            Collider2D bestCollider = null;
            float bestDistance = float.PositiveInfinity;
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].transform.root.GetComponent<TankManager>() != null)
                {
                    float distance = Vector2.Distance(transform.position, hits[i].transform.position);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestCollider = hits[i];
                        bestHit = hits[i].transform.root.GetComponent<TankManager>();
                    }
                }
            }
            if(bestHit != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.position - bestHit.transform.position).normalized);
                if (hit && hit.collider == bestHit)
                {
                    OnTankHit(bestHit, bestCollider, hit.point);
                }
            }
        }
    }

    public virtual void OnTankHit(TankManager tank, Collider2D collider, Vector3 point) {
        if (tank != null)
        {
            Vector2 direction = (gameObject.transform.position - tank.transform.position).normalized;
            DamageData data = new DamageData(damage, direction, tank.transform.position, collider);
            tank.OnHit(data);
        }
    }
}
