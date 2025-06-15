using System;
using System.Collections;
using Mono.Cecil.Cil;
using UnityEditor.Callbacks;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IHealth
{
    //Health
    [Header("Health Settings")]
    [SerializeField] protected int maxHealth = 4;
    protected int currentHealth;

    //Common properties 
    [Header("Common Enemy Attributes")]
    public float patrolMovementRange = 5f;
    public float patrolSpeed = 3f;
    public float detectionRange = 5f;
    public float combatRange = 7f;
    public Transform target;
    protected float ChaseSpeed = 2f;



    private Vector3 randomPos;
    public Vector2 direction;

    protected Vector2 randomDirection;
    protected Vector2 chaseDirection;
    protected bool isPatrolling = false;

    //Reference to state manager of enemy
    protected EnemyStateManager manager;
    protected Rigidbody2D enemyBody;

    SpriteRenderer enemySprite;


    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        manager = GetComponent<EnemyStateManager>();
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();

        if (enemyBody == null)
        {
            Debug.LogError("enemy rigidbody is null");
        }
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            Debug.Log("player is not set as a target for enemy");
        }

        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
    }
    //Abstract methods (must be implemented)
    public abstract void MeleeAttack();
    public abstract void RangedAttack();
    public abstract bool PlayerDetected();

    public abstract bool WithinCombatRange();

    //Virtual methods (can be overrided)

    public virtual void StartPatrol()
    {
        isPatrolling = true;
        Debug.Log($"{name} has started patrolling");
        randomPos = GetRandomPointForPatrol();
    }

    private Vector3 GetRandomPointForPatrol()
    {
        //Get A random point to move to
        return transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * patrolMovementRange;
    }

    public virtual void StopPatrol()
    {
        isPatrolling = false;
        if (enemyBody != null)
        {
            enemyBody.linearVelocity = Vector2.zero;
            Debug.Log($"{name} has stopped patrolling");
        }
        else
            Debug.LogError($"{name} is missing Rigidbody2D!");
    }

    public virtual void PatrolPattern()
    {
        float PatrolDelay = 0.5f;
        float PatrolWait = 0f;
        //Implement how enemy patrols
        if (!isPatrolling) return; // patrol flag

        randomDirection = (randomPos - transform.position).normalized;
        if (enemyBody != null)
            enemyBody.linearVelocity = randomDirection * patrolSpeed;
        else
            Debug.LogError($"{name} is missing Rigidbody2D!");

        if ((randomPos - transform.position).sqrMagnitude < 0.01f)
        {
            PatrolWait += Time.deltaTime;
            Debug.Log("Enemy is waiting");

            if (PatrolWait >= PatrolDelay)
            {
                randomPos = GetRandomPointForPatrol();
            }

            
        }
    }

    public virtual void Chase()
    {
        Debug.Log($"{name} has started chasing!");

        if (target)
        {
            Vector3 targetdirection = (target.position - transform.position).normalized;
            chaseDirection = targetdirection;
            enemyBody.linearVelocity = chaseDirection * ChaseSpeed;
        }

    }

    public virtual void StopChase()
    {
        Debug.Log($"{name} has stopped chasing!");
        enemyBody.linearVelocity = Vector2.zero;
    }

    //function for animation clean ups
    public virtual void StateCleanUp() { }

    public virtual void TakeHealth(int damage)
    {
        currentHealth-= damage;
        Debug.Log($"{name} took damage! Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private void Die()
    {
        Debug.Log($"{name} has died");
    }

    public virtual void GiveHealth()
    {
        throw new NotImplementedException();
    }


    private IEnumerator FlashRed()
    {
        enemySprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        enemySprite.color = Color.white;

    }
}

//  Code references:

//  The patrol logic was inspired by this video:https://www.youtube.com/watch?v=RQd44qSaqww
// The chase logic was inspired by this video: https://www.youtube.com/watch?v=m1x9YFzTX2A