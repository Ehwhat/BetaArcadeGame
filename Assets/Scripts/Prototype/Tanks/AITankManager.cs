using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITankControllerState
{
    public TankManager target;
    public Pathfinding.Path currentPath;
    public int currentWaypoint = 0;
}

public class AITankManager : TankManager {

    AITankControllerState state = new AITankControllerState();

    public override void Process()
    {
        state = (AITankControllerState)controller.Process(this, state);
    }

}
