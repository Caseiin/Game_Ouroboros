using UnityEngine;
using UnityEngine.AI;

public class EnemyTracking : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Transform visual;

    public float rotationSpeed = 5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        visual = GetComponentInChildren<Transform>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (target == null)
        {
            Debug.LogError("Player is not set as target for the enemy in inspector");
            return;
        }

        if (visual == null)
        {
            Debug.LogError("Visual not assigned in inspector");
        }
    }

    void Update()
    {
        if (target == null || visual == null)
        {
            return;
        }

        agent.SetDestination(target.position);
        FlipSpriteBasedOnTarget();
    }

    void RotateTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate around Z axis
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void FlipSpriteBasedOnTarget()
    {
        // Direction to target in local space (before rotation affect it)
        Vector3 directionToTarget = transform.InverseTransformPoint(target.position);

        float threshold = 0.9f; //threshold to sop constant flipping in horizontal axis

        if (directionToTarget.x > threshold)
        {
            SetSpriteFacing(-1); //target faces right
        }
        else if (directionToTarget.x < -threshold)
        {
            SetSpriteFacing(1); //target faces left
        }


    }

    void SetSpriteFacing(int direction)
    {
        Vector3 localScale = visual.localScale;
        localScale.x = Mathf.Abs(localScale.x) * direction; // since sprite faces left by default
        visual.localScale = localScale;
    }
}

