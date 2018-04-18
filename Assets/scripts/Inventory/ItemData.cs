using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, 
    IPointerExitHandler, IPointerDownHandler
{
    public Item item;
    public int amount;
    public int slot;

    private MyInventory inv;
    public Tooltip tooltip;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<MyInventory>();
        tooltip = inv.GetComponent<Tooltip>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && item.Uuid.Equals(System.Guid.Empty) == false)
        {
            // parent of item -> slot, parent of slot -> slotPanel
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inv.slots[slot].transform);
        this.transform.position = inv.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetButton("Fire3") && Input.GetMouseButtonDown(0) && item.Uuid.Equals(System.Guid.Empty) == false)
        {
            inv.RemoveItem(item);
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
}
