using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerItemCollect : MonoBehaviour
{
    InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Loot"))
        {
            Items item = collision.GetComponent<Items>();
            if (item != null)
            {
                int ID = item.ID;
                //add item to inventory
                bool itemAdded = inventoryController.AddItem(collision.gameObject);
                if (itemAdded)
                {
                    item.PickUp();
                    Destroy(collision.gameObject);

                }

                if (ID == 7)
                {
                    Application.Quit();
                }
            }
        }
    }
}
