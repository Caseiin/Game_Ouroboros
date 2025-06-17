
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; //save OG parent
        transform.SetParent(transform.root); //Above other canvas
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; //semi-transparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Get the drop slot (prioritize slot over item)
        Slots dropSlot = eventData.pointerEnter?.GetComponent<Slots>();
        if (dropSlot == null)
        {
            // If we hit an item, get its parent slot
            GameObject droppedObject = eventData.pointerEnter;
            if (droppedObject != null)
            {
                dropSlot = droppedObject.GetComponentInParent<Slots>();
            }
        }

        Slots originalSlot = originalParent.GetComponent<Slots>();

        // If dropping on a valid slot
        if (dropSlot != null)
        {
            // Get the item currently in the drop slot (if any)
            GameObject itemInDropSlot = dropSlot.currentItem;

            // Move our item to the drop slot
            transform.SetParent(dropSlot.transform);
            transform.localPosition = Vector3.zero;
            dropSlot.currentItem = gameObject;

            // If there was an item in the drop slot, move it to our original slot
            if (itemInDropSlot != null)
            {
                itemInDropSlot.transform.SetParent(originalSlot.transform);
                itemInDropSlot.transform.localPosition = Vector3.zero;
                originalSlot.currentItem = itemInDropSlot;
            }
            else
            {
                originalSlot.currentItem = null;
            }
        }
        else
        {
            //if where we dropping is not within the inventory

            if (!isWithinInventory(eventData.position))
            {
                //Drop our item
                DropItem(originalSlot);
            }
            else
            {
                // Return to original slot if no valid drop target
                transform.SetParent(originalParent);
                transform.localPosition = Vector3.zero;
            }

            GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Center items


        }


    }


    bool isWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(Slots originalSlot)
    {
        originalSlot.currentItem = null;

        // Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing Player tag");
            return;
        }


        //Random drop positon 
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPostion = (Vector2)playerTransform.position + dropOffset;

        //Instantiate drop
        GameObject dropItem =  Instantiate(gameObject, dropPostion, Quaternion.identity);
        dropItem.GetComponent<Bounce>().StartBounce();
        //Destroy UI one
        Destroy(gameObject);
    }

}

// Code references:
// 1) For the dropping of items from the inventory plus the bounce FX
// https://www.youtube.com/watch?v=L5phEpQooxw
