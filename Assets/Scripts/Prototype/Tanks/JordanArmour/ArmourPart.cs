using System.Collections;
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

    public float health = 0;
    public ArmourPart[] dependances;

    public ArmourPart()
    {

    }
}
