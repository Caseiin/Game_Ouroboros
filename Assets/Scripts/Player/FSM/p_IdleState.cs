using UnityEngine;

public class p_IdleState : PlayerBaseState
{
    //animation variables
    Animator animator;
    const string player_idle = "Player_idle_forward";

    public override void EnterState(PlayerStateManager playerState)
    {
        Debug.Log("player is idling right now!");
        animator = playerState.GetComponent<Animator>();
        if (playerState.movingState.movedirection == Vector2.zero)
        {
            animator.Play(player_idle);
        }
        else
        {
            playerState.SwitchState(playerState.movingState);
        }
    }

    public override void UpdateState(PlayerStateManager playerState)
    {
        if (Move_input())
        {
            // if input detected move from idle state to moving state   
            playerState.SwitchState(playerState.movingState);
        }
        else if (Atk_Input())
        {
            playerState.SwitchState(playerState.combatState);
        }
    }

    public override void ExitState(PlayerStateManager playerState)
    {

    }

    public bool Move_input()
    {

        //checks for key input
        bool hasKeyInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D);
        bool inputted = hasKeyInput;// || hasMouseInput;

        return inputted;
    }

    public bool Atk_Input()
    {
        bool hasMouseInput = Input.GetMouseButton(0) || Input.GetMouseButton(1); //checks for mouse input
        return hasMouseInput;
    }
}
