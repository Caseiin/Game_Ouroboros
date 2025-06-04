using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseState : EnemyStateManager
{
    public abstract void EnterState(EnemyStateManager enemyState);
    public abstract void UpdateState(EnemyStateManager enemyState);
    public abstract void ExitState(EnemyStateManager enemyState);
}
