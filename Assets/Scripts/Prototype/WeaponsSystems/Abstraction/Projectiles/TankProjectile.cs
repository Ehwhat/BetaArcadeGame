using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectileInstance
{
    public virtual bool doesUpdate
    {
        get { return true; }
    }

    public System.Action finishedCallback = ()  => { };
    public Vector3 position = new Vector3();
    public Vector3 direction = new Vector3();
    public WeaponData weaponData;

    public TankProjectileInstance() { }
}

public abstract class TankProjectile<ProjectileInstance>:TankProjectile where ProjectileInstance : TankProjectileInstance, new() {

    private List<ProjectileInstance> projectiles = new List<ProjectileInstance>();

    [RuntimeInitializeOnLoadMethod]
    public void Init()
    {
        projectiles = new List<ProjectileInstance>();
    }

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, WeaponData weaponData)
    {
        ProjectileInstance newInstance = new ProjectileInstance();
        newInstance.weaponData = weaponData;
        if (newInstance.doesUpdate)
        {
            newInstance.finishedCallback = () => { projectiles.Remove(newInstance); };
            projectiles.Add(newInstance);
        }
        OnFired(firedPosition, firedDirection, newInstance, weaponData);
    }

    public abstract void OnFired(Vector3 firedPosition, Vector3 firedDirection, ProjectileInstance instance, WeaponData weaponData);

    public override void UpdateProjectile(float deltaTime){
        for (int i = 0; i < projectiles.Count; i++)
        {
            UpdateProjectile(deltaTime, projectiles[i]);
        }
    }

    public abstract void UpdateProjectile(float deltaTime, ProjectileInstance instance);

}

public abstract class TankProjectile : ScriptableObject, IProjectile
{
    public enum DamageType
    {
        Singular,
        Explosive
    }
    public enum ExplosiveDamageFalloff
    {
        Fixed,
        Linear,
        InverseSquared
    }

    public float damage;
    public DamageType damageType = DamageType.Singular;
    public ExplosiveDamageFalloff explosiveFalloff = ExplosiveDamageFalloff.InverseSquared;
    public float explosiveRange = 1;

    public LayerMask projectileLayerMask;
    public LayerMask damageableLayerMask;

    private static bool alreadyRan = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        if (!alreadyRan)
        {
            TankProjectile[] projectileInstances = Resources.LoadAll<TankProjectile>("");
            for (int i = 0; i < projectileInstances.Length; i++)
            {
                CoroutineServer.StartCoroutine(UpdateProjectileEnumerator(projectileInstances[i]));
            }
            alreadyRan = true;
        }
        
    }

    private static IEnumerator UpdateProjectileEnumerator(TankProjectile projectile)
    {
        while (true)
        {
            projectile.UpdateProjectile(Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public virtual void OnCharge(Vector3 firedPosition, Vector3 firedDirection, WeaponData weaponData, float chargeAmount) {
    }

    public abstract void OnFired(Vector3 firedPosition, Vector3 firedDirection, WeaponData weaponData);

    public abstract void UpdateProjectile(float deltaTime);

    public bool AttemptToDamage(Collider2D collider, RaycastHit2D hit, TankProjectileInstance instance)
    {
        return AttemptToDamage(collider, hit, instance, damage);
    }

    public bool AttemptToDamage(Collider2D collider, RaycastHit2D hit, TankProjectileInstance instance, float customDamage)
    {
        TankProjectileDamageData hitData = new TankProjectileDamageData(customDamage, hit, this, instance);
        switch (damageType)
        {
            case DamageType.Singular:
                return AttemptToDamageSingular(collider, hit, hitData, instance);
            case DamageType.Explosive:
                return AttemptToDamageExplosive(collider, hit, hitData, instance);
        }
        return false;
    }

    private bool AttemptToDamageExplosive(Collider2D collider, RaycastHit2D sourceHit, DamageData hitData, TankProjectileInstance instance)
    {
        float sourceDamage = hitData.damage;
        bool hitSomething = false;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(sourceHit.point, explosiveRange, damageableLayerMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            float distance = Vector2.Distance(sourceHit.point, hitColliders[i].transform.position);
            hitData.damage = CalulateExplosiveDamage(sourceDamage, distance);
            AttemptToDamageSingular(hitColliders[i], sourceHit, hitData, instance);
            hitSomething = true;
        }
        return hitSomething;
    }

    private bool AttemptToDamageSingular(Collider2D collider, RaycastHit2D hit, DamageData hitData, TankProjectileInstance instance)
    {
        if (damageableLayerMask == (damageableLayerMask | (1 << collider.gameObject.layer)))
        {
            IDamageable[] hitDamageables = collider.transform.root.GetComponentsInChildren<IDamageable>();

            for (int i = 0; i < hitDamageables.Length; i++)
            {
                hitDamageables[i].OnHit(hitData);
            }
            return true;
        }
        return false;
    }

    private float CalulateExplosiveDamage(float sourceDamage, float distance)
    {
        switch (explosiveFalloff)
        {
            case ExplosiveDamageFalloff.Fixed:
                return sourceDamage;
            case ExplosiveDamageFalloff.Linear:
                return sourceDamage / (distance + 1);
            case ExplosiveDamageFalloff.InverseSquared:
                return sourceDamage / ((distance * distance) + 1);
        }
        return sourceDamage;
    }

    
}
