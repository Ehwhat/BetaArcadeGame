﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmourPart : MonoBehaviour
{
    public enum DependecyType
    {
        Or,
        And
    }

    public bool IsActive
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }

    public float health = 100;
    public ArmourPart[] dependancesPrime;
    public ArmourPart[] dependancesSecond;

    public ArmourPart()
    {

    }

    public void DepCheck()
    {
        if (tag == "Ave")
        {
            int all = dependancesPrime.Length;
            foreach(ArmourPart part in dependancesPrime)
            {
                if (part.tag == "Full")
                {
                    all--;
                }
            }
            if (all == 0)
            {
                GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }
        else if (tag == "Full")
        {
            bool connected = false;
            if (dependancesPrime.Length > 0)
            {
                foreach (ArmourPart part in dependancesPrime)
                {
                    if (part.tag == "Full")
                    {
                        connected = true;
                    }
                }
                foreach (ArmourPart part in dependancesSecond)
                {
                    if (part.tag == "Full")
                    {
                        connected = true;
                    }
                }
            }
            else
            {
                connected = true;
            }
            if (connected == false)
            {
                DropPart();
            }
            
        }
    }

    public void DestroyPart()
    {
        tag = "Ave";
        //GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }

    public void DropPart()
    {
        tag = "Ave";
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        print("Part dropped");
    }

    public void AttachPart()
    {
        tag = "Full";
        //GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = true;
        GetComponentInParent<ArmourPlacementSystem>().CheckAll();
    }
}
