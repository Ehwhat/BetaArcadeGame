using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public GameObject hazard;
    void HazardDetection()
    {
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(hazard.transform.position, 1.0f, 1 << 8);

        if(hitCollider.Length != 0)
        {
            for (int i = 0; i < hitCollider.Length; i++)
            {
                TankManager tank = hitCollider[i].transform.root.GetComponent<TankManager>();
                if (tank != null)
                {
                    
                }
            }
        }
    }

    private void Update()
    {
        HazardDetection();
    }
}
