using System;
using System.Collections;
//using System.Numerics;
using NUnit.Framework.Internal;
using UnityEngine;

public class New_Movement : MonoBehaviour
{
    // Audio variables
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private Coroutine footstepCoroutine;

    PlayerAttributes players;
    Rigidbody2D player_rb;
    private Animator animator;
    Vector2 movedirection;
    void Awake()
    {
        player_rb = GetComponent<Rigidbody2D>();
        players = GetComponent<PlayerAttributes>();
        animator = GetComponent<Animator>();
        players.OnPlayerMovespeedChange += UpdateSpeed;
        players.OnPlayerHeightChange += UpdateHeight;
        audioSource = GetComponent<AudioSource>();
    }

    // Declaration of basic movement variables
    bool didCrounch;
    bool isWalking;
    float currentspeed;
    int currentHeight;
    int DefaultHeight;

    //Dash variables
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;
    bool canDash = true;
    bool isDashing = false;
    Vector2 lastNonZeroDirection = Vector2.right;

    TrailRenderer dashTrail;
    [SerializeField] float trailFadeTime = 0.2f;

    void Start()
    {
        currentspeed = players.playermovespeed;
        currentHeight = players.playerheight;
        DefaultHeight = players.playerheight;
        dashTrail = GetComponent<TrailRenderer>();
        // Initialize trail renderer
        if (dashTrail != null)
        {
            dashTrail.emitting = false;
        }
        else
        {
            Debug.Log("Trailrender not connected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveBasic();
        OnCrouch();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void UpdateSpeed(float speed)
    {
        currentspeed = speed;
    }

    void UpdateHeight(int newHeight)
    {
        currentHeight = newHeight;
    }
    void moveBasic()
    {
        movedirection = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            movedirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movedirection.x += 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            movedirection.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movedirection.y -= 1;
        }

        if (movedirection != Vector2.zero)
        {
            lastNonZeroDirection = movedirection.normalized;
        }

        if (!isDashing)
        {
            if (movedirection.magnitude > 1)
            {
                movedirection.Normalize();
            }
            //move the player
            player_rb.linearVelocity = movedirection * currentspeed;

        }

        isWalking = (movedirection != Vector2.zero);

        //Update animations 
        UpdateWalkDirection();

        // handle footsteps sounds
        if (isWalking)
        {
            if (footstepCoroutine == null)
            {
                footstepCoroutine = StartCoroutine(PlayFootstepSounds());
            }
        }
        else
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
                audioSource.Stop(); //stop any ongoing sound
            }
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Activate trail renderer

        Vector2 dashDirection = lastNonZeroDirection;

        if (dashDirection.x < 0)
        {
            animator.SetInteger("Dash Direction", 1);
        }
        else if (dashDirection.x > 0)
        {
            animator.SetInteger("Dash Direction", 2);
        }
        else
        {
            if (dashTrail != null)
            {
                dashTrail.emitting = true; //sets trailrenderer in y direction
            }
        }

        player_rb.linearVelocity = dashDirection * dashSpeed;
        audioSource.PlayOneShot(audioClips[1]);
        yield return new WaitForSeconds(dashDuration);

        player_rb.linearVelocity = Vector2.zero;
        isDashing = false;

        // Fade out trail renderer
        if (dashTrail != null)
        {
            dashTrail.emitting = false;
            yield return new WaitForSeconds(trailFadeTime);
            dashTrail.Clear();
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        animator.SetInteger("Dash Direction", 0);
    }
    void UpdateWalkDirection()
    {
        //Determine walk direction and therefore right animation to use
        int directionKey = 0; //Default to idle state

        if (movedirection != Vector2.zero)
        {
            //Prioritze dominant axis
            if (Mathf.Abs(movedirection.x) > Mathf.Abs(movedirection.y))
            {
                directionKey = (movedirection.x > 0) ? 4 : 3; //Right(4) or left(3)
            }
            else
            {
                directionKey = (movedirection.y > 0) ? 1 : 2; //Up(1) or down(2)
            }
        }

        animator.SetInteger("Walk Direction", directionKey);

    }

    void OnCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (didCrounch)
            {
                animator.SetBool("Crouch", true);
                UpdateSpeed(0.5f * players.playermovespeed);
                UpdateHeight(2); // go to crounching height
                canDash = false;
            }
            else
            {
                animator.SetBool("Crouch", false);
                UpdateSpeed(players.playermovespeed);
                UpdateHeight(DefaultHeight); // return to standing height
                canDash = true;

            }

            didCrounch = !didCrounch; //toggle crouch
        }
    }

    void OnDisable()
    {
        players.OnPlayerMovespeedChange -= UpdateSpeed;
        players.OnPlayerHeightChange -= UpdateHeight;
    }

    private IEnumerator PlayFootstepSounds()
    {
        while (true)
        {
            audioSource.PlayOneShot(audioClips[0]);
            yield return new WaitForSeconds(0.6f); //Adjust interval to movement and sound
        }
    }
}
