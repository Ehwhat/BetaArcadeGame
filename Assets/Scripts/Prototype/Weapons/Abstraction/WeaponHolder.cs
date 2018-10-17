using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public struct WeaponFiringPoint
{
    public Vector2 position;
    public Vector2 direction;
    public WeaponFiringPoint(Vector2 position, Vector2 direction)
    {
        this.position = position;
        this.direction = direction;
    }
}

public class WeaponHolder : MonoBehaviour {

    public Vector2[] firingPoints;
    public Weapon weapon;
    public Transform target;

    // Use this for initialization
    void Start () {
        weapon.OnWeaponActive();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            FireWeapon(new TestArgs() { target = target });
        }
        UpdateWeapon(Time.deltaTime, null);
	}

    [ContextMenu("Fire")]
    public void FireWeapon(object firingData = null)
    {
        weapon.Fire(GetPoints(), firingData);
    }

    public void UpdateWeapon(float deltaTime,object firingData = null)
    {
        weapon.UpdateProjectiles(deltaTime,firingData);
    }

    public WeaponFiringPoint[] GetPoints()
    {
        WeaponFiringPoint[] points = new WeaponFiringPoint[firingPoints.Length];
        for (int i = 0; i < firingPoints.Length; i++)
        {
            Vector2 worldPoint = transform.TransformPoint(firingPoints[i]);
            points[i] = new WeaponFiringPoint(worldPoint, transform.up);
        }
        return points;
    }
}
