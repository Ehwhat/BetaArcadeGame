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
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(barrelLineRendeer.transform.position, barrelLineRendeer.transform.up,float.MaxValue, aimLayer);
        float distance = float.PositiveInfinity;
        float biggestDistance = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider.transform.root != transform.root)
            {
                if(hits[i].distance > biggestDistance)
                {
                    biggestDistance = hits[i].distance;
                    distance = biggestDistance;
                }
            }
        }
        barrelLineRendeer.positionCount = 2;
        barrelLineRendeer.SetPosition(1, Vector2.up * Mathf.Min(distance, solidLength));
        if (distance > solidLength)
        {
            barrelLineRendeer.positionCount = 3;
            barrelLineRendeer.SetPosition(2, Vector2.up * Mathf.Min(distance, fullLineLength));
        }
    }
}
