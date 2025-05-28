using UnityEngine;

public class p_CombatState : PlayerBaseState
{
    //Private fields 
    bool isAttacking = false;
    float atkDuration = 0.3f;
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
    float firepower;
    PlayerAttributes playerAttributes;


    //Initialize method to inject dependencies
    public void Initialize(PlayerStateManager manager)
    {
        melee = manager.melee;
        aim = manager.aim;
        bullet = manager.bullet;
        firepower = manager.firepower;
        playerAttributes = manager.playerAttributes;
    }


    [SerializeField] float combatCooldown = 0.5f;
    float cooldownTimer;
    public override void EnterState(PlayerStateManager playerState)
    {
        Debug.Log("Player is attacking!");
        cooldownTimer = combatCooldown;
    }

    public override void UpdateState(PlayerStateManager playerState)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer<= 0 &&Move_input())
        {
            // if input detected move from idle state to moving state   
            playerState.SwitchState(playerState.movingState);
        }

    }

    public override void ExitState(PlayerStateManager playerState)
    {

    }
    
    bool Move_input()
    {
        //checks for key input
        bool hasKeyInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D);

        return hasKeyInput;
    }


}
