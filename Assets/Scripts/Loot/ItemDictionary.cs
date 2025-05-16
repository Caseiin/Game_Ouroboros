using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Items> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();

        //Auto increment ids
        for(int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i+1;
            }
        }

        foreach(Items item in itemPrefabs)
        {
          itemDictionary[item.ID] = item.gameObject; // assigning each gameobject a unique key for the dictionary
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out GameObject prefab); //ensures that game does not crash if player tries to retrieve Item out of inventory boundary
        if (prefab == null)
        {
            //if itemID within boundary prefab will not be null 
            Debug.LogWarning($"Item with ID {itemID} not found in dictionary");
        }

        return prefab;
    }
}
