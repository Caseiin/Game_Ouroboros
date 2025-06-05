using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    //Common properties 
    public float patrolSpeed = 3f;
    public float detectionRange = 5f;
    public float combatRange = 7f;


    //Reference to state manager of enemy
    protected EnemyStateManager manager;

    void Awake()
    {
        manager = GetComponent<EnemyStateManager>();
    }

    //Abstract methods (must be implemented)
    public abstract void MeleeAttack();
    public abstract void RangedAttack();
    public abstract void PlayerDetected();

    //Virtual methods (can be overrided)

    public virtual void StartPatrol()
    {
        Debug.Log($"{name} has started patrolling");
    }

    public virtual void StopPatrol()
    {
        Debug.Log($"{name} has stopped patrolling");
    }

    public virtual void PatrolPattern()
    {
        //Implement how enemy patrols

    }

    public virtual void EnterCombatMode()
    {
        Debug.Log($"{name} has entered attack mode");
    }

}
