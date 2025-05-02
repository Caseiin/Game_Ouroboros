using UnityEngine;

public class weapon : MonoBehaviour
{
    public int damage = 1;
    public enum weapontype {melee,bullet}
    public weapontype Weapontype;
    void OnTriggerEnter2D(Collider2D collision)
    {
        //EnemyCombat enemyCombat = collision.GetComponent<EnemyCombat>();
       //if (enemyCombat != null)
        {
           // enemyCombat.TakeDamage(damage);
           // if (Weapontype == weapontype.bullet)
            {
           //     Destroy(gameObject);
            }
        }
    }
}
