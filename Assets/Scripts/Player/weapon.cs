using UnityEngine;

public class weapon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IHealth>(out var enemyHealth))
        {
            enemyHealth.TakeHealth(); // Calls TakeHealth() once per hit
        }
    }
}
