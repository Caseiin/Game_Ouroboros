using System.Collections;
using System.Threading;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class p_MovingState : PlayerBaseState
{
    Rigidbody2D player_rb;
    PlayerAttributes player;

    //animation variables
    Animator animator;
    string currentAnim;

    //animation movement states

    //Dash animations
    const string dash_left = "Player_dash_left";
    const string dash_right = "Player_dash_right";

    const string dash_up = "Player_dash_up";
    const string dash_down = "Player_dash_down";
    //base move animations
    const string player_left = "Player_Move_left";
    const string player_right = "Player_Move_right";
    const string player_forward = "Player_Move_forward";
    const string player_back = "Player_Move_Back";
    const string player_idle = "Player_idle_forward";
    const string Crouch_left = "Crouch_left";
    const string Crouch_right = "Crouch_right";


    // Player control requirements
    private PlayerControls controls;
    private Vector2 moveInput;


    //movement variables
    float currentspeed;
    public Vector2 movedirection;

    //Crouch variables
    bool didCrouch = false;
    Vector2 crouchDirection;
    float preCrouchSpeed; // stores speed before crouching
    //Dash variables
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;

    //Dash slider reference
    Slider dashSlide;

    float lastMovementTime;
    bool canDash = true;
    bool isDashing = false; // Track if currently dashing

    Vector2 lastNonZeroDirection = Vector2.right;
    public void Initialize(PlayerStateManager playerState)
    {
        dashSlide = playerState.slider;
        if (dashSlide == null)
        {
            Debug.LogError("dash slider does not exist in the moving state");
        }
        else
        {
            dashSlide.value = 1f;
            Debug.Log("dash slider properly placed!");
        }
    }
    
    public override void EnterState(PlayerStateManager playerState)
    {
        controls = new PlayerControls();
        controls.Player.Enable();

        // Movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Dash (set dashPressed = true only briefly)
        controls.Player.Dash.performed += ctx => StartDash(playerState);

        // Crouch (toggle crouch directly)
        controls.Player.Crouch.performed += ctx => ToggleCrouch();

        player_rb = playerState.GetComponent<Rigidbody2D>();
        player = playerState.GetComponent<PlayerAttributes>();
        animator = playerState.GetComponent<Animator>();
        currentspeed = player.playermovespeed;
        player.OnPlayerMovespeedChange += UpdateSpeed;
    }



    public override void UpdateState(PlayerStateManager playerState)
    {

        // First check pause status and skip all logic if paused
        if (PauseController.isGamePaused)
        {
            Pauselogic();  // Ensure pause effects are applied
            return;        // Skip all other update logic
        }

        if (!isDashing)
        {
            MoveBasic();
        }

        // Check for attack
        if ((Input.GetMouseButton(0) || Input.GetMouseButtonDown(1)) && !PauseController.isGamePaused && !isDashing && !didCrouch)
        {
            playerState.SwitchState(playerState.combatState);
            return;
        }

        // Delay idle transition using time since last movement
        if (movedirection.sqrMagnitude < 0.01f && canDash)
        {
            if (didCrouch)
            {
                string crouchIdle = (lastNonZeroDirection.x < 0) ? Crouch_left : Crouch_right;
                ChangeAnimation(crouchIdle);
            }
            else
            {
                ChangeAnimation(player_idle);
            }
            player_rb.linearVelocity = Vector2.zero;
        }

    }

    public override void ExitState(PlayerStateManager playerState)
    {
        controls.Player.Disable(); // <--- Add this
        playerState.StopAllCoroutines();
        UpdateSpeed(0f);
        player.OnPlayerMovespeedChange -= UpdateSpeed;
    }


    void UpdateSpeed(float newSpeed)
    {
        currentspeed = newSpeed;
    }


    void ToggleCrouch()
    {
        if (!didCrouch) // Entering crouch
        {
            preCrouchSpeed = currentspeed;
            currentspeed *= 0.5f;
            canDash = false;

            string crouchAnim = (lastNonZeroDirection.x < 0) ? Crouch_left : Crouch_right;
            ChangeAnimation(crouchAnim);
        }
        else // Exiting crouch
        {
            currentspeed = preCrouchSpeed;
            canDash = true;

            if (movedirection != Vector2.zero)
                UpdateMoveAnimation();
            else
                ChangeAnimation(player_idle);
        }

        didCrouch = !didCrouch;
    }





    void MoveBasic()
    {
        movedirection = moveInput;
        if (movedirection != Vector2.zero)
        {
            movedirection.Normalize();
            lastNonZeroDirection = movedirection;
            UpdateMoveAnimation();
        }

        player_rb.linearVelocity = movedirection * currentspeed;

    }


    void StartDash(PlayerStateManager playerState)
    {
        if (!canDash || isDashing || didCrouch)  return;
        playerState.StartCoroutine(Dash(playerState));
    }

    private IEnumerator Dash(PlayerStateManager playerState)
    {
        canDash = false;
        isDashing = true;

        Vector2 dashDirection = moveInput;
        if (dashDirection == Vector2.zero) dashDirection = lastNonZeroDirection;

        float originalspeed = currentspeed;
        currentspeed = dashSpeed;

        // Set slider to empty immediately
        if (dashSlide != null)
        {
            dashSlide.value = 0f;
        }

        string dashAnim;
        //  force animation change
        if (Mathf.Abs(dashDirection.x) >= Mathf.Abs(dashDirection.y))
        {
            dashAnim = dashDirection.x < 0 ? dash_left : dash_right;
        }
        else
        {
            dashAnim = dashDirection.y < 0 ? dash_down : dash_up;
        }



        Debug.Log("dash animation");
        ChangeAnimation(dashAnim);

        //freeze other animation during dash
        player_rb.linearVelocity = dashDirection * currentspeed;
        //Dash duration
        yield return new WaitForSeconds(dashDuration);

        //Restore movement
        currentspeed = originalspeed;
        isDashing = false;
        player_rb.linearVelocity = movedirection * currentspeed;

        // Gradually refill slider during cooldown
        float timer = 0f;
        while (timer < dashCooldown)
        {
            // Add pause check in cooldown loop
            if (!PauseController.isGamePaused)
            {
                timer += Time.deltaTime;
                dashSlide.value = Mathf.Lerp(0f, 1f, timer / dashCooldown);
            }
            yield return null;

        }

        canDash = true;
        if (dashSlide != null)
        {
            dashSlide.value = 1f;
        }

    }

    #region Animation
    void UpdateMoveAnimation()
    {
        if (isDashing) return;

        if (didCrouch)
        {
            // Determine crouch direction based on movement or last direction
            Vector2 currentDir = movedirection != Vector2.zero ? movedirection : lastNonZeroDirection;
            string crouchAnim = currentDir.x < 0 ? Crouch_left : Crouch_right;
            ChangeAnimation(crouchAnim);
        }
        else
        {
            // Regular movement animations
            if (movedirection.x < -0.1f) ChangeAnimation(player_left);
            else if (movedirection.x > 0.1f) ChangeAnimation(player_right);
            else if (movedirection.y > 0.1f) ChangeAnimation(player_forward);
            else if (movedirection.y < -0.1f) ChangeAnimation(player_back);
        }
    }

    void ChangeAnimation(string newAnim)
    {
        // Prevent animation changes during pause
        if (PauseController.isGamePaused) return;

        // Block non-dash animations during dash
        if (isDashing)
        {
            bool isDashAnim = newAnim == dash_left || newAnim == dash_right || newAnim == dash_up || newAnim == dash_down;

            if (!isDashAnim) return; // Block non-dash animations during dash
        }


        if (currentAnim == newAnim) return;
        animator.Play(newAnim);
        currentAnim = newAnim;
    }
    #endregion
    void Pauselogic()
    {
        player_rb.linearVelocity = Vector2.zero;
        ChangeAnimation(player_idle);
    }


}
