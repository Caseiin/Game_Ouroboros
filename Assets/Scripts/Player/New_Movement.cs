using System;
using System.Collections;
using NUnit.Framework.Internal;
using UnityEngine;

public class New_Movement : MonoBehaviour
{
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
    }

    // Declaration of basic movement variables
    bool didCrounch;
    bool isWalking;
    float currentspeed;


    void Start()
    {
        currentspeed = players.playermovespeed;

    }

    // Update is called once per frame
    void Update()
    {
  
        
        moveBasic();
        OnCrouch();
    }

    void UpdateSpeed(float speed)
    {
        currentspeed = speed;
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

        if (movedirection.magnitude > 1)
        {
            movedirection.Normalize();
        }
        //move the player

        player_rb.linearVelocity = movedirection * currentspeed;
        isWalking = (movedirection != Vector2.zero);

        //Update animations based on walkDireection array 
        UpdateWalkDirection();
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
            if (didCrounch==false)
            {
                transform.localScale = new Vector3(0.5f,0.5f,1f);
                UpdateSpeed(0.5f*players.playermovespeed);
            }
            else 
            {
                transform.localScale = new Vector3(1f,1f,1f);
                UpdateSpeed(players.playermovespeed);
            }

            didCrounch = !didCrounch;
        }
    }

    void OnDisable()
    {
        players.OnPlayerMovespeedChange -= UpdateSpeed;
    }
}
