using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankManager : MonoBehaviour, IDamageable {

    public System.Action<TankManager, DamageData> onDeath = (tank, weaponHit) => { };

    public TankController controller;
    public TankMovement tankMovement;
    public TrailRenderer[] trails;
    public TurretController[] turrets;
    public ParticleSystem deathParticles;

    public float maxHealth = 100;
    [SerializeField]
    public float health;
    public bool isDead = false;

    private TankController _internalController;

    private void Start()
    {
        Respawn();
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

    public void OnHit(DamageData hit)
    {
        if (!isDead)
        {
            health -= hit.damage;
            if (health <= 0)
            {
                isDead = true;
                onDeath(this, hit);
                deathParticles.Play();
            }
        }
    }

    public void Respawn()
    {
        health = maxHealth;
        isDead = false;
    }

    public void GiveWeapon(TankWeapon weapon)
    {
        TurretController randomTurret = turrets[Random.Range(0, turrets.Length)];
        randomTurret.GiveWeapon(weapon);
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
