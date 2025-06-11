using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUI : MonoBehaviour
{
    public static ItemPickUI Instance { get; private set; }

    [Header("Popup settings")]
    public GameObject popupPrefab;
    public int maxPopups = 5;
    public float popupDuration = 3f;

    private readonly Queue<GameObject> activePopups = new();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple ItemPickUpUiManager instances dectected! Destroying the extra one.");
            Destroy(gameObject);
        }
    }

    public void ShowItemPickUp(string itemName, Sprite itemICon)
    {
        GameObject newPopup = Instantiate(popupPrefab, transform);
        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;
        Image itemImage = newPopup.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemImage)
        {
            itemImage.sprite = itemICon;
        }
        else
        {
            Debug.LogWarning("Sprite passed to ShowItemPickUp is null!");
        }

        activePopups.Enqueue(newPopup);

        //destroys pop when it exceeds the max popup counter
        if (activePopups.Count > maxPopups)
        {
            Destroy(activePopups.Dequeue());
        }


        //Fade out and destroy
        StartCoroutine(FadeOutAndDestroy(newPopup));
    }

    private IEnumerator FadeOutAndDestroy(GameObject popup)
    {
        yield return new WaitForSeconds(popupDuration);
        if (popup == null) yield break;

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for (float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popup == null) yield break;
            canvasGroup.alpha = 1f - timePassed; //changes the opaque of the popup
            yield return null;
        }

        Destroy(popup);
    }
}

//Code reference:
//1) https://www.youtube.com/watch?v=UV1OJ4Kg6wY&list=PLaaFfzxy_80HtVvBnpK_IjSC8_Y9AOhuP&index=12
