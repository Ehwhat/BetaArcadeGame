using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileRepresentation), typeof(TrailRenderer))]
public class BulletChangeColour : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GetComponent<ProjectileRepresentation>().onCustomColourAdded += OnCustomColour;  
	}

    private void OnCustomColour(Color obj)
    {
        GetComponent<TrailRenderer>().startColor = obj;
        GetComponent<TrailRenderer>().endColor = new Color(obj.r, obj.g, obj.b, 0);
        var main = GetComponent<ParticleSystem>().main;
        main.startColor = obj;
    }
}
