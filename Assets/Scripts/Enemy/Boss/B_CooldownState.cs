using UnityEngine;
using UnityEngine.UI;

public class B_CooldownState : B_BaseState
{
    Slider slider;
    float pronelength;
    float proneTimer;

    float proneMovespeed;

    Transform transform;
    Vector3 randomPos;
    SpriteRenderer bSprite;

    int bossHealth;


    Rigidbody2D bossrb;

    public void Initialize(BossStateManager boss)
    {
        slider = boss.slider;
        pronelength = boss.ProneDuration;
        proneTimer = 0;
        transform = boss.transform;
        bossrb = boss.rb;
        proneMovespeed = boss.moveSpeed;
        bossHealth = boss.BossHealth;
        bSprite = boss.bossSprite;
    }

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Enemy is proned!");
        proneTimer = 0;
    }

    public override void UpdateState(BossStateManager boss)
    {
        proneTimer += Time.deltaTime;
        PatrolPattern();

        if (proneTimer >= pronelength)
        {
            boss.SwitchState(boss.attackState);
        }
    }

    public override void ExitState(BossStateManager boss)
    {
        bossrb.linearVelocity = Vector2.zero;
    }

    public virtual void PatrolPattern()
    {
        float PatrolDelay = 0.5f;
        float PatrolWait = 0f;
        //Implement how enemy patrols
        randomPos = GetRandomPointForPatrol();

        Vector2 randomDirection = (randomPos - transform.position).normalized;
        if (bossrb != null)
            bossrb.linearVelocity = randomDirection * proneMovespeed;
        else
            Debug.LogError("boss is missing Rigidbody2D!");

        if ((randomPos - transform.position).sqrMagnitude < 0.01f)
        {
            PatrolWait += Time.deltaTime;

            if (PatrolWait >= PatrolDelay)
            {
                randomPos = GetRandomPointForPatrol();
            }


        }
    }

    private Vector3 GetRandomPointForPatrol()
    {
        //Get A random point to move to
        return transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * 2;
    }


}
