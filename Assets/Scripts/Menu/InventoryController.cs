using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCounter;
    public GameObject [] itemPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i=0; i< slotCounter; i++)
        {
            Slots slots = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slots>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i],slots.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slots.currentItem = item;
            }
        }
    }


}
