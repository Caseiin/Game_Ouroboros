using System;
using System.Collections;
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
    bool[] walkDirection; //Up,Down,Left,Right

    void Start()
    {
        currentspeed = players.playermovespeed;
        walkDirection=new bool[] { false, false,false,false};
    }

    // Update is called once per frame
    void Update()
    {
        walkDirection=new bool[] { false, false,false,false};
        movedirection = Vector2.zero;
        moveBasic();
        OnCrouch();
    }

    void UpdateSpeed(float speed)
    {
        currentspeed = speed;
    }

    void moveBasic()
    {
        //Reset all directions at the start of frame
        walkDirection = new bool[4]; //[Up,Down,Left,Right]

        if (Input.GetKey(KeyCode.A))
        {
            movedirection.x -= 2;
            walkDirection[2] = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            walkDirection[1] = true;
            movedirection.x += 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            walkDirection[0] = true;
            movedirection.y += 2;

        }
        if (Input.GetKey(KeyCode.S))
        {
            walkDirection[3] = true;
            movedirection.y -= 2;
        }

        if (movedirection.magnitude > 1)
        {
            movedirection.Normalize();
        }
        //move the player

        player_rb.linearVelocity = movedirection * currentspeed;
        isWalking = (movedirection != Vector2.zero);

        //Update animations based on walkDireection array 
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        //First rest all animation parameters;
        
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
