using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public int[,] shopItem = new int[5, 5]; //no of shop items
    public float coins;
    public TextMeshProUGUI  CoinstTxt;

    void Start()
    {
        CoinstTxt.text = "Coins:" + coins.ToString();

        //Initialise array This is very simple
        shopItem[1, 1] = 1; //column IDS
        shopItem[1, 2] = 2;
        shopItem[1, 3] = 3;
        shopItem[1, 4] = 4;

        //Price
        shopItem[2, 1] = 10;
        shopItem[2, 2] = 20;
        shopItem[2, 3] = 30;
        shopItem[2, 4] = 40;

        //Quantity
        shopItem[3, 1] = 0;
        shopItem[3, 2] = 0;
        shopItem[3, 3] = 0;
        shopItem[3, 4] = 0;
    }

    public void Buy()
    {
        GameObject Buttonref = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItem[2, Buttonref.GetComponent<ButtonInfo>().ItemID])
        {
            //subtracting coins from the price
            coins -= shopItem[2, Buttonref.GetComponent<ButtonInfo>().ItemID];
            //increase quantity
            shopItem[3, Buttonref.GetComponent<ButtonInfo>().ItemID]++;

            //Update coins to current available amount
            CoinstTxt.text = "Coins:" + coins.ToString();

            //Update quantity text 
            Buttonref.GetComponent<ButtonInfo>().QuantityTxt.text = shopItem[3, Buttonref.GetComponent<ButtonInfo>().ItemID].ToString(); 
        }
    }

}

// Code reference:
// 1) https://www.youtube.com/watch?v=Oie-G5xuQNA