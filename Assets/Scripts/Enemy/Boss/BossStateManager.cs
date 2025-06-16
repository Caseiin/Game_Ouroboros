using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStateManager : MonoBehaviour, IHealth, IInteractable
{
    B_BaseState currentState;

    public B_AttackState attackState = new B_AttackState();
    public B_CooldownState proneState = new B_CooldownState();
    public B_DeathState deathState = new B_DeathState();

    //References
    public Slider slider;
    public float AttackDuration = 120f;
    public float ProneDuration = 20f;

    public float attackCooldown = 2f;


    public float moveSpeed = 2f;

    public int BossHealth = 10;
    public SpriteRenderer bossSprite;
    public Sprite deathBoss;

    public float firepower = 8f;
    public Transform BossTransform;
    public Rigidbody2D rb;
    public GameObject bullet;


    private Dictionary<string, float> clipDurations;
    public Animator animator;


    public string DeathAnimationName, hitAnimationName;
    public float DeathAnimDuration, hitAnimationDuration;

    void Awake()
    {
        BossTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        bossSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attackState.Initialize(this);
        proneState.Initialize(this);
        deathState.Initialize(this);
        currentState = attackState;
        currentState?.EnterState(this);

        slider.maxValue = BossHealth;
        slider.value = BossHealth;

        //Adding a dictionary of the clip durations
        clipDurations = new Dictionary<string, float>();
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            //Optional : remove duplicates
            if (!clipDurations.ContainsKey(clip.name))
            {
                clipDurations.Add(clip.name, clip.length);

                Debug.Log($"Clip name: {clip.name}, Length: {clip.length}");

                if (clip.name.ToLower().Contains("death"))
                {
                    Debug.Log($"This clip seems to be a death animation: {clip.name}");
                    DeathAnimationName = clip.name;
                    DeathAnimDuration = clip.length;

                }

                if (clip.name.ToLower().Contains("hit"))
                {
                    Debug.Log($"This clip seems to be a hit animation: {clip.name}");
                    DeathAnimationName = clip.name;
                    DeathAnimDuration = clip.length;

                }
            }
        }
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

    public void TakeHealth(int damage)
    {
        if (currentState == proneState)
        {
            BossHealth -= damage;
            Debug.Log("Boss current hp:" + BossHealth);
            //Update slider
            UpdateSlider(BossHealth);
            StartCoroutine(hitRoutine());

            if (BossHealth <= 0)
            {
                //Die
                Die();
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
        slider.value = BossHealth;
    }

    public void Die()
    {
        SwitchState(deathState);
    }

    private IEnumerator hitRoutine()
    {
        bossSprite.color = Color.red;
        yield return new WaitForSeconds(1f);
        bossSprite.color = Color.white;
    }

    public void Interact()
    {
        
    }

    public bool CanInteract()
    {
        if (currentState == deathState)
        {
            return true;
        }

        return false;
    }
}
