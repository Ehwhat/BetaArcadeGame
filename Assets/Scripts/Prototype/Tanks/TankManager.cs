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
    public TankDefinition tankDefinition;
    public TankController controller;
    public TankSprite tankSprite;
    public TankMovement tankMovement;
    public TankArmourManager armourManager;
    public TankArmourPickupManager armourPickupManager;

    public Collider2D coreCollider;
    public Rigidbody2D tankRigidbody;
    
    public TrailRenderer[] trails;
    public List<TurretController> turrets = new List<TurretController>();
    public ParticleSystem deathParticles;
    public ParticleSystem onHitParticleSystem;

    public GameObject[] disableOnDeath;
    public RespawnSpriteController respawnSpriteController;

    public float maxHealth = 100;
    [SerializeField]
    public float health;
    public bool isDead = false;
    public float armourMinSpeedModifer = 0.3f;
    public AnimationCurve tankSpeedModCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float respawnTime = 1;
    private float elapsedDeathTime = 0;

    private System.Func<Vector3> GetRespawnLocation;
    private Vector3 latestRespawnLocation;
    private TankController _internalController;
    private Vector3 lastAliveLocation;
    private Quaternion lastAliveRotation;

    private void Start()
    {
        Respawn(transform.position);
        
        armourManager.OnPieceAdded += ChangeArmourSpeedModifer;
        armourManager.OnPieceRemoved += ChangeArmourSpeedModifer;

        
    }

    private void ChangeArmourSpeedModifer(TankArmourPiece piece, float percent)
    {
        tankMovement.SetSpeedModifer(Mathf.SmoothStep(1, armourMinSpeedModifer, tankSpeedModCurve.Evaluate(percent)));
    }

    public void LoadTankDefinition(TankDefinition tankDefinition)
    {
        this.tankDefinition = tankDefinition;
        tankSprite.LoadTankDefinitionSprites(tankDefinition);
        respawnSpriteController.SetHull(tankDefinition);

        var renderer = onHitParticleSystem.GetComponent<Renderer>();
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetTexture("_MainTex", tankDefinition.tankBase.outlineSprite.texture);
        renderer.SetPropertyBlock(block);


    }

    private void Update()
    {
        Process();
        if (isDead)
        {
            elapsedDeathTime += Time.deltaTime;
            if (elapsedDeathTime > respawnTime)
            {
                elapsedDeathTime = 0;
                Respawn();
            }
            else
            {
                float t = Mathf.Clamp01(elapsedDeathTime / (respawnTime/2));
                float t2 = Mathf.Clamp01(((elapsedDeathTime / respawnTime)-0.5f)*2);
                t = t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;  // Ease in out
                transform.rotation = Quaternion.Lerp(lastAliveRotation, Quaternion.identity, t);
                transform.position = Vector3.Lerp(lastAliveLocation, latestRespawnLocation, t);
                SetTankGrid(t2);
            }
        }
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
                onHitParticleSystem.Emit(1);
                health -= hit.damage;
                if (health <= 0)
                {
                    isDead = true;
                    onDeath(this, hit);
                    for (int i = 0; i < disableOnDeath.Length; i++)
                    {
                        disableOnDeath[i].SetActive(false);
                    }
                    respawnSpriteController.gameObject.SetActive(true);
                    SpawnDeathParticles();
                    armourPickupManager.EjectArmourPickups();
                    lastAliveLocation = transform.position;
                    lastAliveRotation = transform.rotation;
                    SetTankGrid(0);
                }
            }
            else
            {
                armourManager.ProcessDamage(hit);
            }
        }
    }

    public void SetRespawnParameters(float time, System.Func<Vector3> spawnFunc)
    {
        respawnTime = time;
        GetRespawnLocation = spawnFunc;
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
        turrets[0].ResetWeapon();
    }

    public void Respawn()
    {
        Respawn(latestRespawnLocation);
    }

    public void Respawn(Vector3 pos)
    {
        ClearTrails();
        SetTankGrid(1);
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].SetActive(true);
        }
        respawnSpriteController.gameObject.SetActive(false);
        health = maxHealth;
        isDead = false;
        tankMovement.speedModifer = 1;
        for (int i = 0; i < turrets.Count; i++)
        {
            turrets[i].ResetWeapon();
        }

        transform.position = pos;
        transform.rotation = Quaternion.identity;

        latestRespawnLocation = GetRespawnLocation();
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

    private void SetTankGrid(float amount)
    {
        //MaterialPropertyBlock block = new MaterialPropertyBlock();
        //tankSprite.GetPropertyBlock(block);
        //block.SetFloat("_Amount", amount);
        //tankSprite.SetPropertyBlock(block);

        //block = new MaterialPropertyBlock();
        //tankOutline.GetPropertyBlock(block);
        //block.SetFloat("_Amount", amount);
        //tankOutline.SetPropertyBlock(block);

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
