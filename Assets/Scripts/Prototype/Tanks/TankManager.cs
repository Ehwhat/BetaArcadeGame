using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankManager : MonoBehaviour, IDamageable {

    public System.Action<TankManager, DamageData> onDeath = (tank, weaponHit) => { };

    public int tankID;
    public string tankDisplayName;

    public TankController controller;
    public Rigidbody2D tankRigidbody;
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
        SetTurretOwners();
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
                gameObject.SetActive(false);
                SpawnDeathParticles();
            }
        }
    }

    private void SpawnDeathParticles()
    {
        ParticleSystem spawnedParticles =  Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
        spawnedParticles.Play();
        Destroy(spawnedParticles.gameObject, spawnedParticles.main.duration + 1);
    }

    public void SetTurretOwners()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].SetWeaponHolderOwner(this);
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
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
