using UnityEngine;

public class TraderStateManager : MonoBehaviour
{
    TraderBaseState CurrentState; // current context of trader

    t_IdleState idleState = new t_IdleState();
    t_ShopState shopState = new t_ShopState();

    public NPC traderNPC;
    public BaseTrader baseTrader;

    void Start()
    {
        CurrentState = idleState;
        CurrentState?.Enterstate(this);
    }

    void Update()
    {
        CurrentState?.Updatestate(this);
    }

    void SwitchState(TraderBaseState newState)
    {
        CurrentState = newState;
        CurrentState?.ExitState(this);
        CurrentState?.Enterstate(this);
    }
}
