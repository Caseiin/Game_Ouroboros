using UnityEngine;

public class t_IdleState : TraderBaseState
{
    public override void Enterstate(TraderStateManager TradeState)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Trader is sleeping!");
    }

    public override void Updatestate(TraderStateManager TradeState)
    {
        //throw new System.NotImplementedException();
    }

    public override void ExitState(TraderStateManager traderState)
    {
        // throw new System.NotImplementedException();
    }
}
