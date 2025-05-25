using System.Collections;
using System.Threading;
using UnityEngine;

public class p_MovingState : PlayerBaseState
{
    Rigidbody2D player_rb;
    PlayerAttributes player;

    //animation variables
    Animator animator;
    string currentAnim;

    //animation movement states
    const string dash_left = "Player_dash_left";
    const string dash_right = "Player_dash_right";
    const string player_left = "Player_Move_left";
    const string player_right = "Player_Move_right";
    const string player_forward = "Player_Move_forward";
    const string player_back = "Player_Move_Back";
    const string player_idle = "Player_idle_forward";

    //movement variables
    float currentspeed;
    public Vector2 movedirection;


    //Dash variables
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] float idleTransitionDelay = 0.2f; // Adjust in Inspector
    float lastMovementTime;

    bool canDash = true;

    Vector2 lastNonZeroDirection = Vector2.right;

    public override void EnterState(PlayerStateManager playerState)
    {
        Debug.Log("Player now moving!");

        player_rb = playerState.GetComponent<Rigidbody2D>();
        player = playerState.GetComponent<PlayerAttributes>();
        animator = playerState.GetComponent<Animator>();
        currentspeed = player.playermovespeed;
        player.OnPlayerMovespeedChange += UpdateSpeed;
    }

    public override void UpdateState(PlayerStateManager playerState)
    {
        MoveBasic();
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            playerState.StartCoroutine(Dash(playerState));
        }

        // Check for attack
        if (Atk_Input())
        {
            playerState.SwitchState(playerState.combatState);
            return;
        }

        // Delay idle transition using time since last movement
        if (movedirection.sqrMagnitude < 0.01f && canDash)
        {
            if (Time.time - lastMovementTime > idleTransitionDelay)
            {
                playerState.SwitchState(playerState.idleState);
            }
        }
        else
        {
            lastMovementTime = Time.time; // Reset timer while moving
        }
    }

    public override void ExitState(PlayerStateManager playerState)
    {
        playerState.StopAllCoroutines();
        player.OnPlayerMovespeedChange -= UpdateSpeed;
    }

    bool Atk_Input()
    {
        bool hasMouseInput = Input.GetMouseButton(0) || Input.GetMouseButton(1); //checks for mouse input
        return hasMouseInput;
    }

    void UpdateSpeed(float newSpeed)
    {
        currentspeed = newSpeed;
    }

    void MoveBasic()
    {
        movedirection = Vector2.zero;
        //ChangeAnimation(player_idle);
        bool movedHorizontal = false;
        // Check horizontal first
        if (Input.GetKey(KeyCode.A))
        {
            movedirection.x -= 1;
            movedHorizontal = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movedirection.x += 1;
            movedHorizontal = true;
        }

        // Vertical movement (only if no horizontal)

        if (!movedHorizontal)
        {
            if (Input.GetKey(KeyCode.W)) movedirection.y += 1;
            if (Input.GetKey(KeyCode.S)) movedirection.y -= 1;
        }
        


        //Track last valid direction and movement time
        if (movedirection != Vector2.zero)
        {
            lastMovementTime = Time.time; //reset idle timer when moving
            UpdateMoveAnimation();
            lastNonZeroDirection = movedirection.normalized;
        }

        player_rb.linearVelocity = movedirection * currentspeed;

    }

    private IEnumerator Dash(PlayerStateManager playerState)
    {
        canDash = false;
        lastMovementTime = Time.time; // Reset idle timer when dashing
        float originalspeed = currentspeed;
        currentspeed = dashSpeed;
        Vector2 dashDirection = lastNonZeroDirection;

        //store pre-dash animation
        string preDashAnim = currentAnim;

        //  force animation change
        ChangeAnimation(dashDirection.x < 0 ? dash_left : dash_right);

        //freeze other animation during dash
        player_rb.linearVelocity = dashDirection * currentspeed;
        //Dash duration
        yield return new WaitForSeconds(dashDuration);
        //Restore movement
        currentspeed = originalspeed;

        //Force animation update
        if (movedirection != Vector2.zero)
        {
            //Use current movement direction for animation
            UpdateMoveAnimation();
        }
        else
        {
            //return to pre-dash animation if not moving
            ChangeAnimation(preDashAnim);
        }

        yield return new WaitForSecondsRealtime(dashCooldown);
        canDash = true;

    }


    void UpdateMoveAnimation()
    {
        if (movedirection.x < -0.1f) ChangeAnimation(player_left);
        else if (movedirection.x > 0.1f) ChangeAnimation(player_right);
        else if (movedirection.y > 0.1f) ChangeAnimation(player_forward);
        else if (movedirection.y < -0.1f) ChangeAnimation(player_back);
    }



    void ChangeAnimation(string newAnim)
    {

        if (currentAnim == newAnim) return;
        if (currentAnim == dash_left || currentAnim == dash_right) return;
        if (newAnim == player_idle && movedirection != Vector2.zero) return;        

        animator.Play(newAnim);
        currentAnim = newAnim; 
    }

}
