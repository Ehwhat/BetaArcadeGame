using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRepresentation : MonoBehaviour {

    public System.Action<Color> onCustomColourAdded = (Color c) => { };

    public float deathDelay = 2;
    public ParticleSystem onHitParticles;
    public TrailRenderer trailRenderer;
    public bool spawnParticles = false;
    
    public Color customColour;

    [HideInInspector]
    public ProjectileRepresentation next;

    private System.Action storeCallback;
    private bool isDead = false;
    private bool clearTrail = true;
    private float deathTime = 0;

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

    private void Update()
    {
        if (isDead && Time.time - deathTime > deathDelay)
        {
            if (clearTrail && trailRenderer)
            {
                trailRenderer.Clear();
            }
            storeCallback();
        }
    }

    public virtual void OnSpawn(Vector2 position, Vector2 direction)
    {
        isDead = false;
        transform.rotation = Quaternion.FromToRotation(Vector2.up, direction);
    }

    public virtual void SetColour(Color colour)
    {
        customColour = colour;
        onCustomColourAdded(colour);
    }

    public virtual void Destroy(System.Action storeCallback, bool playParticles = true, bool clearTrailRenderer = true)
    {
        if(playParticles)
            PlayOnHitParticles();
        clearTrail = clearTrailRenderer;
        this.storeCallback = storeCallback;
        deathTime = Time.time;
        isDead = true;
    }
	
}
