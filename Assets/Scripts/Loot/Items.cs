
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int ID;
    public string Name;


    private HeartUIController heart;
    void Awake()
    {
        heart = FindFirstObjectByType<HeartUIController>();
        if (heart == null)
        {
            Debug.LogWarning("HeartUIController not found in the scene.");
        }
    }

    public virtual void UseItem()
    {
        Debug.Log("Using item" + Name);
        // Have other items inherit from this class then use this function

        heart.Heal();

    }

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