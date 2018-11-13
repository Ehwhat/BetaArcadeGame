using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusCollision : MonoBehaviour
{
    public float radius = 10.0f;
    public float damage = 10;

    void RadiusDetection()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, radius, 1 << 8);

        int i = 0;
        while (i < hitColliders.Length)
        {
            Debug.Log("Hit");
            i++;
        }
    }

    public virtual void OnTankHit(TankManager tank) { }

    // Update is called once per frame
    void Update ()
    {
        RadiusDetection();
	}
}
