using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRepresentation : MonoBehaviour {

    public float deathDelay = 2;
    public ParticleSystem onHitParticles;
    public bool spawnParticles = false;

    public void PlayOnHitParticles()
    {
        if (onHitParticles)
        {
            if (spawnParticles)
            {
                Instantiate(onHitParticles, transform.position, transform.rotation).Play();
            }
            else
            {
                onHitParticles.Play();
            }
        }
    }

    public virtual void OnSpawn(Vector2 position, Vector2 direction)
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.up, direction);
    }

    public virtual void Destroy()
    {
        PlayOnHitParticles();
        Destroy(gameObject, deathDelay);
    }
	
}
