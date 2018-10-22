using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    public float degreesPerSecond = 270;
    public TankMovement movement;
    public TankWeaponHolder weaponHolder;
    [SerializeField]
    public Vector2 targetVector = new Vector2();

    [Space(10)]
    [Header("Auto Aim")]
    public bool autoAimEnabled = true;
    public float autoAimRange = 10;
    public float autoAimArcAngle = 30;
    public float autoAimDegreesPerSecond = 20;
    public LayerMask autoAimLayer;

    private Vector2 currentVector = new Vector2();

    public void Start()
    {
    }

    private void Update()
    {
        if (autoAimEnabled)
        {
            AutoAimTurretAtTargets(targetVector);
        }
        else
        {
            RotateToVector(targetVector);
        }
    }

    public void Fire()
    {
        weaponHolder.FireWeapon();
    }

    private void AutoAimTurretAtTargets(Vector2 vector)
    {
        List<Collider2D> validTargets = FindAutoAimTargets(vector);
        if(validTargets.Count <= 0)
        {
            RotateToVector(vector);
            return;
        }
        Collider2D target = ChooseClosestAutoAimTarget(validTargets);
        Vector2 direction = (target.transform.position - transform.position).normalized;

        Vector2 calculatedVector = Vector3.RotateTowards(currentVector, direction, autoAimDegreesPerSecond * Mathf.Deg2Rad * Time.deltaTime, 1);
        if (Vector2.Angle(vector, direction) < Vector2.Angle(calculatedVector, direction))
        {
            RotateToVector(vector);
        }
        else
        {
            RotateToVector(calculatedVector);
        }
    }

    private List<Collider2D> FindAutoAimTargets(Vector2 vector)
    {
        List<Collider2D> validTargets = new List<Collider2D>();
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, autoAimRange, autoAimLayer);
        for (int i = 0; i < targets.Length; i++)
        {
            Vector2 direction = (targets[i].transform.position - transform.position).normalized;
            if(Vector2.Angle(direction, vector) <= autoAimArcAngle)
            {
                validTargets.Add(targets[i]);
            }
        }
        return validTargets;

    }

    private Collider2D ChooseClosestAutoAimTarget(List<Collider2D> colliders)
    {
        if(colliders.Count <= 0)
        {
            return null;
        }
        Collider2D bestCollider = colliders[0];
        float bestDistance = (colliders[0].transform.position - transform.position).sqrMagnitude;
        for (int i = 1; i < colliders.Count; i++)
        {
            float distance = (colliders[i].transform.position - transform.position).sqrMagnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestCollider = colliders[i];
            }
        }
        return bestCollider;
    }

    private void RotateToVector(Vector2 vector)
    {
        currentVector = vector;
        Quaternion targetQuaternion = Quaternion.FromToRotation(Vector2.up, vector);
        if (vector.sqrMagnitude <= 0.05)
        {
            targetQuaternion = Quaternion.FromToRotation(Vector2.up, movement.targetVector);
            return;
        }
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, degreesPerSecond * Time.deltaTime);
        if(transform.rotation.eulerAngles.y != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        }
    }

}
