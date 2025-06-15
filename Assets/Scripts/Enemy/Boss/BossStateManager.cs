using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossStateManager : MonoBehaviour, IHealth
{
    B_BaseState currentState;

    public B_AttackState attackState = new B_AttackState();
    public B_CooldownState proneState = new B_CooldownState();

    //References
    public Slider slider;
    public float AttackDuration = 120f;
    public float ProneDuration = 20f;

    public float attackCooldown = 2f;

    public float moveSpeed = 2f;

    public int BossHealth = 10;
    public SpriteRenderer bossSprite;

    public float firepower = 8f;
    public Transform BossTransform;
    public Rigidbody2D rb;
    public GameObject bullet;



    void Awake()
    {
        BossTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        bossSprite = GetComponent<SpriteRenderer>();
        attackState.Initialize(this);
        proneState.Initialize(this);
        currentState = attackState;
        currentState?.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }


    public void SwitchState(B_BaseState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void StartShootCoroutine(IEnumerator coroutine)
    {
        //coroutine needs to be within monobehaviour scripts
        StartCoroutine(coroutine);
    }

    public void TakeHealth()
    {
        if (currentState == proneState)
        {
            BossHealth--;
            //Update slider
            UpdateSlider(BossHealth);

            if (BossHealth <= 0)
            {
                //Die
            }
        }
        else
        {
            Debug.Log("Boss cannot be hit in attack mode!");
        }



    }

    public void GiveHealth()
    {

    }

    public void UpdateSlider(int health)
    {
        Debug.Log("Slider is being updated!");
    }
}
