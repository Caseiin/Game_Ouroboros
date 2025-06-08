using System.Collections;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    EnemyStateManager stateManager;
    SpriteRenderer enemy;

    [Header("Melee parameters")]
    public int meleeDamage = 20;
    public float attackRadius = 2f;
    public float attackCooldown = 3f;
    bool CanAttack = true;


    e_MeleeState meleeState = new e_MeleeState();
    e_ChaseState chaseState = new e_ChaseState();

    protected override void Awake()
    {
        base.Awake();
        stateManager = GetComponent<EnemyStateManager>();
        enemy = GetComponent<SpriteRenderer>();
        stateManager.Start();
        stateManager.ChaseState = chaseState;
        stateManager.AttackState = meleeState;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void MeleeAttack()
    {

        if (CanAttack && WithinCombatRange())
        {
            StartCoroutine(AttackRoutine());
        }

    }

    public override void RangedAttack()
    {
        //ignore for melee enemies
    }

    public override bool WithinCombatRange()
    {
        bool AttackReady = false;
        if (PlayerDetected())
        {
            AttackReady = Physics2D.OverlapCircle(transform.position, attackRadius, LayerMask.GetMask("Player"));
        }
        return AttackReady;
    }

    public override bool PlayerDetected() //transition check to the combat state;
    {
        bool Detect = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));
        return Detect;
    }

    private void OnDrawGizmosSelected()
    {
        //Detection range 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        //Attack Radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

    }


    public override void StateCleanUp()
    {

    }

    private IEnumerator AttackRoutine()
    {
        CanAttack = false;

        Debug.Log("Melee attack!");
        enemy.color = Color.red;

        yield return new WaitForSeconds(attackCooldown);

        enemy.color = Color.white;
        CanAttack = true;
    }

}
