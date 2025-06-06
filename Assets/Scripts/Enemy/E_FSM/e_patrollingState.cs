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
        if (enemyState.enemy.PlayerDetected() && !enemyState.enemy.WithinCombatRange())
        {
            //Move to the chase state
            enemyState.SwitchState(enemyState.ChaseState);
        }
        else if (enemyState.enemy.PlayerDetected() && enemyState.enemy.WithinCombatRange())
        {
            //Move to the Attack State
            enemyState.SwitchState(enemyState.AttackState);
        }

        //Implementing Patrol logic
        enemyState.enemy.PatrolPattern();
    }

    public override void ExitState(EnemyStateManager enemyState)
    {
        enemyState.enemy.StopPatrol();
    }
}
