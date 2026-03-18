using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Random = UnityEngine.Random;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Transform originalParent;
    public CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 2f;

    private InventoryController inventoryController;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryController = InventoryController.Instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; //save og parent slot
        transform.SetParent(transform.root); //above other canvas layers
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f; //item becomes semi-transparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //item follows the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f; //item is no longer transparent

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>(); //slot where item is dropped
        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();
   

        if (dropSlot != null)
        {
            //is a slot under drop point
            if (dropSlot.currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetItem = dropSlot.currentItem.GetComponent<Item>();

                if (draggedItem.ID == targetItem.ID)
                {
                    targetItem.AddToStack(draggedItem.quantity);
                    originalSlot.currentItem = null;
                    Destroy(gameObject);
                }
                else
                {
                    //if slot has an item, swap items
                    dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }

             
            }
            else
            {
                originalSlot.currentItem = null;
            }

            //move item into drop slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            //no slot under drop point, send item back to original slot
            transform.SetParent(originalParent);
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //centers item in slot
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

//void DropItem(Slot originalSlot)
//    {

//        Item item = GetComponent<Item>();
//        int quantity = item.quantity;

//        if (quantity > 1)
//        {
//            item.RemoveFromStack();

//            transform.SetParent(originalParent);
//            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

//            quantity = 1;
//        }
//        else
//        {
//            originalSlot.currentItem = null;
//        }

//        //find player
//        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
//        if (playerTransform == null)
//        {
//            Debug.LogError("Missing 'Player' tag");
//            return;
//        }

//        //random drop position
//        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
//        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

//        //Instantiate drop item and bounce
//        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
//        Item droppedItem = dropItem.GetComponent<Item>();
//        droppedItem.quantity = 1;

//        dropItem.GetComponent<BounceEffect>().StartBounce();

//        //destroy ui item
//        if (quantity <= 1 && originalSlot.currentItem = null)
//        {
//            Destroy(gameObject);
//        }     

//     }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            SplitStack();
        }
    }

    private void SplitStack()
    {
        Item item = GetComponent<Item>();
        if (item == null || item.quantity <= 1) return;

        int splitAmount = item.quantity / 2;
        if (splitAmount <= 0) return;

        item.RemoveFromStack(splitAmount);

        GameObject newItem = item.CloneItem(splitAmount);

        if(inventoryController == null || newItem == null) return;

        foreach(Transform slotTransform in inventoryController.inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.currentItem == null)
            {
                slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }
        //if no empty slot, return to stack
        item.AddToStack(splitAmount);
        Destroy(newItem);
    }
}
