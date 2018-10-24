using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelAimingIndicator : MonoBehaviour {

    public LineRenderer barrelLineRendeer;
    public LayerMask aimLayer;
	
	// Update is called once per frame
	void Update () {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(barrelLineRendeer.transform.position, barrelLineRendeer.transform.up,float.MaxValue, aimLayer);

        if (hit)
        {
            barrelLineRendeer.SetPosition(1, transform.InverseTransformPoint(hit.point));
        }
	}
}
