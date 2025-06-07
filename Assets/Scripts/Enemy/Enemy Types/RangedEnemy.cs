using System.Collections;
using Unity.Mathematics;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedEnemy : BaseEnemy
{
    EnemyStateManager stateManager;
    SpriteRenderer archer;
    e_RangedState rangedState = new e_RangedState();
    e_ChaseState chaseState = new e_ChaseState();

    [Header("Ranged enemy settings")]
    public GameObject projectile;

    public float shootCooldown = 5f;
    public float firepower = 15f;
    public int rangedDamage = 1;
    protected bool canShoot = true;


    protected override void Awake()
    {
        base.Awake();
        stateManager = GetComponent<EnemyStateManager>();
        archer = GetComponent<SpriteRenderer>();
        stateManager.Start();
        stateManager.ChaseState = chaseState;
        stateManager.AttackState = rangedState;
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log("Current state: " + stateManager.currentState);
        //Debug.DrawRay(transform.position, direction * detectionRange, Color.cyan);
        //Debug.Log("Direction: " + direction);
    }

    public override void Chase()
    {
        //ignore since ranged enemies do not chase 
    }

    public override void MeleeAttack()
    {
        //ignore since ranged enemies dont melee
    }

    public override void RangedAttack()
    {
        if (AimAtPlayer()&& canShoot)
        {
            Debug.Log("Enemy shooting!");
            StartCoroutine(ShootRoutine());
        }
    }

    public override bool WithinCombatRange()
    {
        return PlayerDetected();
    }

    public override bool PlayerDetected()
    {
        bool Detect = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));
        return Detect;
    }



    private void OnDrawGizmosSelected()
    {
        //Detection range 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private bool AimAtPlayer()
    {

        float maxRayDistance = detectionRange;
        int playerLayer = LayerMask.GetMask("Player");

        bool aim = Physics2D.Raycast(transform.position, direction, maxRayDistance, playerLayer);

        Debug.DrawRay(transform.position, direction * maxRayDistance, Color.red, 0.5f);
        return aim;
    }

    private IEnumerator ShootRoutine()
    {
        canShoot = false;

        if (projectile == null)
        {
            Debug.Log("Enemy has no bullets");
            yield break;
        }



        GameObject intBullet = Instantiate(projectile, transform.position, quaternion.identity);
        Rigidbody2D BulletRb = intBullet.GetComponent<Rigidbody2D>();

        if (BulletRb == null)
        {
            Debug.LogError("Bullet has no rigidbody");
            Object.Destroy(intBullet, 2f);
            yield break;
        }

        //Apply force on bullet
        BulletRb.AddForce(direction * firepower, ForceMode2D.Impulse);
        Debug.Log("Bullet is shot!");
        Debug.Log("shootDirection" + direction);

        //wait for cooldown duration
        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
    }
}
