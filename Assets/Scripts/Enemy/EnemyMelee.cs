using Unity.VisualScripting;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] PlayerAttributes playerAttributes;
    
    
    int damage=0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            damage++;
            Debug.Log("Damage to player:"+damage);
        }
    }

}
