using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectileData
{
    public GameObject projectileRepresentation;
    public LayerMask projectileLayerMask;
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
    public abstract void OnFired(Vector3 firedPosition, Vector3 firedDirection, object firedData = null);

    public abstract void UpdateProjectile(float deltaTime);
}
