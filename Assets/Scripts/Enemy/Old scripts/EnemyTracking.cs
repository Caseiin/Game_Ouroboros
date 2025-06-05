using UnityEngine;
using UnityEngine.AI;

public class EnemyTracking : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    private Transform meleeLocalScale;

    [Header("Movement Flipping")]
    public float flipThreshold = 0.1f; // Minimum movement to trigger flip
    private bool isFacingRight = true;

    [Header("Rotation")]
    public float rotationSpeed = 5f;
    public bool rotateTowardsTarget = true; // Toggle rotation if needed

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        meleeLocalScale = GetComponentInChildren<Transform>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (target == null)
        {
            Debug.LogError("Assign target in inspector!");
        }

        if (meleeLocalScale == null)
        {
            Debug.LogError("Melee is not a child of the enemy");
        }
            
        
    }

    void Update()
    {
        if (target == null) return;

        agent.SetDestination(target.position);
        HandleSpriteFlipping();
        if (rotateTowardsTarget)
        {
            RotateTowardsTarget();
        }
        
    }

    void HandleSpriteFlipping()
    {
        // Flip based on horizontal movement direction
        if (agent.velocity.x > flipThreshold && !isFacingRight)
        {
            FlipSprite();
        }
        else if (agent.velocity.x < -flipThreshold && isFacingRight)
        {
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = meleeLocalScale.localScale;
        scale.x *= -1;
        meleeLocalScale.localScale = scale;
    }

    void RotateTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust angle based on current flip
        if (!isFacingRight) angle += 180f;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            rotationSpeed * Time.deltaTime
        );
    }
}