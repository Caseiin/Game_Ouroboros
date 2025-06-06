using System;
using Mono.Cecil.Cil;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    //Common properties 
    [Header("Common Enemy Attributes")]
    public float patrolMovementRange = 5f;
    public float patrolSpeed = 3f;
    public float detectionRange = 5f;
    public float combatRange = 7f;


    private Vector3 targetPos;
    private Vector3 direction;
    protected bool isPatrolling = false;

    //Reference to state manager of enemy
    protected EnemyStateManager manager;
    protected Rigidbody2D enemyBody;
    protected virtual void Awake()
    {
        manager = GetComponent<EnemyStateManager>();
        enemyBody = GetComponent<Rigidbody2D>();

        if (enemyBody == null)
        {
            Debug.LogError("enemy rigidbody is null");
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
        targetPos = GetRandomPointForPatrol();
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
        //Implement how enemy patrols
        if (!isPatrolling) return; // patrol flag

        direction = (targetPos - transform.position).normalized;
        if (enemyBody != null)
            enemyBody.linearVelocity = direction * patrolSpeed;
        else
            Debug.LogError($"{name} is missing Rigidbody2D!");

        if ((targetPos - transform.position).sqrMagnitude < 0.01f)
        {
            targetPos = GetRandomPointForPatrol();
        }
    }

    public virtual void Chase()
    {
        Debug.Log($"{name} has started chasing!");
    }

    public virtual void StopChase()
    {
        Debug.Log($"{name} has stopped chasing!");
    }

    public virtual void StateCleanUp() { }

}

//  Code references:

//  The patrol logic was inspired by this video:https://www.youtube.com/watch?v=RQd44qSaqww