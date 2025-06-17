using Mono.Cecil;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool playerfound =false;
    
    public GameObject melee;
    private EnemyAttributes enemy;
    Transform target;
    Vector2 movedirection;
    Rigidbody2D rb;

    void Awake()
    {
        enemy = GetComponent<EnemyAttributes>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = false;
        target = GameObject.Find("Player").transform;
        melee = GameObject.Find("Melee");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerfound = true;
            melee.SetActive(true);
        }
    }




    void Update()
    {
        if (playerfound&& target != null)
        {
            FindTarget();
            RotateToTarget();
            MovetoPlayer();
        }
    }

    void FindTarget()
    {
        if (target)
        {
            Vector3 direction = (target.position-transform.position).normalized;
            movedirection = direction;
        }
    }

    void RotateToTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector2 targetDirection = target.position -transform.position;
        float angle = Mathf.Atan2(targetDirection.y,targetDirection.x)* Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f,0f,angle);
    }
    void MovetoPlayer()
    {
        rb.linearVelocity = new Vector2 (movedirection.x,movedirection.y)*enemy.EnemyMove;
    }
}
