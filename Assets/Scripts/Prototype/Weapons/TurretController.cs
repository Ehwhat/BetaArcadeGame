using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    public float degreesPerSecond = 270;
    public TankMovement movement;
    public TankWeaponHolder weaponHolder;
    [SerializeField]
    public Vector2 targetVector = new Vector2();

    public void Start()
    {
    }

    private void Update()
    {
        RotateToTargetVector();
    }

    public void Fire()
    {
        weaponHolder.FireWeapon();
    }

    private void RotateToTargetVector()
    {
        Quaternion targetQuaternion = Quaternion.FromToRotation(Vector2.up, targetVector);
        if (targetVector.sqrMagnitude <= 0.05)
        {
            targetQuaternion = Quaternion.FromToRotation(Vector2.up, movement.targetVector);
            return;
        }
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, 270);
        if(transform.rotation.eulerAngles.y != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        }
    }

}
