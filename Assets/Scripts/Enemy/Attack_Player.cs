using UnityEngine;

public class Attack_Player : MonoBehaviour
{
    public int damage = 1;
    int Did_hit = 0;
    Attack_Range attack_Range;

    void Awake()
    {
        attack_Range = GameObject.Find("Attack_range").GetComponent<Attack_Range>();

        if (attack_Range == null)
        {
            Debug.Log("The script does not exist");
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Combat combat = collision.GetComponent<Combat>();
            if (combat != null && attack_Range.isAttacking && Did_hit == 0)
            {
                Did_hit++;
                bool hit = true;
                combat.TakeDamage(hit, damage);
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Did_hit = 0;
        }
    }
}
