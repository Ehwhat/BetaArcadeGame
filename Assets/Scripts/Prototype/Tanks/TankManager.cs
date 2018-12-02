using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankManager : MonoBehaviour, IDamageable {

    public enum FiringInputType
    {
        Down,
        Held,
        Up
    }

    public System.Action<TankManager, DamageData> onDeath = (tank, weaponHit) => { };

    public int tankID;
    public string tankDisplayName;

    public Collider2D coreCollider;
    public TankController controller;
    public Rigidbody2D tankRigidbody;
    public TankMovement tankMovement;
    public TrailRenderer[] trails;
    public List<TurretController> turrets = new List<TurretController>();
    public ParticleSystem deathParticles;
    public TankArmourManager armourManager;
    public TankArmourPickupManager armourPickupManager;

    public float maxHealth = 100;
    [SerializeField]
    public float health;
    public bool isDead = false;
    public float armourMinSpeedModifer = 0.3f;
    public AnimationCurve tankSpeedModCurve = AnimationCurve.Linear(0, 0, 1, 1);


    private TankController _internalController;

    private void Start()
    {
        Respawn();
        SetTurretOwners();
        armourManager.OnPieceAdded += ChangeArmourSpeedModifer;
        armourManager.OnPieceRemoved += ChangeArmourSpeedModifer;
    }

    private void ChangeArmourSpeedModifer(TankArmourPiece piece, float percent)
    {
        tankMovement.SetSpeedModifer(Mathf.SmoothStep(1, armourMinSpeedModifer, tankSpeedModCurve.Evaluate(percent)));
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

    public virtual void OnHit(DamageData hit)
    {
        if (!isDead)
        {
            if (hit.collider == coreCollider)
            {
                health -= hit.damage;
                if (health <= 0)
                {
                    isDead = true;
                    onDeath(this, hit);
                    gameObject.SetActive(false);
                    SpawnDeathParticles();
                    armourPickupManager.EjectArmourPickups();
                    
                }
            }
            else
            {
                armourManager.ProcessDamage(hit);
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
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].SetWeaponHolderOwner(this);
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        health = maxHealth;
        isDead = false;
        tankMovement.speedModifer = 1;
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].ResetWeapon();
        }
    }

    public virtual void GiveWeapon(TankWeapon weapon)
    {
        int activeTurrets = 1;
        for (int i = 1; i < turrets.Count; i++)
        {
            if(turrets[i].gameObject.activeInHierarchy && turrets[i].weaponHolder.weapon == null)
            {
                activeTurrets++;
                turrets[i].GiveWeapon(weapon);
                return;
            }
        }
        TurretController randomTurret = turrets[UnityEngine.Random.Range(0, activeTurrets)];
        randomTurret.GiveWeapon(weapon);
    }

    public void AimTurrets(Vector2 direction)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].targetVector = direction;
        }
    }

    public void FireTurrets(FiringInputType inputType = FiringInputType.Held)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            switch (inputType)
            {
                case FiringInputType.Down:
                    turrets[i].FireDown();
                    break;
                case FiringInputType.Held:
                    turrets[i].Fire();
                    break;
                case FiringInputType.Up:
                    turrets[i].FireUp();
                    break;
                default:
                    break;
            }
        }
    }

    public void AddTurret(TurretController controller)
    {
        turrets.Add(controller);
        controller.SetWeaponHolderOwner(this);
    }

    public void RemoveTurret(TurretController controller)
    {
        turrets.Remove(controller);
    }
}
