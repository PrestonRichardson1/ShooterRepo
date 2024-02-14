using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    // track which waypoint we are currently targeting.
    public int waypointsIndex;
    public float waitTimer;

    public override void Enter()
    {
    }
    public override void Perform()
    {
        PatrolCycle();
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }
    public override void Exit()
    {
    }

    public void PatrolCycle()
    {
        // Implement our patrol Logic.
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > 3)
            {

            
            if(waypointsIndex < enemy.path.waypoints.Count - 1)
            {
                waypointsIndex++;
            }
            else
            {
                waypointsIndex = 0;
            }
            enemy.Agent.SetDestination(enemy.path.waypoints[waypointsIndex].position);
            waitTimer = 0;
            }
        }
    }

}
