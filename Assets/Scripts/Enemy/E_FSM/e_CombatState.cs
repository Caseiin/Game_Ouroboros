using UnityEngine;

public abstract class e_CombatState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyState)
    {
        Debug.Log("Enemy is attacking!");
    }

    public override void UpdateState(EnemyStateManager enemyState)
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState(EnemyStateManager enemyState)
    {
        throw new System.NotImplementedException();
    }

}
