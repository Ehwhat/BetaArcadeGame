using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirepitTrap : MonoBehaviour {

    enum Stage
    {
        Off,
        Warning,
        On,
    }

    public ParticleSystem bigFire;
    public ParticleSystem smallFire;

    public float delayMin = 20f;
    public float delayMax = 40f;
    public float warningTime = 5f;

    public float duration = 10f;

    public LayerMask tankLayermask;
    public float damagePerSecond = 50f;
    public Vector2 box = new Vector2();

    private Stage stage;
    private float phaseTime = 0;
    private float actualDelay = 0;

	void Start () {
        bigFire.Stop();
        smallFire.Stop();
        phaseTime = 0;
        stage = Stage.Off;
        actualDelay = PickDelay();

    }
	
	// Update is called once per frame
	void Update () {

        if(phaseTime > (actualDelay - warningTime) && stage == Stage.Off)
        {
            smallFire.Play();
            stage = Stage.Warning;
        }else if(phaseTime > actualDelay && stage == Stage.Warning)
        {
            smallFire.Stop();
            bigFire.Play();
            stage = Stage.On;
        }else if(phaseTime > actualDelay + duration && stage == Stage.On)
        {
            bigFire.Stop();
            stage = Stage.Off;
            phaseTime = 0;
        }

        phaseTime += Time.deltaTime;

        if (stage == Stage.On)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, box, 0, tankLayermask);
            List<TankManager> tanks = new List<TankManager>();
            for (int i = 0; i < colliders.Length; i++)
            {
                TankManager tank = colliders[i].attachedRigidbody.GetComponent<TankManager>();
                if (tank && !tanks.Contains(tank))
                {
                    tanks.Add(tank);
                }
            }
            for (int i = 0; i < tanks.Count; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle;
                tanks[i].OnHit(new DamageData(damagePerSecond * Time.deltaTime, Vector2.zero, tanks[i].transform.position, tanks[i].coreCollider));
            }
        }

    }

    float PickDelay()
    {
        return Random.Range(delayMin, delayMax);
    }
}
