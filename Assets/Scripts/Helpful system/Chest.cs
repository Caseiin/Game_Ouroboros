using System;
using Mono.Cecil.Cil;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }
    public GameObject itemPrefab; //Item that chest drops
    public Sprite openedSprite; //make this public



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }


    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    void OpenChest()
    {
        //SetOpened
        SetOpened(true);

        //DropItem
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<Bounce>().StartBounce();
        }
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;
        if (IsOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;

        }
    }

}

// Code reference:
// 1) The chest logic is found: https://www.youtube.com/watch?v=MPP9GLp44Pc