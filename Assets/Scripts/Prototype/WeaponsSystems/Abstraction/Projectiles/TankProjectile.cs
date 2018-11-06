using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectileData
{
    public Weapon ownerWeapon;
    public TankWeaponHolder ownerWeaponHolder;
}

public class TankProjectileInstance
{
    public virtual bool doesUpdate
    {
        get { return true; }
    }

    public System.Action finishedCallback = ()  => { };
    public Vector3 position = new Vector3();
    public Vector3 direction = new Vector3();

    public TankProjectileInstance() { }
}

public abstract class TankProjectile<ProjectileInstance,ProjectileFiredDataType>:TankProjectile where ProjectileFiredDataType : TankProjectileData where ProjectileInstance : TankProjectileInstance, new() {

    private List<ProjectileInstance> projectiles = new List<ProjectileInstance>();

    [RuntimeInitializeOnLoadMethod]
    public void Init()
    {
        projectiles = new List<ProjectileInstance>();
    }

    public override void OnFired(Vector3 firedPosition, Vector3 firedDirection, object firedData = null)
    {
        ProjectileInstance newInstance = new ProjectileInstance();
        if (newInstance.doesUpdate)
        {
            newInstance.finishedCallback = () => { projectiles.Remove(newInstance); };
            projectiles.Add(newInstance);
        }
        OnFired(firedPosition, firedDirection, newInstance, (ProjectileFiredDataType)firedData);
    }

    public abstract void OnFired(Vector3 firedPosition, Vector3 firedDirection, ProjectileInstance instance, ProjectileFiredDataType data);

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

    public float damage;
    public DamageType damageType = DamageType.Singular;
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

    public abstract void OnFired(Vector3 firedPosition, Vector3 firedDirection, object firedData = null);

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
}
