using UnityEngine;

public class TraderStateManager : MonoBehaviour
{
    TraderBaseState CurrentState; // current context of trader

    t_IdleState idleState = new t_IdleState();
    t_ShopState shopState = new t_ShopState();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentState = idleState;
        CurrentState.Enterstate(this);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.Updatestate(this);
    }
}
