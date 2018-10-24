using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelAimingIndicator : MonoBehaviour {

    public LineRenderer barrelLineRendeer;
    public LayerMask aimLayer;
    public float solidLength = 0.1f;
    public float fullLineLength = 4f;
	
	// Update is called once per frame
	void Update () {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(barrelLineRendeer.transform.position, barrelLineRendeer.transform.up,float.MaxValue, aimLayer);

        if (hit)
        {
            barrelLineRendeer.positionCount = 2;
            barrelLineRendeer.SetPosition(1, Vector2.up * Mathf.Min(hit.distance, solidLength));
            if (hit.distance > solidLength)
            {
                barrelLineRendeer.positionCount = 3;
                barrelLineRendeer.SetPosition(2, Vector2.up * Mathf.Min(hit.distance, fullLineLength));
            }
        }
	}
}
