using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    public override void MeleeAttack()
    {
        Debug.Log("Melee attack!");
    }

    public override void RangedAttack()
    {

    }

    public override void PlayerDetected()
    {
        Debug.Log("Player detected!");
    }


}
