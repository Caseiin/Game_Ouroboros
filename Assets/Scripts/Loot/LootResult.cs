using UnityEngine;

public class LootResult : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coin has been collected!");
            Destroy(gameObject);
        }
    }
}
