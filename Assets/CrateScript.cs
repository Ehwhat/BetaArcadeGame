using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CrateScript : MonoBehaviour, IDamageable {
    
    public GameObject part;
    public float area = 1.5f;
    public int partsPerPlayer = 3;
    public float health = 100;

    public void OnHit(DamageData damage)
    {
        health -= damage.damage;
        print(this.name + " " + health);
        if (health <= 0)
        {
            DestroyCrate();
        }
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
        tempPart.transform.parent = null;
    }

    void DestroyCrate()
    {
        for (int i = 0; i < partsPerPlayer * GameManager.numberOfPlayers; i++)
        {
            PlaceArmour();
        }
        print(this.name);
        Destroy(gameObject);
    }
}
