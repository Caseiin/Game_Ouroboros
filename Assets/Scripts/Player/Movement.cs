using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    PlayerAttributes players;
    public Transform aim;
    bool isWalking;
    Rigidbody2D rb;
    TrailRenderer _trailRenderer;
    private Animator animator;
     
    //dash variables
    float _dashSpeed;


    bool _canDash=true;
    [Header("Dash Settings")]
    [SerializeField] float _dashSpeedMultiplier = 1f;
    [SerializeField] float _dashTime = 0.25f;
    [SerializeField] float _dashCooldown = 0.5f;
    

    Vector2 _dashDirection;
    void Awake()
    { 
        players = GetComponent<PlayerAttributes>();
        players.OnPlayerMovespeedChange += UpdateSpeed;
        rb= GetComponent<Rigidbody2D>();

    }
    
    Vector2 movedirection;
    Vector3 lastmovedirection;
    float currentspeed;
    bool DidCrounch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        currentspeed = players.playermovespeed;
        Debug.Log("Player speed: "+currentspeed);
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.time = _dashTime * 0.8f; // Trail lasts 80% of dash duration
        _trailRenderer.startWidth = 0.5f;
        _trailRenderer.endWidth = 0f;
        DidCrounch = false;
        isWalking = false;
    }
    // basic movement, dash movement and finally crouch movementcool
    // Update is called once per frame

    
    void UpdateSpeed(float speed)
    {
        currentspeed = speed;
    }


    void Update()
    {
        //isWalking =false;
        movedirection = Vector3.zero;
        
        MoveBasic();
        OnCrouch();
        if (isWalking)
        {
            //ensures that the melee faces the direction which the player is moving
            Vector3 meleevector = Vector3.left*movedirection.x+Vector3.down*movedirection.y;
            aim.rotation = Quaternion.LookRotation(Vector3.forward,meleevector);
        }
        else
        {
            lastmovedirection = movedirection;
            Vector3 meleevector = Vector3.left*lastmovedirection.x+Vector3.down*lastmovedirection.y;
            aim.rotation = Quaternion.LookRotation(Vector3.forward,meleevector);
        }

        if (Input.GetKeyDown(KeyCode.Space)&& _canDash)
        {
            //Determines the dash direction
            Vector2 desiredDirection = movedirection;
            if (desiredDirection == Vector2.zero)
            {
                desiredDirection = lastmovedirection;
            }
            
            if (desiredDirection != Vector2.zero)
            {
                _dashDirection = movedirection.normalized;
                StartCoroutine(Dash());
            }
        }

    }

    //dash coroutine
    private IEnumerator Dash()
    {
        _canDash = false;
        _trailRenderer.enabled = true; // Enable the trail renderer

        rb.linearVelocity = _dashDirection * _dashSpeedMultiplier * currentspeed; // Apply dash velocity

        yield return new WaitForSeconds(_dashTime); // Wait for dash duration

        rb.linearVelocity = Vector2.zero; // Reset velocity after dash
        _trailRenderer.enabled = false; // Disable the trail renderer

        yield return new WaitForSeconds(_dashCooldown); // Wait for cooldown
        _canDash = true; // Allow dashing again 

    }

    void OnCrouch()
    {
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (DidCrounch==false)
            {
                transform.localScale = new Vector3(0.5f,0.5f,1f);
                UpdateSpeed(currentspeed*0.5f);
            }
            else 
            {
                transform.localScale = new Vector3(1f,1f,1f);
                UpdateSpeed(players.playermovespeed);
            }

            DidCrounch = !DidCrounch;
        }
    }
    void MoveBasic()
    {

        if (Input.GetKey(KeyCode.A))
        {
            movedirection.x -=2;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movedirection.x +=2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Walk Forward", isWalking);
            movedirection.y +=2;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movedirection.y -=2;
        }

        movedirection.Normalize();
        //move the player
        //transform.position+= movedirection.normalized*currentspeed*Time.deltaTime;
        rb.linearVelocity = movedirection*currentspeed;
        isWalking = (movedirection != Vector2.zero);

    }


    void OnDisable()
    {
        // unsubscribe publisher and subcriber to avoid memory leakage
        players.OnPlayerMovespeedChange -= UpdateSpeed;
    }
}
