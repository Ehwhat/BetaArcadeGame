using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public Transform[] spawnPoints;


    public List<Transform> GetSpawnPositions()
    {
        return new List<Transform>(spawnPoints);
    }


}
