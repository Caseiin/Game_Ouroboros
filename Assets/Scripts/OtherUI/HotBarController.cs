using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HotBarController : MonoBehaviour
{
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10; // 1 - 0 on keyboard

    private ItemDictionary itemDictionary;
    private Key[] hotBarKeys;

    void Awake()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();

        hotBarKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            // if keypressed less than 9 its from 1 to 8 else 0
            hotBarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotBarKeys[i]].wasPressedThisFrame)
            {
                //Use Item
                UseITemInSlot(i);
            }
        }
    }

    void UseITemInSlot(int index)
    {
        Slots slot = hotbarPanel.transform.GetChild(index).GetComponent<Slots>();

        if (slot.currentItem != null)
        {
            //checks if current slot is not empty
            Items item = slot.currentItem.GetComponent<Items>();
            item.UseItem();
        }
    }


    //saves new inventory arrangement 
    public List<InventorySaveData> GetHotbarItem()
    {
        List <InventorySaveData> hotbarData = new List<InventorySaveData>();
        foreach (Transform slotTransfrom in hotbarPanel.transform)
        {
            Slots slot = slotTransfrom.GetComponent<Slots>();
            if (slot.currentItem != null)
            {
                Items items = slot.currentItem.GetComponent<Items>();
                hotbarData.Add(new InventorySaveData {itemID = items.ID, slotIndex = slotTransfrom.GetSiblingIndex()} );
                // populates inventory save list with new itemID, slot Index
                // GetSiblingIndex returns the index of gameobject in relation to other gameobjects under the same parent
            }
        }

        return hotbarData;
    }

    public void SetHotbarItem(List<InventorySaveData> hotbarSaves)
    {
        //clear out gameobject and inventory panel to avoid duplication

        foreach (Transform child in hotbarPanel.transform )
        {
            Destroy(child.gameObject);
            //destroys slots and objects within slots as they are child of the inventory panel
        }

        //create new slots 
        for (int i =0; i< slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        //Populate slots with saved Items
        foreach (InventorySaveData data in hotbarSaves)
        {
            if (data.slotIndex < slotCount)
            {
                //ensures the number of saved slot does not exceed available slots
                Slots slot = hotbarPanel.transform.GetChild(data.slotIndex).GetComponent<Slots>();
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

// Code references:
// 1) https://www.youtube.com/watch?v=CcfYUYgaBTw