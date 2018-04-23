using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, 
    IPointerExitHandler, IPointerDownHandler
{

    public string optionalItemName; //if optionalItem == null, assuming it holds stackable item name
    public Item optionalItem; //if itemName == null,  assuming it holds nonstackable item

    public int slot;

    private MyInventory myInv;
    public Tooltip tooltip;

    void Start()
    {
        myInv = GameObject.Find("Inventory").GetComponent<MyInventory>();
        tooltip = myInv.GetComponent<Tooltip>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && optionalItemName != null) //item.Uuid.Equals(System.Guid.Empty) == false
        {
            // parent of item -> slot, parent of slot -> slotPanel
            transform.SetParent(transform.parent.parent);
            transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (FetchItem() != null)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(myInv.slots[slot].transform);
        transform.position = myInv.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        Item item = FetchItem();
        if (item != null) tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Item item = FetchItem();
        if (item == null) return;
        if (Input.GetButton("Fire3") && Input.GetMouseButtonDown(0) && item.Uuid.Equals(System.Guid.Empty) == false)
        {
            InventoryController.instance.RemoveItem(item);
        }

        if (eventData.button == PointerEventData.InputButton.Right && item.Uuid.Equals(System.Guid.Empty) == false)
        {
            if (item.ItemType == Item.itemTypes.Weapon)
            {
                InventoryController.instance.EquipItem(item);
       
            }
            else if (item.ItemType == Item.itemTypes.Consumable)
            {
                InventoryController.instance.ConsumeItem(item);
            }
        }
    }

    public Item FetchItem()
    {
        return optionalItem != null ? optionalItem : InventoryController.instance.FindOneOfName(optionalItemName);
    }

}
