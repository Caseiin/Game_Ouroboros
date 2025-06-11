using UnityEngine;

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
                //add item to inventory
                bool itemAdded = inventoryController.AddItem(collision.gameObject);
                if (itemAdded)
                {
                    item.PickUp();
                    Destroy(collision.gameObject);

                }
            }
        }
    }
}
