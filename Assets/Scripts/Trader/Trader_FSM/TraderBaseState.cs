using UnityEngine;

public abstract class TraderBaseState
{
    public abstract void Enterstate(TraderStateManager TradeState);
    public abstract void Updatestate(TraderStateManager TradeState);
    public abstract void ExitState(TraderStateManager traderState);
}
