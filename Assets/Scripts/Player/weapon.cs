using UnityEngine;

public class weapon : MonoBehaviour
{
    public int weapondamage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStateManager enemyState = other.gameObject.GetComponent<EnemyStateManager>();

            enemyState.TakeHealth(weapondamage);
        }

        if (other.CompareTag("Boss"))
        {
            BossStateManager bossState = FindFirstObjectByType<BossStateManager>();
            bossState.TakeHealth(weapondamage);
        }
    }
}
