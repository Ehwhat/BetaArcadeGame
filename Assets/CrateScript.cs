using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CrateScript : MonoBehaviour, IDamageable {

    public GameManager instance;
    public GameObject part;
    public float area = 1.5f;
    public int partsPerPlayer = 3;
    public int numberOfPlayers = 4;
    public float health = 100;

    public void OnHit(DamageData damage)
    {
        health -= damage.damage;
        if (health <= 0)
        {
            DestroyCrate();
        }
    }

    // Use this for initialization
    void Start () {
        numberOfPlayers = instance.players.Length;
    }
	
	// Update is called once per frame
	void Update () {

    }


    void PlaceArmour()
    {
        Vector3 tempPos = new Vector3(Random.Range(area, -area), Random.Range(area, -area), 0);
        GameObject tempPart = Instantiate(part);
        tempPart.transform.parent = transform;
        tempPart.transform.localPosition = tempPos;
    }

    void DestroyCrate()
    {
        for (int i = 0; i < partsPerPlayer * numberOfPlayers; i++)
        {
            PlaceArmour();
        }
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
