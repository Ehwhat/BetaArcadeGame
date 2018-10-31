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

    public abstract void OnFired(Vector3 firedPosition, Vector3 firedDirection, WeaponData weaponData);

    public abstract void UpdateProjectile(float deltaTime);

    public void AttemptToDamage(Collider2D collider, RaycastHit2D hit)
    {
        ProjectileHit hitData = new ProjectileHit()
        {
            hitData = hit,
            projectile = this,
            damage = damage
        };
        switch (damageType)
        {
            case DamageType.Singular:
                AttemptToDamageSingular(collider, hit, hitData);
                break;
            case DamageType.Explosive:
                AttemptToDamageExplosive(collider, hit, hitData);
                break;
        }
    }

    private void AttemptToDamageExplosive(Collider2D collider, RaycastHit2D hit, ProjectileHit hitData)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(hit.point, explosiveRange, damageableLayerMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            AttemptToDamageSingular(hitColliders[i], hit, hitData);
        }
    }

    private void AttemptToDamageSingular(Collider2D collider, RaycastHit2D hit, ProjectileHit hitData)
    {
        if (damageableLayerMask == (damageableLayerMask | (1 << collider.gameObject.layer)))
        {
            IDamageable[] hitDamageables = collider.transform.root.GetComponentsInChildren<IDamageable>();
            for (int i = 0; i < hitDamageables.Length; i++)
            {
                hitDamageables[i].OnHit(hitData);
            }
        }
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
