
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int ID;
    public string Name;

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickUI.Instance != null)
        {
            ItemPickUI.Instance.ShowItemPickUp(Name, itemIcon);
        }
    }
}

//code references:
//1) https://www.youtube.com/watch?v=UV1OJ4Kg6wY&list=PLaaFfzxy_80HtVvBnpK_IjSC8_Y9AOhuP&index=12