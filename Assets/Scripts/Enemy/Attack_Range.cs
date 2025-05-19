using UnityEngine;

public class Attack_Range : MonoBehaviour
{
    public GameObject melee;

    void Start()
    {
        if (melee == null)
        {
            Debug.LogError("enemy melee object not in inspector");
            return;
        }

        melee.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            melee.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            melee.SetActive(false);
        }
    }
}
