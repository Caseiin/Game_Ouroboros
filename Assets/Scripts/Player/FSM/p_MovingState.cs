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
    
    float lastMovementTime;
    bool canDash = true;
    bool isDashing = false; // Track if currently dashing

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
        ReadMovementInput();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            playerState.StartCoroutine(Dash(playerState));
        }


        if (!isDashing)
        {
            MoveBasic();
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
            ChangeAnimation(player_idle);
        }

    }

    public override void ExitState(PlayerStateManager playerState)
    {
        playerState.StopAllCoroutines();
        player.OnPlayerMovespeedChange -= UpdateSpeed;
    }


    void UpdateSpeed(float newSpeed)
    {
        currentspeed = newSpeed;
    }
#region TransitionStateChecks
    bool Atk_Input()
    {
        bool hasMouseInput = Input.GetMouseButton(0) || Input.GetMouseButton(1); //checks for mouse input
        return hasMouseInput;
    }

    bool Move_input()
    {
        bool Moved = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D);
        return Moved;
    }
#endregion
#region  Direction/MoveInput
    void ReadMovementInput()
    {
        movedirection = Vector2.zero;

        if (Input.GetKey(KeyCode.A)) movedirection.x -= 1;
        if (Input.GetKey(KeyCode.D)) movedirection.x += 1;
        if (Input.GetKey(KeyCode.W)) movedirection.y += 1;
        if (Input.GetKey(KeyCode.S)) movedirection.y -= 1;

        if (movedirection != Vector2.zero)
        {
            movedirection.Normalize();
            lastNonZeroDirection = movedirection;
        }
    }

    Vector2 GetCurrentMovementDirection()
    {
        Vector2 direction = Vector2.zero;
        bool hasHorizontal = false;

        // Check horizontal first with priority
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;
            hasHorizontal = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;
            hasHorizontal = true;
        }

        // Only check vertical if no horizontal
        if (!hasHorizontal)
        {
            if (Input.GetKey(KeyCode.W)) direction.y += 1;
            if (Input.GetKey(KeyCode.S)) direction.y -= 1;
        }

        return direction.normalized;
    }
 #endregion
#region BasicMoveCode
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
            Debug.Log("move animation");
            lastMovementTime = Time.time; //reset idle timer when moving
            UpdateMoveAnimation();
            lastNonZeroDirection = movedirection.normalized;
        }

        player_rb.linearVelocity = movedirection * currentspeed;

    }
#endregion
#region DashCode
    private IEnumerator Dash(PlayerStateManager playerState)
    {
        canDash = false;
        isDashing = true;

        Vector2 dashDirection = GetCurrentMovementDirection();
        if (dashDirection == Vector2.zero) dashDirection = lastNonZeroDirection;

        float originalspeed = currentspeed;
        currentspeed = dashSpeed;

        //  force animation change
        string dashAnim = dashDirection.x < 0 ? dash_left : dash_right;
        ChangeAnimation(dashAnim);
        Debug.Log("dash animation");
        currentAnim = dashAnim;

        //freeze other animation during dash
        player_rb.linearVelocity = dashDirection * currentspeed;
        //Dash duration
        yield return new WaitForSeconds(dashDuration);

        //Restore movement
        currentspeed = originalspeed;
        isDashing = false;
        player_rb.linearVelocity = movedirection * currentspeed;

        yield return new WaitForSecondsRealtime(dashCooldown);
        canDash = true;

    }
#endregion
#region Animation
    void UpdateMoveAnimation()
    {
        if (isDashing) return; // Just in case
        if (movedirection.x < -0.1f) ChangeAnimation(player_left);
        else if (movedirection.x > 0.1f) ChangeAnimation(player_right);
        else if (movedirection.y > 0.1f) ChangeAnimation(player_forward);
        else if (movedirection.y < -0.1f) ChangeAnimation(player_back);
    }

    void ChangeAnimation(string newAnim)
    {

    // Block non-dash animations during dash
    if (isDashing)
    {
        bool isDashAnimation = (newAnim == dash_left || newAnim == dash_right);
        if (!isDashAnimation)
        {
            Debug.Log($"Blocked {newAnim} during dash");
            return;
        }
    }


        if (currentAnim == newAnim)
            return;

        Debug.Log($"Animation changed to: {newAnim}");
        animator.Play(newAnim);
        currentAnim = newAnim; 
    }
#endregion
}
