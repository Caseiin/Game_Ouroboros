using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour, IHealth
{
    public EnemyBaseState currentState; //current context/state  of enemy
    public BaseEnemy enemy;
    //hello
    public e_patrollingState PatrolState;
    public e_ChaseState ChaseState;
    public e_CombatState AttackState;

    public int maxHealth, CurrentHealth = 4;

    SpriteRenderer enemySprite;

    Animator animator;
    private Dictionary<string, float> clipDurations;  // Holds durations of clips by name

    void Awake()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Load animation durations dynamically from Animator
        clipDurations = new Dictionary<string, float>();
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (!clipDurations.ContainsKey(clip.name))
            {
                clipDurations.Add(clip.name, clip.length);
            }
        }
    }

    public void Start()
    {
        PatrolState = new e_patrollingState();
        currentState = PatrolState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
    }

    public void TakeHealth()
    {
        CurrentHealth--;
        if (CurrentHealth <= 0)
        {
            Debug.Log("Enemy has died");
            //Dead
        }

        //FlashRed
        StartCoroutine(FlashRed());
    }

    public void GiveHealth()
    {
        Debug.Log("enemy is healing");
    }

    private IEnumerator FlashRed()
    {
        enemySprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        enemySprite.color = Color.white;
    }


    private IEnumerable DeathRoutine()
    {
        yield return null;
    }
}
