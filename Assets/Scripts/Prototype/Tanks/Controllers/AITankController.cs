using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Controllers/New AI Tank Controller", fileName = "New AI Tank Controller")]
public class AITankController : TankController {

    public LayerMask targetMask;
    public float searchRadius = 100;

    public override object Process(TankManager manager, object state = null)
    {
        AITankControllerState aiState = (AITankControllerState)state;
        if (aiState != null)
        {
            TankManager lastTarget = aiState.target;
            aiState.target = FindTarget(manager);

            if(aiState.target != null)
            {
                manager.GetComponent<Pathfinding.Seeker>().StartPath(manager.transform.position, aiState.target.transform.position, (Path p) => { aiState.currentPath = p; aiState.currentWaypoint = 0; Debug.Log("Ai found Path"); });
            }
            Debug.Log("Ai update");

            if (aiState.currentPath != null)
            {
                if (aiState.currentWaypoint >= aiState.currentPath.vectorPath.Count)
                {
                    manager.tankMovement.targetSpeed = 0;
                    return aiState;
                }
                else
                {
                    Vector2 waypoint = aiState.currentPath.vectorPath[aiState.currentWaypoint];
                    if (Vector2.Distance(manager.transform.position, waypoint) <= 5)
                    {
                        aiState.currentWaypoint++;
                        return aiState;
                    }

                    Vector2 direction = (waypoint - (Vector2)manager.transform.position).normalized;

                    manager.tankMovement.targetSpeed = 1;
                    manager.tankMovement.targetVector = direction;

                }
            }

            return aiState;
        }
        return state;
    }

    private TankManager FindTarget(TankManager self)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(self.transform.position, searchRadius, targetMask);
        TankManager bestTank = null;
        float bestScore = float.PositiveInfinity;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.root == self.transform.root)
                continue;
            TankManager tank = hits[i].transform.root.GetComponent<TankManager>();
            if (tank)
            {
                float dist = Vector2.Distance(tank.transform.position, self.transform.position);
                if (dist < bestScore)
                {
                    bestScore = dist;
                    bestTank = tank;
                }
            }
        }

        return bestTank;

    }

}
