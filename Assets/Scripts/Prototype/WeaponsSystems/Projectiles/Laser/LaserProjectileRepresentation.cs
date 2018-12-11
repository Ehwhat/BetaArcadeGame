using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectileRepresentation : ProjectileRepresentation {

    public LineRenderer lineRenderer;
    public float alphaModifer = 1;
    public float targetAlpha = 1;

    private void Start()
    {
        SetLineAlpha(targetAlpha);
    }

    private void Update()
    {
        SetLineAlpha(targetAlpha);
    }

    private void SetLineAlpha(float alpha)
    {
        lineRenderer.startColor = new Color(customColour.r, customColour.g, customColour.b, alpha * alphaModifer);
        lineRenderer.endColor = new Color(customColour.r, customColour.g, customColour.b, alpha * alphaModifer);
    }

}
