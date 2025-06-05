using UnityEngine;

public class e_MeleeState : EnemyBaseState
{
    MeleeEnemy enemy;
    public override void EnterState(EnemyStateManager enemyState)
    {
        enemy = enemyState.GetComponent<MeleeEnemy>();
    }

    public override void UpdateState(EnemyStateManager enemyState)
    {
        enemy = enemyState.GetComponent<MeleeEnemy>();
        //Transition checks?
    }

    public override void ExitState(EnemyStateManager enemyState)
    {
        enemy = enemyState.GetComponent<MeleeEnemy>();
    }
}
