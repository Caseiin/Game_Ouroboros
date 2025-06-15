using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class B_AttackState : B_BaseState
{
    Slider slider;
    float attackLength;
    float atkTimer;
    Transform transform;
    GameObject projectile;
    bool canShoot;

    float shootCooldown;
    float firepower;


    public void Initialize(BossStateManager boss)
    {
        slider = boss.slider;
        attackLength = boss.AttackDuration;
        transform = boss.BossTransform;
        projectile = boss.bullet;
        shootCooldown = boss.attackCooldown;
        firepower = boss.firepower;

        atkTimer = 0f;
    }

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Enemy is attacking");
        atkTimer = 0f;
        canShoot = true;
    }

    public override void UpdateState(BossStateManager boss)
    {
        atkTimer += Time.deltaTime;

        if (canShoot)
        {
            canShoot = false;
            
            boss.StartShootCoroutine(ShootRoutine());
        }


        Debug.Log("Enemy attack duration:" + atkTimer);
        if (atkTimer >= attackLength)
        {
            boss.SwitchState(boss.proneState);
        }
    }

    public override void ExitState(BossStateManager boss)
    {
        
    }

    Vector2 direction
    {
        get
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                return (player.transform.position - transform.position).normalized;
            }
            return Vector2.right; // fallback direction
        }
    }


    // public  bool WithinCombatRange()
    // {
    //     return PlayerDetected();
    // }

    // public  bool PlayerDetected()
    // {
    //     bool Detect = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));
    //     return Detect;
    // }



    private void OnDrawGizmosSelected()
    {
        //Detection range 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3);
    }

    // private bool AimAtPlayer()
    // {

    //     float maxRayDistance = detectionRange;
    //     int playerLayer = LayerMask.GetMask("Player");

    //     bool aim = Physics2D.Raycast(transform.position, direction, maxRayDistance, playerLayer);

    //     Debug.DrawRay(transform.position, direction * maxRayDistance, Color.red, 0.5f);
    //     return aim;
    // }

    private IEnumerator ShootRoutine()
    {
        

        if (projectile == null)
        {
            Debug.Log("Enemy has no bullets");
            yield break;
        }



        GameObject intBullet = Object.Instantiate(projectile, transform.position, Quaternion.identity);
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
