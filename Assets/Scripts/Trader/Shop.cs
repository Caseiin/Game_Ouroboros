using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public int[,] shopItem = new int[6, 6]; //no of shop items
    public float coins;
    public TextMeshProUGUI CoinstTxt;
    public Canvas canvas;

    private InventoryController inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventoryController>();

        CoinstTxt.text = "Coins:" + coins.ToString();

        //Initialise array This is very simple
        shopItem[1, 1] = 1; //column IDS
        shopItem[1, 2] = 2;
        shopItem[1, 3] = 3;
        shopItem[1, 4] = 4;
        shopItem[1, 5] = 5;

        //Price
        shopItem[2, 1] = 10;
        shopItem[2, 2] = 20;
        shopItem[2, 3] = 30;
        shopItem[2, 4] = 40;
        shopItem[2, 5] = 50;

        //Quantity
        shopItem[3, 1] = 0;
        shopItem[3, 2] = 0;
        shopItem[3, 3] = 0;
        shopItem[3, 4] = 0;
        shopItem[3, 5] = 0;
    }

    public void Buy()
    {
        GameObject Buttonref = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        int itemID = Buttonref.GetComponent<ButtonInfo>().ItemID;
        int itemPrice = shopItem[2, itemID];

        if (coins >= itemPrice)
        {
            GameObject itemButton = Buttonref.GetComponent<ButtonInfo>().item;

            // Add item to inventory
            bool added = inventory.AddItem(itemButton);
            if (!added)
            {
                Debug.Log("Could not add item to inventory.");
                return; // Don't deduct coins or update UI if adding failed
            }

            coins -= itemPrice;
            shopItem[3, itemID]++;

            CoinstTxt.text = "Coins:" + coins.ToString();
            Buttonref.GetComponent<ButtonInfo>().QuantityTxt.text = shopItem[3, itemID].ToString();
        }
    }

    public void closeShop()
    {
        canvas.gameObject.SetActive(false);
    }

}

// Code reference:
// 1) https://www.youtube.com/watch?v=Oie-G5xuQNA