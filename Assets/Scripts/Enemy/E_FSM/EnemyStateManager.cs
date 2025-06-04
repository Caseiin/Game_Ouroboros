using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState currentState; //current context/state  of enemy

    public  e_patrollingState PatrolState = new e_patrollingState();
    public e_CombatState AttackState = new e_CombatState();

    void Start()
    {
        currentState = PatrolState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }
}
