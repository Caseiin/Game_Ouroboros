
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

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

    //Player Control
    public PlayerControls controls;

    //References
    GameObject melee;
    Transform aim;
    GameObject bullet;
    float firepower = 10f;
    PlayerAttributes playerAttributes;
    PlayerStateManager playerstate;
    Animator animator;

    BaseEnemy enemy;
    Transform transform;

    private SpriteRenderer sprite;
    private weapon weap;
    //direction reference
    Vector3 combatDirection;


    // Combat animation

    const string UpAttack = "Attack_forward";
    const string DownAttack = "Attack_Back";
    const string RightAttack = "Attack_left";
    const string LeftAttack = "Attack_Right";
    const string CombatIdle = "Player_idle_forward";

    const string Leftshoot= "Shoot left";
    const string Upshoot= "Shoot up";
    const string Downshoot= "Shoot down";
    const string Rightshoot= "Shoot right";

    //Animation management
    string currentAnim;


    private float shootspeed;
    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    //Initialize method to inject dependencies
    public void Initialize(PlayerStateManager manager)
    {
        animator = manager.GetComponent<Animator>();
        melee = manager.melee;
        aim = manager.aim;
        bullet = manager.bullet;
        firepower = manager.firepower;
        playerAttributes = manager.playerAttributes;

        enemy = manager.baseEnemy;
        transform = manager.transform;
        sprite = manager.playersprite;

        shootspeed = manager.shootspeed;

        // Get controls
        controls = manager.controls;

    }


    [SerializeField] float combatCooldown = 2f;
    float cooldownTimer;
    public override void EnterState(PlayerStateManager playerState)
    {
        Debug.Log("Player is attacking!");
        cooldownTimer = combatCooldown;

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


        if (Input.GetMouseButton(0) && !isPointerOnUI())
        {
            OncloseCombat();
        }
        if (Input.GetMouseButtonDown(1) && !isPointerOnUI())
        {
            OnRangedCombat();
        }
        //Debug.Log(EventSystem.current.IsPointerOverGameObject());
    }

    public override void ExitState(PlayerStateManager playerState)
    {
        if (isAttacking) EndAttack();
    }

    bool isPointerOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    bool Move_input()
    {
        Vector2 move = controls.Player.Move.ReadValue<Vector2>();
        bool result = move.magnitude > 0.1f || controls.Player.Crouch.ReadValue<float>() > 0 || controls.Player.Dash.ReadValue<float>() > 0;
        return result;
    }

    public bool EnemyDetected() //transition check to the combat state;
    {
        bool Detect = Physics2D.OverlapCircle(transform.position, 2, LayerMask.GetMask("Enemy"));
        return Detect;
    }

    #region Shooting
    void OnRangedCombat()
    {
        if (shootTimer > shootCoolDown)
        {
            SoundEffectManager.Play("Playershoot");
            //null check 
            if (bullet == null)
            {
                Debug.LogError("Bullet prefab not assinged");
                return;
            }

            Vector2 shootDirection = combatDirection.normalized;

            // bool isXDominant = Mathf.Abs(shootDirection.x) > Mathf.Abs(shootDirection.y);
            // string shootAnim = "";
                
            // if (isXDominant)
            // {
            //     shootAnim = shootDirection.x < 0 ? Rightshoot : Leftshoot;
            // }
            // else
            // {
            //     shootAnim= shootDirection.y < 0 ? Downshoot : Upshoot;
            // }

            // animator.Play(shootAnim);

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
    #endregion
    #region MeleeCombat

    void OncloseCombat()
    {
        if (!isAttacking)
        {
            melee.SetActive(true);
            SoundEffectManager.Play("Playermelee");
            isAttacking = true;
            ChangeAnimation(GetAttackAnim());
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

    #endregion
    #region CombatAnimation
    string GetAttackAnim()
    {
        bool isXDominant = Mathf.Abs(combatDirection.x) > Mathf.Abs(combatDirection.y);

        if (isXDominant)
        {
            return combatDirection.x < 0 ? RightAttack : LeftAttack;
        }
        return combatDirection.y < 0 ? UpAttack : DownAttack;
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

        // Debug.Log($"Animation changed to: {newAnim}");
        animator.Play(newAnim);
        currentAnim = newAnim;
    }

    public IEnumerator SwordRoutine()
    {
        sprite.color = Color.magenta;

        // weap can still be accessed and modified while inactive
        weap = melee.GetComponent<weapon>();
        weap.weapondamage = 4;

        yield return new WaitForSeconds(3f);

        weap.weapondamage = 1;
        sprite.color = Color.white;

    }
    #endregion

    //Sound coroutines

}
