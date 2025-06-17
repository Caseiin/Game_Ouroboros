using UnityEngine;

public class e_MeleeState : e_CombatState
{
   
    public override void EnterState(EnemyStateManager enemyState)
    {
        enemyState.enemy.MeleeAttack();
    }

    public override void UpdateState(EnemyStateManager enemyState)
    {
        //Transition checks
        if (enemyState.enemy.PlayerDetected() && !enemyState.enemy.WithinCombatRange())
        {
            enemyState.SwitchState(enemyState.ChaseState);
            return;
        }


        if (!enemyState.enemy.PlayerDetected())
            {
                enemyState.SwitchState(enemyState.PatrolState);
                return; //important return
            }

        enemyState.enemy.MeleeAttack();
    }

    public override void ExitState(EnemyStateManager enemyState)
    {
        //Melee cleanup being animations and coroutines
        enemyState.enemy.StateCleanUp();
    }
}
