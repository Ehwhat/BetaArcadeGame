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
    public TankMovement movement;
    public TankWeaponHolder weaponHolder;
    [SerializeField]
    public Vector2 targetVector = new Vector2();

    [Space(10)]
    [Header("Auto Aim")]
    public AutoAimType autoAimType = AutoAimType.FiringDirection;
    public float autoAimRange = 10;
    public float autoAimArcAngle = 30;
    public float autoAimDegreesPerSecond = 20;
    [Range(0,1)]
    public float autoAimOffsetMagnitude = 0.2f;
    [Header("0 is for distance, 1 is for angle")]
    [Range(0,1)]
    public float autoAimPriorityMix = 0.5f;
    public LayerMask autoAimLayer;

    private Vector2 currentVector = new Vector2();
    private Vector2 shootingDirection = Vector2.up;
    private List<Collider2D> lastValidTargets = new List<Collider2D>();
    private Collider2D lastTarget = new Collider2D();

    public void Start()
    {
    }

    private void Update()
    {
        if (autoAimType != AutoAimType.None)
        {
            AutoAimTurretAtTargets(targetVector);
        }
        else
        {
            RotateToVector(targetVector);
            shootingDirection = targetVector;
        }
    }

    public void Fire()
    {
        weaponHolder.FireWeapon(shootingDirection);
    }

    private void AutoAimTurretAtTargets(Vector2 vector)
    {
        List<Collider2D> validTargets = FindAutoAimTargets(vector);
        lastValidTargets = validTargets;
        if(validTargets.Count <= 0)
        {
            lastTarget = null;
            shootingDirection = vector;
            RotateToVector(vector);
            return;
        }
        Collider2D target = ChooseClosestAutoAimTarget(vector,validTargets);
        lastTarget = target;
        Vector2 direction = (target.transform.position - transform.position).normalized;

        Vector2 calculatedVector = Vector3.RotateTowards(currentVector, direction, autoAimDegreesPerSecond * Mathf.Deg2Rad * Time.deltaTime, 1);
        calculatedVector = (calculatedVector + (vector * autoAimOffsetMagnitude)).normalized;

        if(autoAimType == AutoAimType.FiringDirection)
        {
            RotateToVector(vector);
            shootingDirection = calculatedVector;

        }
        else if (Vector2.Angle(vector, direction) < Vector2.Angle(calculatedVector, direction))
        {
            RotateToVector(vector);
            shootingDirection = vector;
        }
        else
        {
            RotateToVector(calculatedVector);
            shootingDirection = calculatedVector;
        }
    }

    private List<Collider2D> FindAutoAimTargets(Vector2 vector)
    {
        List<Collider2D> validTargets = new List<Collider2D>();
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, autoAimRange, autoAimLayer);
        for (int i = 0; i < targets.Length; i++)
        {
            Vector2 direction = (targets[i].transform.position - transform.position).normalized;
            if(Vector2.Angle(direction, vector) <= autoAimArcAngle/2)
            {
                validTargets.Add(targets[i]);
            }
        }
        return validTargets;

    }

    private Collider2D ChooseClosestAutoAimTarget(Vector2 vector,List<Collider2D> colliders)
    {
        if(colliders.Count <= 0)
        {
            return null;
        }

        Collider2D bestCollider = colliders[0];
        float bestScore = ScoreTarget(vector, colliders[0].transform.position);
        for (int i = 1; i < colliders.Count; i++)
        {
            float score = ScoreTarget(vector, colliders[i].transform.position); 
            if (score > bestScore)
            {
                bestScore = score;
                bestCollider = colliders[i];
            }
        }
        return bestCollider;
    }

    private float ScoreTarget(Vector2 vector, Vector2 targetPosition)
    {
        Vector2 difference = (targetPosition - (Vector2)transform.position);
        float distance = Vector2.Distance(targetPosition, transform.position);
        Vector2 direction = difference.normalized;
        float angle = Vector2.Angle(vector, direction);
        float distanceScore = 1f - (distance / autoAimRange);
        float directionScore = 1f - (angle / autoAimArcAngle);

        return ((distanceScore * (1 - autoAimPriorityMix)) + (directionScore * autoAimPriorityMix));

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

    private void OnDrawGizmos()
    {
        if (autoAimType != AutoAimType.None)
        {
            Vector2 direction = targetVector;
            if(direction.sqrMagnitude <= 0.05f)
            {
                direction = currentVector;
            }

            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.position, targetVector * autoAimRange);

            Gizmos.color = new Color(0,1,1,0.5f);
            Gizmos.DrawWireSphere(transform.position, autoAimRange);

            float angle = Vector2.SignedAngle(Vector2.up, targetVector);
            float startRad = (angle - (autoAimArcAngle / 2)+90)*Mathf.Deg2Rad;
            float endRad = (angle + (autoAimArcAngle / 2)+90) * Mathf.Deg2Rad;

            Vector2 startPoint = (Vector2)transform.position + autoAimRange * new Vector2(Mathf.Cos(startRad), Mathf.Sin(startRad));
            Vector2 endPoint = (Vector2)transform.position + autoAimRange * new Vector2(Mathf.Cos(endRad), Mathf.Sin(endRad));

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, startPoint);
            Gizmos.DrawLine(transform.position, endPoint);

            Gizmos.color = Color.yellow;
            for (int i = 0; i < lastValidTargets.Count; i++)
            {
                Gizmos.DrawLine(transform.position, lastValidTargets[i].transform.position);
            }
            Gizmos.color = Color.red;
            if (lastTarget)
            {
                Gizmos.DrawLine(transform.position, lastTarget.transform.position);
            }
            
        }
    }

}
