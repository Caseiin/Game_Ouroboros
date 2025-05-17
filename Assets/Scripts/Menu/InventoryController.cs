using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCounter;
    public GameObject [] itemPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>(); // grab existing itemDictionary
/*         for (int i=0; i< slotCounter; i++)
        {
            Slots slots = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slots>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i],slots.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slots.currentItem = item;
            }
        } */
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slots slot = slotTransform.GetComponent<Slots>();
            if (slot != null && slot.currentItem == null)
            {
                //checks if there is a slot and if that slot is also empty
                GameObject newItem = Instantiate(itemPrefab,slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //ensures centred in the middle of slot
                slot.currentItem = newItem;
                return true;
            }
            
        }

        Debug.Log("Inventory is full");
        return false;
    }

    //saves new inventory arrangement 
    public List<InventorySaveData> GetInvItem()
    {
        List <InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTransfrom in inventoryPanel.transform)
        {
            Slots slot = slotTransfrom.GetComponent<Slots>();
            if (slot.currentItem != null)
            {
                Items items = slot.currentItem.GetComponent<Items>();
                invData.Add(new InventorySaveData {itemID = items.ID, slotIndex = slotTransfrom.GetSiblingIndex()} );
                // populates inventory save list with new itemID, slot Index
                // GetSiblingIndex returns the index of gameobject in relation to other gameobjects under the same parent
            }
        }

        return invData;
    }

    public void SetInvItem(List<InventorySaveData> inventorySaves)
    {
        //clear out gameobject and inventory panel to avoid duplication

        foreach (Transform child in inventoryPanel.transform )
        {
            Destroy(child.gameObject);
            //destroys slots and objects within slots as they are child of the inventory panel
        }

        //create new slots 
        for (int i =0; i< slotCounter; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        //Populate slots with saved Items
        foreach (InventorySaveData data in inventorySaves)
        {
            if (data.slotIndex < slotCounter)
            {
                //ensures the number of saved slot does not exceed available slots
                Slots slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slots>();
                //ensure we get the right slot which has an item saved in it
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab,slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }

            }
        }
    }
}
