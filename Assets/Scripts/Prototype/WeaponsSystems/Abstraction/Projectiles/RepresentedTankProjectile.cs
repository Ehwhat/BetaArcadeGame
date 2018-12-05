using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepresentedTankProjectile<ProjectileInstance> : TankProjectile<ProjectileInstance> where ProjectileInstance : TankProjectileInstance, new()
{

    public ProjectileRepresentation projectileRepresentation;

    public int representationCount = 0;

    private const int poolInitCount = 100;
    private Transform myProjectilePoolHolder;
    private ProjectileRepresentation firstAvaliable;

    public override void OnInit()
    {
        base.OnInit();
        myProjectilePoolHolder = new GameObject(name + " Projectile Pool").transform;
        myProjectilePoolHolder.SetParent(poolHolder);

        if(projectileRepresentation == null)
        {
            throw new System.Exception("Wtf, you don't have a representaion on " + this.ToString());
        }

        firstAvaliable = Instantiate(projectileRepresentation, myProjectilePoolHolder);
        firstAvaliable.gameObject.SetActive(false);
        ProjectileRepresentation last = firstAvaliable;
        for (int i = 0; i < poolInitCount - 1; i++)
        {
            ProjectileRepresentation next = Instantiate(projectileRepresentation, myProjectilePoolHolder);
            next.gameObject.SetActive(false);
            last.next = next;
            last = next;
        }

    }

    public ProjectileRepresentation GetRepresentationInstance(Vector3 position)
    {
        ProjectileRepresentation representation = firstAvaliable;
        ProjectileRepresentation next = firstAvaliable.next;
        if (!next)
        {
            firstAvaliable = Instantiate(projectileRepresentation,position, Quaternion.identity, myProjectilePoolHolder);
            firstAvaliable.gameObject.SetActive(false);
            firstAvaliable.transform.SetParent(myProjectilePoolHolder);
        }
        else
        {
            firstAvaliable = next;
        }
        representation.transform.position = position;
        representation.gameObject.SetActive(true);
        representationCount++;
        return representation;
    }

    public void StoreRepresentationInstance(ProjectileRepresentation representation)
    {
        representation.next = firstAvaliable;
        representation.gameObject.SetActive(false);
        representationCount--;
        firstAvaliable = representation;
    }

}
