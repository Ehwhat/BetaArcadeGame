using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ArmourPlacementSystem : MonoBehaviour {
    public int layout = 0;
    public int numParts = 0;
    public GameObject part;
    public ArmourPart[] armourStats;

    // Use this for initialization
    void Start ()
    {
        //LoadPro();
        CheckAll();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [ContextMenu("Load Children")]
    private void LoadChildren()
    {
        armourStats = GetComponentsInChildren<ArmourPart>();
    }

    void LoadPro()
    {
        if (layout == 0)
        {
            numParts = 5;
        }
        else
        {
            print("Error on layout");
        }
        armourStats = new ArmourPart[numParts];
        //armourBlocks = new GameObject[numParts];

        for (int i = 0; i < numParts; i++)
        {
            //armourBlocks[i].transform.parent = this.transform;
            //transform.localPosition = new Vector3 (armourStats[i].posX, armourStats[i].posY, 0);
            //if (armourStats[i].nDep == 0)
            //{
            //    //armourBlocks[i].GetComponent<Collider2D>().enabled = true;
            //    //armourStats[i].colEna = true;
            //}
        }

        if (layout == 0)
        {
            //armourStats[0].posX = 0;
            //armourStats[0].posY = 0;
            //armourStats[0].scaX = 10;
            //armourStats[0].scaY = 10;
        }
    }

     public void CheckAll()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<ArmourPart>().DepCheck();
        }
    }
}
