using UnityEngine;

public class e_ChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyState)
    {
        enemyState.enemy.Chase();
    }

    public override void UpdateState(EnemyStateManager enemyState)
    {
        if (enemyState.enemy.WithinCombatRange())
        {
            enemyState.SwitchState(enemyState.AttackState);
        }
        else if (!enemyState.enemy.PlayerDetected())
        {
            enemyState.SwitchState(enemyState.PatrolState);
        }
    }

    public override void ExitState(EnemyStateManager enemyState)
    {
        enemyState.enemy.StopChase();
    }
}
