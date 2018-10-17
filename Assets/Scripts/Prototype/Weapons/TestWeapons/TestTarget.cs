using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour, IDamageable
{
    public void OnHit()
    {
        Debug.Log("Damaged!");
    }
}
