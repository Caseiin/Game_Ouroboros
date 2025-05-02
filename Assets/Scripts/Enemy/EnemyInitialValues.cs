using UnityEngine;

public class EnemyInitialValues : MonoBehaviour
{
    private EnemyAttributes enemy;
    void Start()
    {
        enemy = GetComponent<EnemyAttributes>();
        if (enemy == null)
        {
            Debug.LogError("enemyattributes not in inspector");
            return;
        }

        enemy.EnemyMove = 4f;
        enemy.EnemyHealth = 4;
    }

}
