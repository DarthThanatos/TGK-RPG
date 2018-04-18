
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 

public class InventoryUI : MonoBehaviour {


    public RectTransform inventoryPanel;
    public RectTransform scrollViewContent;

    InventoryUIItem itemContainer { get; set; }
    public Text itemsNumber;

    bool menuIsActive { get; set; }
    Item currentSelectedItem { get; set; }

    private Dictionary<System.Guid, GameObject> uuidToPrefab;

    void Start() {
        uuidToPrefab = new Dictionary<System.Guid, GameObject>();
        itemContainer = Resources.Load<InventoryUIItem>("UI/Item_Container");


        UIEventHandler.OnItemEquipped += ItemEquipped;
        UIEventHandler.OnItemUnequipped += ItemUneqipped;


    }

    private void ItemEquipped(Item item)
    {
        RemoveItemRepresentation(item);
    }

    private void ItemUneqipped(Item item)
    {
        AddItemRepresentation(item);
    }


    private void ItemRemoved(Item item)
    {
        RemoveItemRepresentation(item);
    }

    void UpdateItemNumbersUI()
    {
        int numberOfItems = InventoryController.instance.playerItems.Count;
    }

    private void RemoveItemRepresentation(Item item)
    {
        if (item == null) return;
        if (uuidToPrefab.ContainsKey(item.Uuid)) { 
            Destroy(uuidToPrefab[item.Uuid]);
            uuidToPrefab.Remove(item.Uuid);
            UpdateItemNumbersUI();
        }
    }

    private void AddItemRepresentation(Item item)
    {
        Debug.Log("Inventory UI adding item with uuid: " + item.Uuid + "Items in map: " + uuidToPrefab.Count);
        InventoryUIItem emptyItem = Instantiate(itemContainer);
        emptyItem.SetItem(item);
        emptyItem.transform.SetParent(scrollViewContent, false);
        uuidToPrefab[item.Uuid] = emptyItem.gameObject;
        UpdateItemNumbersUI();
    }

 
}
