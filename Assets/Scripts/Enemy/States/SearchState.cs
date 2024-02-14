using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float moveTimer;
    private float searchTimer;


    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnownPos);
    }
    public override void Perform()
    {
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
        
        if(enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (moveTimer > Random.Range(1, 3))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }
            if (searchTimer > Random.Range(2,9)) 
            {
             stateMachine.ChangeState(new PatrolState());
            }
        }
    }
    public override void Exit()
    {

    }
}
