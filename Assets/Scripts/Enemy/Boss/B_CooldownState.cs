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
    private Animator animator;
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
        animator = boss.animator;
    }

    public override void EnterState(BossStateManager boss)
    {
        Debug.Log("Enemy is proned!");
        animator.Play("BossDamageStateIdle");
        proneTimer = 0;
    }

    public override void UpdateState(BossStateManager boss)
    {
        proneTimer += Time.deltaTime;

        if (proneTimer >= pronelength)
        {
            boss.SwitchState(boss.attackState);
        }
    }

    public override void ExitState(BossStateManager boss)
    {
        bossrb.linearVelocity = Vector2.zero;
    }



}
