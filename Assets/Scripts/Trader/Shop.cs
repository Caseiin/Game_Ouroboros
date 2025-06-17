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
    public Canvas Uicanvas;

    private InventoryController inventory;

    void Awake()
    {
        canvas.gameObject.SetActive(false);
    }

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

        canvas.gameObject.SetActive(true);


        GameObject Buttonref = EventSystem.current.currentSelectedGameObject;

        if (Buttonref == null)
        {
            Debug.LogError("No button is currently selected.");
            return;
        }

        ButtonInfo info = Buttonref.GetComponent<ButtonInfo>();
        if (info == null)
        {
            Debug.LogError("ButtonInfo component is missing.");
            return;
        }

        if (info.QuantityTxt == null)
        {
            Debug.LogError("QuantityTxt is not assigned in ButtonInfo.");
            return;
        }

        int itemID = info.ItemID;
        int itemPrice = shopItem[2, itemID];

        if (coins >= itemPrice)
        {
            GameObject itemButton = info.item;

            bool added = inventory.AddItem(itemButton);
            if (!added)
            {
                Debug.Log("Could not add item to inventory.");
                return;
            }

            coins -= itemPrice;
            shopItem[3, itemID]++;

            CoinstTxt.text = "Coins: " + coins.ToString();
            info.QuantityTxt.text = shopItem[3, itemID].ToString();
        }

    }

    public void closeShop()
    {
        canvas.gameObject.SetActive(false);
        Uicanvas.enabled = true;
    }

}

// Code reference:
// 1) https://www.youtube.com/watch?v=Oie-G5xuQNA