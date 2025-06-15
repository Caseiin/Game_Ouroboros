using UnityEngine;

public class weapon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStateManager enemyState = other.gameObject.GetComponent<EnemyStateManager>();

            enemyState.TakeHealth();
        }
    }
}
