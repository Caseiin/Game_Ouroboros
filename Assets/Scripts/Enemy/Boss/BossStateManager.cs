using UnityEngine;

public class BossStateManager : MonoBehaviour
{
    B_BaseState currentState;

    B_AttackState attackState = new B_AttackState();
    B_CooldownState proneState = new B_CooldownState();

    void Awake()
    {
        currentState = attackState;
        currentState?.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }


    void SwitchState(B_BaseState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}
