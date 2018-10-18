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
    public Rigidbody2D owningRigidbody;
    public AudioSource audioSource;
    public ParticleSystem particleEffect;

    public float lastFired = 0;

    // Use this for initialization
    void Start () {
        weapon.OnWeaponActive();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        UpdateWeapon(Time.deltaTime);
	}

    [ContextMenu("Fire")]
    public virtual void FireWeapon()
    {
        if(weapon.Fire(GetPoints(), this))
        {
            lastFired = Time.time;
            if (particleEffect)
            {
                particleEffect.Emit(10);
            }
        }
        
    }

    public virtual void UpdateWeapon(float deltaTime)
    {
        weapon.UpdateProjectiles(deltaTime, this);
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

    private void OnDrawGizmos()
    {
        WeaponFiringPoint[] points = GetPoints();
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawWireSphere(points[i].position, 0.1f);
        }
        
    }
}
