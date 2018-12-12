﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArmourDrops : MonoBehaviour {
    
    public GameObject part;
    public int numberOfPlayers = 4;
    public Vector2 area = new Vector2 (3.0f, 3.0f);
    public int timerSet = 960;
    private int timer;
    public int startingPartsPerPlayer = 1;

    // Use this for initialization
    void Start ()
    {
        numberOfPlayers = GameManager.numberOfPlayers;
        timerSet /= numberOfPlayers;
        timer = 1;
        for (int i = 0; i < startingPartsPerPlayer * numberOfPlayers; i++)
        {
            PlaceArmour();
        }
	}
	
	// Update is called once per frame
	void Update () {
        timer--;
        if(timer <= 0)
        {
            timer = timerSet;
            PlaceArmour();
        }
	}

    void PlaceArmour()
    {
        float tempX = Random.Range(area.x, -area.x);
        float tempY = Random.Range(area.y, -area.y);
        Vector3 tempPos = new Vector3(Random.Range(area.x, -area.x), Random.Range(area.y, -area.y), 0);
        GameObject tempPart = Instantiate(part);
        tempPart.transform.SetParent(this.transform);
        tempPart.transform.localPosition = tempPos;
    }
}
