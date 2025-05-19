using UnityEngine;

public class Attack_Player : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Combat combat = collision.GetComponent<Combat>();
            if (combat != null)
            {
                bool hit = true;
                Debug.Log("Enemy attacks player");
                combat.TakeDamage(hit, damage);
            }
        }
    }
}
