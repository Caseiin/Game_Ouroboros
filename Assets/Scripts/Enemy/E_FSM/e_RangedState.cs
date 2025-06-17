using UnityEngine;

public class e_RangedState : e_CombatState
{
    public override void EnterState(EnemyStateManager enemyState)
    {
        enemyState.enemy.RangedAttack();
    }

    public override void UpdateState(EnemyStateManager enemyState)
    {
        if (!enemyState.enemy.PlayerDetected())
        { enemyState.SwitchState(enemyState.PatrolState); }

        enemyState.enemy.RangedAttack();
    }

    public override void ExitState(EnemyStateManager enemyState)
    {
        enemyState.enemy.StateCleanUp();
    }
}
