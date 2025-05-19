using UnityEngine;
using UnityEngine.AI;
public class EnemyTracking : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (target == null)
        {
            Debug.Log("Player is not set as target for the enemy in inspector");
            return;
        }
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);  
        }
        
    }
}
