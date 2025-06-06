    using UnityEngine;

    public class MeleeEnemy : BaseEnemy
    {
        EnemyStateManager stateManager;

        [Header("Melee parameters")]
        public int meleeDamage = 20;
        public float attackRadius = 2f;


        e_MeleeState meleeState = new e_MeleeState();
        e_ChaseState chaseState = new e_ChaseState();

        void Awake()
        {
            stateManager = GetComponent<EnemyStateManager>();
            stateManager.Start();
            stateManager.ChaseState = chaseState;
            stateManager.AttackState = meleeState;
        }

        public override void MeleeAttack()
        {
            Debug.Log("Melee attack!");
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRadius);            

        }
        

        public override void StateCleanUp()
        {

        }


    }
