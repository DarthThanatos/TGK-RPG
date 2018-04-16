using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInventory : MonoBehaviour {

    public GameObject inventoryPanel;
    public GameObject slotPanel;
    ItemDatabase database;

    bool isActive = false;

    public GameObject inventorySlot;
    public GameObject inventoryItem;

    int slotAmount;
    private InventoryController invController;
    public List<Item> items;
    public List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        slotAmount = 16;
        database = GetComponent<ItemDatabase>();
        invController = GameObject.Find("Player").GetComponent<InventoryController>();

        items = invController.playerItems;
        Debug.Log("Drawing...");

        for(int i = 0; i < slotAmount; i++)
        {
            Debug.Log("Drawing " + i);
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }

        UIEventHandler.OnItemAddedToInventory += ItemAdded;
        AddItem("Sword_01");
        AddItem("Sword_01");
        AddItem("Sword_01");

        inventoryPanel.SetActive(false);

    }

    private void ItemAdded(Item item)
    {
        AddItem(item.ObjectSlug);
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

        // TODO: has to add this to model
        // if(itemToAdd.Stackable)
        if (CheckIfItemIsInInventory(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ItemName == itemToAdd.ItemName)
                {
                    // the only child in item is text with item amount
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Uuid.Equals(System.Guid.Empty) == true)
                {
                    Debug.Log("Adding item");
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().amount = 1;
                    itemObj.GetComponent<ItemData>().slot = i;
                    itemObj.transform.SetParent(slots[i].transform, false);
                    itemObj.transform.position = slots[i].transform.position;
                    itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Icons/" + itemToAdd.ObjectSlug);
                    itemObj.name = itemToAdd.ItemName;
                    break;
                }
            }
        }

        items.Add(itemToAdd);
        Debug.Log("Added: " + objectSlug + " to inventory");
       
    }

    bool CheckIfItemIsInInventory(Item item)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemName == item.ItemName)
                return true;
        }
        return false;
    }
}
