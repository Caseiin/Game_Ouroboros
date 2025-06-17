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

    Rigidbody2D rangedrb;


    //Animation constants
    Animator animator;
    const string Attack = "Golem_Attack";
    const string Death = "Golem_Death";
    const string Ready = "Golem_Ready";

    [Header("Ranged enemy settings")]
    public GameObject projectile;

    public float shootCooldown = 5f;
    public float firepower = 15f;
    public int rangedDamage = 1;
    protected bool canShoot = true;
    private bool isTimeStopped = false;
    private Coroutine currentAttackCoroutine;


    protected override void Awake()
    {
        base.Awake();
        stateManager = GetComponent<EnemyStateManager>();
        archer = GetComponent<SpriteRenderer>();
        stateManager.Start();
        stateManager.ChaseState = chaseState;
        stateManager.AttackState = rangedState;
        animator = GetComponent<Animator>();
        animator.Play(Ready);
        rangedrb = GetComponent<Rigidbody2D>();

    }

    protected override void Update()
    {
        base.Update();
        Debug.Log("Current state: " + stateManager.currentState);
        Debug.DrawRay(transform.position, direction * detectionRange, Color.cyan);
        Debug.Log("Direction: " + direction);
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
        if (AimAtPlayer() && canShoot && !isTimeStopped)
        {
            Debug.Log("Enemy shooting!");
            currentAttackCoroutine = StartCoroutine(ShootRoutine());
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
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("Player not found in AimAtPlayer");
            return false;
        }

        // Update direction here
        direction = (player.position - transform.position).normalized;

        float maxRayDistance = detectionRange;
        int playerLayerMask = LayerMask.GetMask("Player");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxRayDistance, playerLayerMask);
        Debug.DrawRay(transform.position, direction * maxRayDistance, Color.red, 0.5f);

        if (hit.collider != null)
        {
            Debug.Log("Ray hit: " + hit.collider.name);
            return true;
        }

        Debug.Log("Raycast did not hit player");
        return false;
    }

    private IEnumerator ShootRoutine()
    {
        canShoot = false;

        if (projectile == null)
        {
            Debug.Log("Enemy has no bullets");
            yield break;
        }


        rangedrb.linearVelocity = Vector2.zero;
        animator.Play(Attack);
        GameObject intBullet = Instantiate(projectile, transform.position, quaternion.identity);
        Rigidbody2D BulletRb = intBullet.GetComponent<Rigidbody2D>();

        if (BulletRb == null)
        {
            Debug.LogError("Bullet has no rigidbody");
            Object.Destroy(intBullet, 1f);
            yield break;
        }

        //Apply force on bullet
        BulletRb.AddForce(direction * firepower, ForceMode2D.Impulse);
        Debug.Log("Bullet is shot!");
        Debug.Log("shootDirection" + direction);

        //wait for cooldown duration
        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
        currentAttackCoroutine = null;
    }

    public override IEnumerator TimeStop()
    {
        Debug.Log("RangedEnemy TimeStop");

        isTimeStopped = true;

        // Stop attack coroutine if it’s running
        if (currentAttackCoroutine != null)
        {
            StopCoroutine(currentAttackCoroutine);
            currentAttackCoroutine = null;
        }

        canShoot = false;
        animator.enabled = false;
        rangedrb.linearVelocity = Vector2.zero;
        archer.color = Color.blue;

        yield return new WaitForSeconds(3f);

        archer.color = Color.white;
        animator.enabled = true;
        canShoot = true;

        isTimeStopped = false;

        Debug.Log("Time resumes");

        yield return base.TimeStop(); // optional
    }

    public override void StateCleanUp()
    {
        animator.Play(Ready);
    }
}
