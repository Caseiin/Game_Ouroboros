using UnityEngine;

public class e_patrollingState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyState)
    {
        Debug.Log("Enemy is patrolling");
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
