using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    public Item item;
    public int amount;
    public int slot;

    private MyInventory inv;
    private Tooltip tooltip;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<MyInventory>();
        tooltip = inv.GetComponent<Tooltip>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
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
}
