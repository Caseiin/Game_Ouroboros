using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState; //current context/state  of enemy
    public BaseEnemy enemy;
    //hello
    public e_patrollingState PatrolState;
    public e_ChaseState ChaseState;
    public e_CombatState AttackState;

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
}
