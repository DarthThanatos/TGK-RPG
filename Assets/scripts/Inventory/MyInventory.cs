
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MyInventory : MonoBehaviour {

    public GameObject inventoryPanel;
    public GameObject slotPanel;

    int occupiedSlotsAmount;
    public GameObject occupiedSlots;


    ItemDatabase database;

    bool isActive = false;

    public GameObject inventorySlot;
    public GameObject inventoryItem;

    int slotAmount;
    public List<GameObject> slots = new List<GameObject>();

    private Dictionary<GameObject, string> slotToStackableNameMap = new Dictionary<GameObject, string>();
    private Dictionary<GameObject, System.Guid> slotToNonStackableUuidMap = new Dictionary<GameObject, System.Guid>();

    private void Start()
    {
        slotAmount = 16;
        occupiedSlotsAmount = 0;
 
        setOccupiedSlots();
        database = GetComponent<ItemDatabase>();
        
        for(int i = 0; i < slotAmount; i++)
        {
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }

        UIEventHandler.OnItemAddedToInventory += ItemAdded;
        UIEventHandler.OnItemRemovedFromInventory += ItemRemoved;
        UIEventHandler.OnItemEquipped += ItemRemoved;
        UIEventHandler.OnItemUnequipped += ItemAdded;

        inventoryPanel.SetActive(false);

    }

    public void MoveItemsInSlots(GameObject oldSlot, GameObject newSlot, Item item)
    {
        if (item.stackable)
        {
            string name = slotToStackableNameMap[oldSlot];
            slotToStackableNameMap[newSlot] = name;
            slotToStackableNameMap.Remove(oldSlot);
        }
        else
        {
            System.Guid uuid = slotToNonStackableUuidMap[oldSlot];
            slotToNonStackableUuidMap[newSlot] = uuid;
            slotToNonStackableUuidMap.Remove(oldSlot);

        }
    }

    public void SwapItemsInSlots(GameObject slotOne, GameObject slotTwo, Item itemInSlotOne, Item itemInSlotTwo)
    {
        if (itemInSlotOne.stackable)
        {
            slotToStackableNameMap[slotTwo] = itemInSlotOne.ItemName;
            if(itemInSlotOne.stackable != itemInSlotTwo.stackable) slotToStackableNameMap.Remove(slotOne);
        }
        else
        {
            slotToNonStackableUuidMap[slotTwo] = itemInSlotOne.Uuid;
            if (itemInSlotOne.stackable != itemInSlotTwo.stackable) slotToNonStackableUuidMap.Remove(slotOne);
        }

        if (itemInSlotTwo.stackable)
        {
            slotToStackableNameMap[slotOne] = itemInSlotTwo.ItemName;
            if (itemInSlotOne.stackable != itemInSlotTwo.stackable) slotToStackableNameMap.Remove(slotTwo);
        }
        else
        {
            slotToNonStackableUuidMap[slotOne] = itemInSlotTwo.Uuid;
            if (itemInSlotOne.stackable != itemInSlotTwo.stackable) slotToNonStackableUuidMap.Remove(slotTwo);
        }
        Debug.Log("Stackables: " + slotToStackableNameMap.Aggregate("", (agg, x) => agg + x.Key.GetComponent<Slot>().id + ": " + x.Value + ", " ));
        Debug.Log("Nontackables: " + slotToNonStackableUuidMap.Aggregate("", (agg, x) => agg + x.Key.GetComponent<Slot>().id + ": " + InventoryController.instance.UUIDToName(x.Value) + ", "));
    }


    private void ItemAdded(Item item)
    {
        AddItem(item);
    }

    private void ItemRemoved(Item item)
    {
        RemoveItem(item);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isActive = !isActive;
            inventoryPanel.gameObject.SetActive(isActive);
        }
    }

    public void AddItem(string objectSlug)
    {
        Item itemToAdd = database.GetNewInstanceOfItemWithSlug(objectSlug);
        AddItem(itemToAdd);
    }




    private int FirstEmptySlotIndex()
    {
        int i = 0;
        foreach(GameObject slot in slots)
        {
            if (IsSlotEmpty(slot)) return i;
            i++;
        }
        return -1;
    }

    public bool IsSlotEmpty(GameObject slot)
    {
        return !slotToStackableNameMap.ContainsKey(slot) && !slotToNonStackableUuidMap.ContainsKey(slot);
    }

    public void AddItem(Item itemToAdd)
    {
        
        
        if (itemToAdd.stackable && InventoryController.instance.CountItemsHavingName(itemToAdd.ItemName) > 1) // && CheckIfItemIsInInventory(itemToAdd)
        {
            GameObject slot = slotToStackableNameMap.Where(x => x.Value == itemToAdd.ItemName).First().Key;
            ItemData data = slot.transform.GetChild(0).GetComponent<ItemData>();
            data.transform.GetChild(0).GetComponent<Text>().text = InventoryController.instance.CountItemsHavingName(itemToAdd.ItemName).ToString();

            
        }
        else
        {
            int index = FirstEmptySlotIndex();
            GameObject itemObj = Instantiate(inventoryItem);

            if (itemToAdd.stackable)
            {
                slotToStackableNameMap[slots[index]] = itemToAdd.ItemName;
                itemObj.GetComponent<ItemData>().optionalItemName = itemToAdd.ItemName;
            }
            else
            {
                slotToNonStackableUuidMap[slots[index]] = itemToAdd.Uuid;
                itemObj.GetComponent<ItemData>().optionalItem = itemToAdd;
            }
            
            itemObj.GetComponent<ItemData>().slot = index;
            itemObj.transform.SetParent(slots[index].transform, false);
            itemObj.transform.position = slots[index].transform.position;
            itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Icons/" + itemToAdd.ObjectSlug);
            itemObj.name = itemToAdd.ItemName;
            occupiedSlotsAmount++;
            setOccupiedSlots();
        }
       
    }

    public void RemoveItem(Item item)
    {
        if (item.stackable)
            RemoveStackableItem(item.ItemName);
        else
            RemoveNonStackableItem(item);
    }

    void RemoveStackableItem(string itemName)
    {
        GameObject slot = slotToStackableNameMap.Where(x => x.Value == itemName).First().Key; 
        ItemData data = slot.transform.GetChild(0).GetComponent<ItemData>();
        int amount = InventoryController.instance.CountItemsHavingName(itemName);

        Debug.Log("Removing stackable item: amount: " + amount);

        if (amount >= 1)
        {
            data.transform.GetChild(0).GetComponent<Text>().text = amount.ToString();
        }
        else
        {
            Destroy(slot.transform.GetChild(0).gameObject);

            occupiedSlotsAmount--;
            setOccupiedSlots();

            // when we consume last stackable element, we have to turn off tooltip 
            if (data.tooltip != null) data.tooltip.Deactivate();

            slotToStackableNameMap.Remove(slot);
        }
    }

    void RemoveNonStackableItem(Item item)
    {
        GameObject slot = slotToNonStackableUuidMap.Where(x => x.Value == item.Uuid).First().Key; 
        ItemData data = slot.transform.GetChild(0).GetComponent<ItemData>();
        Destroy(slot.transform.GetChild(0).gameObject);

        occupiedSlotsAmount--;
        setOccupiedSlots();

        // when we dispose of a nonstackable element, we have to turn off tooltip 
        if (data.tooltip != null)
            data.tooltip.Deactivate();

        slotToNonStackableUuidMap.Remove(slot);

    }


    void setOccupiedSlots()
    {
        occupiedSlots.GetComponent<Text>().text = occupiedSlotsAmount + " / " + slotAmount;
    }
}