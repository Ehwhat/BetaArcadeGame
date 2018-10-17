using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankManager : MonoBehaviour, IDamageable {

    public TankController controller;
    public TankMovement tankMovement;
    public TrailRenderer[] trails;
    public TurretController[] turrets;

    public float maxHealth = 100;

    private float health;

    private TankController _internalController;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        Process();
    }

    public virtual void Process()
    {
        controller.Process(this);
    }

    public void ClearTrails()
    {
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Clear();
        }
    }

    public void OnHit(ProjectileHit hit)
    {
        health -= hit.damage;
    }

    public void AimTurrets(Vector2 direction)
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].targetVector = direction;
        }
    }

    public void FireTurrets()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].Fire();
        }
    }
}
