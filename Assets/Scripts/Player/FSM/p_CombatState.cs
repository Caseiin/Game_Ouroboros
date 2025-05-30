
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class p_CombatState : PlayerBaseState
{
    //Private fields 
    bool isAttacking = false;
    float atkDuration = 0.5f;
    float atkTimer = 0f;
    float shootCoolDown = 0.25f;
    float shootTimer = 1f;

    int maxhealth;
    int CurrentHealth;
    bool dead;

    //References
    GameObject melee;
    Transform aim;
    GameObject bullet;
    float firepower = 10f;
    PlayerAttributes playerAttributes;
    PlayerStateManager playerstate;
    Animator animator;
    //direction reference
    Vector3 combatDirection;


    // Combat animation

    const string UpAttack = "Attack_forward";
    const string DownAttack = "Attack_Back";
    const string RightAttack = "Attack_left";
    const string LeftAttack = "Attack_Right";
    const string CombatIdle = "Player_idle_forward";

    //Animation management
    string currentAnim;


    //Initialize method to inject dependencies
    public void Initialize(PlayerStateManager manager)
    {
        animator = manager.GetComponent<Animator>();
        melee = manager.melee;
        aim = manager.aim;
        bullet = manager.bullet;
        firepower = manager.firepower;
        playerAttributes = manager.playerAttributes;
    }


    [SerializeField] float combatCooldown = 2f;
    float cooldownTimer;
    public override void EnterState(PlayerStateManager playerState)
    {
        Debug.Log("Player is attacking!");
        cooldownTimer = combatCooldown;
        //ChangeAnimation(CombatIdle); //Start with combat idle
    }

    public override void UpdateState(PlayerStateManager playerState)
    {
        combatDirection = playerState.currentdirection;
        MeleeTimer();
        cooldownTimer -= Time.deltaTime;
        shootTimer += Time.deltaTime;

        if (!isAttacking && currentAnim != CombatIdle)
        {
            ChangeAnimation(CombatIdle);
        }

        if (cooldownTimer <= 0 || Move_input())
            {
                // if input detected move from idle state to moving state   
                playerState.SwitchState(playerState.movingState);
            }


        if (Input.GetMouseButton(0))
        {
            OncloseCombat();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRangedCombat();
        }
    }

    public override void ExitState(PlayerStateManager playerState)
    {
        if (isAttacking) EndAttack();
    }

    bool Move_input()
    {
        //checks for key input
        bool hasKeyInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D);

        return hasKeyInput;
    }


    void OncloseCombat()
    {
        if (!isAttacking)
        {
            melee.SetActive(true);
            isAttacking = true;
            ChangeAnimation(GetAttackAnim());
        }

    }


    void OnRangedCombat()
    {
        if (shootTimer > shootCoolDown)
        {
            //null check 
            if (bullet == null)
            {
                Debug.LogError("Bullet prefab not assinged");
                return;
            }

            Vector2 shootDirection = combatDirection.normalized;
            float BulletAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, BulletAngle);

            GameObject intBullet = Object.Instantiate(bullet, aim.position, bulletRotation);
            // obtain bullet rigidbody

            Rigidbody2D rb = intBullet.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Bullet has no rigidBody");
                Object.Destroy(intBullet);
            }

            //Apply force
            rb.AddForce(shootDirection * firepower, ForceMode2D.Impulse);

            //Rest timer after shooting
            shootTimer = 0f;
            Object.Destroy(intBullet, 2f);
        }
    }


    void MeleeTimer()
    {

        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if (atkTimer >= atkDuration)
            {
                EndAttack();
            }
        }

    }

    void EndAttack()
    {
        atkTimer = 0;
        isAttacking = false;
        melee.SetActive(false);
        ChangeAnimation(CombatIdle);
    }

    string GetAttackAnim()
    {
        bool isXDominant = Mathf.Abs(combatDirection.x) > Mathf.Abs(combatDirection.y);

        if (isXDominant)
        {
            return combatDirection.x < 0 ? RightAttack :LeftAttack ;
        }
        return combatDirection.y < 0 ?  UpAttack: DownAttack ;
    }

    void ChangeAnimation(string newAnim)
    {
        // Block non-attack animations during attack
        if (isAttacking)
        {
            //if attacking, only allow for attack animations
            bool isAttackAnim = newAnim == UpAttack || newAnim == DownAttack || newAnim == LeftAttack || newAnim == RightAttack;
            
            if (!isAttackAnim)
            {
                Debug.Log($"Blocked {newAnim} during attack");
                return;
            }
        }

        if (currentAnim == newAnim) return;

        Debug.Log($"Animation changed to: {newAnim}");
        animator.Play(newAnim);
        currentAnim = newAnim;
    }
}
