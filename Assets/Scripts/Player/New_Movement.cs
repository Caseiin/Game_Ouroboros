using System;
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

        if (movedirection.magnitude >1)
        {
            movedirection.Normalize();
        }
        //move the player
       
        player_rb.linearVelocity = movedirection*currentspeed;
        isWalking = (movedirection != Vector2.zero); 
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
