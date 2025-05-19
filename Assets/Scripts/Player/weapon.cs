using UnityEngine;

public class weapon : MonoBehaviour
{
    public int damage = 1;
    public enum weapontype {melee,bullet}
    public weapontype Weapontype;
    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyCombat1 enemyCombat = collision.GetComponent<EnemyCombat1>();
       if (enemyCombat != null)
        {
            bool hit = true;
            enemyCombat.TakeDamage(hit, damage);
        }
    }
}
