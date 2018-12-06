using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserChargingVisualisation : WeaponChargingVisualisation {

    public LineRenderer laserLineRenderer;
    public float width = 0.6f;
    public float alpha = 0.4f;

    public override void SetColour(Color colour)
    {
        colour = new Color(colour.r, colour.g, colour.b, alpha);
        laserLineRenderer.startColor = colour;
        laserLineRenderer.endColor = colour;
    }

    public override void OnCharge(float percent)
    {
        laserLineRenderer.widthMultiplier = width * percent;
        laserLineRenderer.SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1000, 0) });
    }

}
