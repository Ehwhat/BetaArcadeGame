using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {


    public enum AutoAimType
    {
        None,
        TurretDirection,
        FiringDirection,
    }

    public float degreesPerSecond = 270;
    public TankWeaponHolder weaponHolder;
    [SerializeField]
    public Vector2 targetVector = new Vector2();

    private Vector2 currentVector = new Vector2();

    public void Start()
    {
    }

    private void Update()
    {
        RotateToVector(targetVector);
    }

    public void SetWeaponHolderOwner(TankManager owner)
    {
        weaponHolder.ownerTank = owner;
    }

    public void FireDown()
    {
        weaponHolder.OnFiringDown(transform.up);
    }

    public void Fire()
    {
        weaponHolder.OnFiring(transform.up);
    }

    public void FireUp()
    {
        weaponHolder.OnFiringUp(transform.up);
    }

    public void GiveWeapon(TankWeapon weapon)
    {
        weaponHolder.SetWeapon(weapon);
    }

    private void RotateToVector(Vector2 vector)
    {
        if(vector.sqrMagnitude <= 0.05f)
        {
            vector = currentVector;
        }
        currentVector = vector;
        Vector2 actualVector = new Vector2(-vector.x, vector.y);
        float targetAngle = Vector2.SignedAngle(actualVector, Vector2.up);
        float actualAngle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, targetAngle, degreesPerSecond * Time.deltaTime); ;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, actualAngle);
    }

    

}
