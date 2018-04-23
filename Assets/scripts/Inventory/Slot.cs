using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {
    public int id;
    private MyInventory myInv;

    void Start()
    {
        myInv = GameObject.Find("Inventory").GetComponent<MyInventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>();

        if (myInv.IsSlotEmpty(gameObject))
        {
            myInv.MoveItemsInSlots(myInv.slots[droppedItemData.slot], gameObject, droppedItemData.FetchItem());
            droppedItemData.slot = id;
        }
        else if (droppedItemData.slot != id) //if not dropped on the same slot we started from
        {
            Transform itemTransform = transform.GetChild(0);
            ItemData currentItemData = itemTransform.GetComponent<ItemData>();

            Item currentItem = currentItemData.FetchItem();
            Item dropDataItem = droppedItemData.FetchItem();

            myInv.SwapItemsInSlots(gameObject, myInv.slots[droppedItemData.slot], currentItem, dropDataItem);
            
            itemTransform.transform.SetParent(myInv.slots[droppedItemData.slot].transform);
            itemTransform.transform.position = myInv.slots[droppedItemData.slot].transform.position;
            itemTransform.GetComponent<ItemData>().slot = droppedItemData.slot;

            droppedItemData.transform.SetParent(transform);
            droppedItemData.transform.position = transform.position;
            droppedItemData.slot = id;
        }
    }
}
