using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    EnemyStateManager stateManager;
    SpriteRenderer enemy;

    private HeartUIController heart;

    [Header("Melee parameters")]
    public int meleeDamage = 20;
    public float attackRadius = 2f;
    public float attackCooldown = 3f;
    bool CanAttack = true;

    bool isTimeStopped = false;
    bool AttackReady;

    //Animation stuff
    Animator animator;
    const string snakeAttack = "Snake enemy attack";
    const string snakeDies = "Snake enemy death";
    const string snakeReady = "Snake enemy walk";

    //animations durations:
    private Dictionary<string, float> clipDurations;


    e_MeleeState meleeState = new e_MeleeState();
    e_ChaseState chaseState = new e_ChaseState();

    protected override void Awake()
    {
        base.Awake();
        stateManager = GetComponent<EnemyStateManager>();
        enemy = GetComponent<SpriteRenderer>();
        stateManager.Start();
        stateManager.ChaseState = chaseState;
        stateManager.AttackState = meleeState;
        animator = GetComponent<Animator>();
        animator.Play(snakeReady);


        //Adding a dictionary of the clip durations
        clipDurations = new Dictionary<string, float>();
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            //Optional : remove duplicates
            if (!clipDurations.ContainsKey(clip.name))
            {
                clipDurations.Add(clip.name, clip.length);
            }
        }

        heart = FindFirstObjectByType<HeartUIController>();
        if (heart == null)
        {
            Debug.LogWarning("HeartUIController not found in the scene.");
        }
    }


    public float GetDuration(string clipName)
    {
        return clipDurations.TryGetValue(clipName, out float duration) ? duration : 0f;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void MeleeAttack()
    {

        if (CanAttack && !isTimeStopped && WithinCombatRange())
        {
            currentAttackCoroutine = StartCoroutine(AttackRoutine());
            heart.TakeHealth(1);
        }

    }

    public override void RangedAttack()
    {
        //ignore for melee enemies
    }

    public override bool WithinCombatRange()
    {
        AttackReady = false;
        if (PlayerDetected())
        {
            AttackReady = Physics2D.OverlapCircle(transform.position, attackRadius, LayerMask.GetMask("Player"));
        }
        return AttackReady;
    }

    public override bool PlayerDetected() //transition check to the combat state;
    {
        bool Detect = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));
        return Detect;
    }

    private void OnDrawGizmosSelected()
    {
        //Detection range 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        //Attack Radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

    }


    public override void StateCleanUp()
    {
        animator.Play(snakeReady);
    }

    private Coroutine currentAttackCoroutine;
    private IEnumerator AttackRoutine()
    {
        CanAttack = false;

        Debug.Log("Melee attack!");
        animator.Play(snakeAttack);
        float waitTime = GetDuration("Snake enemy attack");

        yield return new WaitForSeconds(waitTime);

        animator.Play(snakeReady);

        yield return new WaitForSeconds(attackCooldown);

        CanAttack = true;
        currentAttackCoroutine = null;
    }


    public override IEnumerator TimeStop()
    {
        Debug.Log("MeleeEnemy TimeStop");

        isTimeStopped = true;

        // Stop attack coroutine if it’s running
        if (currentAttackCoroutine != null)
        {
            StopCoroutine(currentAttackCoroutine);
            currentAttackCoroutine = null;
        }

        CanAttack = false;
        animator.enabled = false;
        enemyBody.linearVelocity = Vector2.zero;
        enemy.color = Color.blue;

        yield return new WaitForSeconds(3f);

        enemy.color = Color.white;
        animator.enabled = true;
        CanAttack = true;

        isTimeStopped = false;

        Debug.Log("Time resumes");

        yield return base.TimeStop(); // optional

    }

    public override IEnumerator SpikeDamage()
    {
        enemy.color = Color.black;
        yield return new WaitForSeconds(3f);

        enemy.color = Color.white;
        
    }


}
