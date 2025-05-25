using UnityEngine;

public class p_CombatState : PlayerBaseState
{
    [SerializeField] float combatCooldown = 0.5f;
    float cooldownTimer;
    public override void EnterState(PlayerStateManager playerState)
    {
        Debug.Log("Player is attacking!");
        cooldownTimer = combatCooldown;
    }

    public override void UpdateState(PlayerStateManager playerState)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer<= 0 &&Move_input())
        {
            // if input detected move from idle state to moving state   
            playerState.SwitchState(playerState.movingState);
        }

    }

    public override void ExitState(PlayerStateManager playerState)
    {

    }
    
    bool Move_input()
    {
        //checks for key input
        bool hasKeyInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D);

        return hasKeyInput;
    }


}
