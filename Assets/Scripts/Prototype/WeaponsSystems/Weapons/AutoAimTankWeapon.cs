using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Weapons/New Auto Aimed Tank Weapon", fileName = "New Auto Aimed Tank Weapon")]
public class AutoAimTankWeapon : TankWeapon {

    public enum AutoAimType
    {
        None,
        Positional,
        Velocity,
    }

    [Space(10)]
    [Header("Auto Aim")]
    public AutoAimType autoAimType = AutoAimType.Velocity;
    public float autoAimRange = 10;
    public float autoAimArcAngle = 60;
    public float autoAimDegreesPerSecond = 720;
    [Range(0, 1)]
    public float autoAimOffsetMagnitude = 0.675f;
    [Range(1,10)]
    public int autoAimCycles = 3;
    public float autoAimPredictedProjectileSpeed = 10;

    [Header("0 is for distance, 1 is for angle")]
    [Range(0, 1)]
    public float autoAimPriorityMix = 0.78f;
    
    public LayerMask autoAimLayer;
    public LayerMask autoAimBlockingLayer = Physics2D.AllLayers;

    private Vector2 lastInputDirection = Vector2.up;
    private Vector2 lastDirection = Vector2.zero;
    private Vector2 lastPosition = Vector2.up;
    private List<Collider2D> lastValidTargets = new List<Collider2D>();
    private Collider2D lastTarget = new Collider2D();
    private Vector2 lastVelocity;

    public override bool FireProjectile(Vector2 position, Vector2 direction, TankWeaponHolder holder)
    {
        Vector2 autoAimedDirection = AutoAimAtTargets(position, direction, holder);
        lastPosition = position;
        lastInputDirection = direction;
        lastDirection = autoAimedDirection;
        return base.FireProjectile(position, autoAimedDirection, holder);
    }

    private Vector2 AutoAimAtTargets(Vector2 position,Vector2 vector, TankWeaponHolder holder)
    {
        if(autoAimType == AutoAimType.None)
        {
            return vector;
        }
        List<Collider2D> validTargets = FindAutoAimTargets(position,vector, holder);
        lastValidTargets = validTargets;
        if (validTargets.Count <= 0)
        {
            lastTarget = null;
            return vector;
        }
        Collider2D target = ChooseClosestAutoAimTarget(position,vector, validTargets);
        lastTarget = target;
        Vector2 targetPoint = GetTarget(position, vector, target);
        Vector2 direction = (targetPoint - position).normalized;

        Vector2 calculatedVector = direction;
        calculatedVector = Vector2.Lerp(calculatedVector, vector, autoAimOffsetMagnitude);

        return calculatedVector;
    }

    private Vector2 GetTarget(Vector2 position, Vector2 bulletDirection, Collider2D target, int autoAimCycles = 1)
    {
        Rigidbody2D rb = target.attachedRigidbody;

        

        if(autoAimType == AutoAimType.Positional || rb == null)
        {
            return target.transform.position;
        }
        Vector2 direction = (Vector2)target.transform.position - position;
        Vector2 acceleration = (rb == lastTarget) ? rb.velocity - lastVelocity / Time.deltaTime : Vector2.zero;
        if (rb == lastTarget)
        {

        }

        float time = direction.magnitude / autoAimPredictedProjectileSpeed;
        Vector2 predictedPoint = (Vector2)target.transform.position + rb.velocity * time + 0.5f * acceleration * (time * time);

        for (int i = 1; i < autoAimCycles; i++)
        {
            direction = predictedPoint - position;
            time = direction.magnitude / autoAimPredictedProjectileSpeed;
            predictedPoint = (Vector2)target.transform.position + rb.velocity * time + 0.5f * acceleration * (time * time);
        }

        return predictedPoint;

    }

    private List<Collider2D> FindAutoAimTargets(Vector2 position,Vector2 vector,TankWeaponHolder holder)
    {
        List<Collider2D> validTargets = new List<Collider2D>();
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, autoAimRange, autoAimLayer);
        for (int i = 0; i < targets.Length; i++)
        {
            if(targets[i].transform.root == holder.transform.root)
            {
                continue;
            }
            Vector2 direction = ((Vector2)targets[i].transform.position - position).normalized;
            if (Vector2.Angle(direction, vector) <= autoAimArcAngle / 2)
            {
                validTargets.Add(targets[i]);
            }
        }
        return validTargets;

    }

    private Collider2D ChooseClosestAutoAimTarget(Vector2 position, Vector2 vector, List<Collider2D> colliders)
    {
        if (colliders.Count <= 0)
        {
            return null;
        }

        Collider2D bestCollider = colliders[0];
        float bestScore = ScoreTarget(position,vector, colliders[0].transform.position);
        for (int i = 1; i < colliders.Count; i++)
        {
            float score = ScoreTarget(position, vector, colliders[i].transform.position);
            if (score > bestScore)
            {
                bestScore = score;
                bestCollider = colliders[i];
            }
        }
        return bestCollider;
    }

    private float ScoreTarget(Vector2 position,Vector2 vector, Vector2 targetPosition)
    {
        Vector2 difference = (targetPosition - position);
        float distance = Vector2.Distance(targetPosition, position);
        Vector2 direction = difference.normalized;
        float angle = Vector2.Angle(vector, direction);
        float distanceScore = 1f - (distance / autoAimRange);
        float directionScore = 1f - (angle / autoAimArcAngle);

        return ((distanceScore * (1 - autoAimPriorityMix)) + (directionScore * autoAimPriorityMix));

    }

    public Collider2D GetLastTarget()
    {
        return lastTarget;
    }
    public override void OnDrawGizmos()
    {
        if (autoAimType != AutoAimType.None && false)
        {
            Vector2 direction = lastDirection;

            Gizmos.color = Color.white;
            Gizmos.DrawRay(lastPosition, lastInputDirection * autoAimRange);

            Gizmos.color = new Color(0, 1, 1, 0.5f);
            Gizmos.DrawWireSphere(lastPosition, autoAimRange);

            float angle = Vector2.SignedAngle(Vector2.up, lastInputDirection);
            float startRad = (angle - (autoAimArcAngle / 2) + 90) * Mathf.Deg2Rad;
            float endRad = (angle + (autoAimArcAngle / 2) + 90) * Mathf.Deg2Rad;

            Vector2 startPoint = (Vector2)lastPosition + autoAimRange * new Vector2(Mathf.Cos(startRad), Mathf.Sin(startRad));
            Vector2 endPoint = (Vector2)lastPosition + autoAimRange * new Vector2(Mathf.Cos(endRad), Mathf.Sin(endRad));

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(lastPosition, startPoint);
            Gizmos.DrawLine(lastPosition, endPoint);

            Gizmos.color = Color.yellow;
            for (int i = 0; i < lastValidTargets.Count; i++)
            {
                Gizmos.DrawLine(lastPosition, lastValidTargets[i].transform.position);
            }
            Gizmos.color = Color.red;
            if (lastTarget)
            {
                Gizmos.DrawLine(lastPosition, lastTarget.transform.position);
            }

        }
    }
}
