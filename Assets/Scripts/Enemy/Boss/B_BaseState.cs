using UnityEngine;

public abstract class B_BaseState
{
    public abstract void EnterState(BossStateManager boss);
    public abstract void UpdateState(BossStateManager boss);
    public abstract void ExitState(BossStateManager boss);
    
}
