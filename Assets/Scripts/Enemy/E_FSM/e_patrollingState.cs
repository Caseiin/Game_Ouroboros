using UnityEngine;

public class e_patrollingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyState)
    {
        //Starts patrolling
        enemyState.enemy.StartPatrol();
    }

    public override void UpdateState(EnemyStateManager enemyState)
    {
        if (enemyState.enemy.PlayerDetected())
        {
            //Move to the chase state
            enemyState.SwitchState(enemyState.ChaseState);
            return; //prevents further execution after switching
        }

        //Implementing Patrol logic
        enemyState.enemy.PatrolPattern();
    }

    public override void ExitState(EnemyStateManager enemyState)
    {
        enemyState.enemy.StopPatrol();
    }
}
