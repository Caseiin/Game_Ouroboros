using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI  PriceTxt;
    public TextMeshProUGUI  QuantityTxt;
    public GameObject ShopManager;

    // Update is called once per frame
    void Update()
    {
        PriceTxt.text = "Price: $" + ShopManager.GetComponent<Shop>().shopItem[2, ItemID].ToString();
        QuantityTxt.text = ShopManager.GetComponent<Shop>().shopItem[3, ItemID].ToString();

    }
}

// Code reference:
// 1) https://www.youtube.com/watch?v=Oie-G5xuQNA
