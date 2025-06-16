using System.Collections;
using UnityEngine;

public class B_DeathState : B_BaseState
{
    private Animator animator;
    private Sprite deathSprite;
    private SpriteRenderer deadRender;

    public bool interacted;

    public void Initialize(BossStateManager boss)
    {
        interacted = true;
        animator = boss.animator;
        deadRender = boss.bossSprite;
        deathSprite = boss.deathBoss;
    }

    public override void EnterState(BossStateManager boss)
    {
        boss.StartCoroutine(AfterDeath(boss));
    }

    public override void ExitState(BossStateManager boss)
    {
        // throw new System.NotImplementedException();
    }

    public override void UpdateState(BossStateManager boss)
    {
        // throw new System.NotImplementedException();
    }


    private IEnumerator AfterDeath(BossStateManager boss)
    {
        animator.Play(boss.DeathAnimationName);
        yield return new WaitForSeconds(boss.DeathAnimDuration);
        deadRender.sprite = deathSprite;
        animator.enabled = false;
    }
}
