using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    EnemyStateManager stateManager;
    SpriteRenderer enemy;

    [Header("Melee parameters")]
    public int meleeDamage = 20;
    public float attackRadius = 2f;
    public float attackCooldown = 3f;
    bool CanAttack = true;

    //Animation stuff
    Animator animator;
    const string snakeAttack = "Snake Attack";
    const string snakeDies = "Snake dead";
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

        if (CanAttack && WithinCombatRange())
        {
            StartCoroutine(AttackRoutine());
        }

    }

    public override void RangedAttack()
    {
        //ignore for melee enemies
    }

    public override bool WithinCombatRange()
    {
        bool AttackReady = false;
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

    private IEnumerator AttackRoutine()
    {
        CanAttack = false;

        Debug.Log("Melee attack!");
        enemy.color = Color.red;
        animator.Play(snakeAttack);
        float waitTime = GetDuration("Snake enemy attack"); //use clipNames not state names

        yield return new WaitForSeconds(waitTime);

        animator.Play(snakeReady);
        yield return new WaitForSeconds(attackCooldown);

        enemy.color = Color.white;
        CanAttack = true;
    }

}
