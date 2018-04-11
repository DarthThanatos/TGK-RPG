
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public RectTransform inventoryPanel;
    public RectTransform scrollViewContent;

    InventoryUIItem itemContainer { get; set; }
    public Text itemsNumber;

    bool menuIsActive { get; set; }
    Item currentSelectedItem { get; set; }


    void Start() {
        itemContainer = Resources.Load<InventoryUIItem>("UI/Item_Container");
        UIEventHandler.OnItemAddedToInventory += ItemAdded;
        UIEventHandler.OnItemRemovedFromInventory += ItemRemoved;
        UIEventHandler.OnRemoveItemFromInventory += RemoveItem;
        inventoryPanel.gameObject.SetActive(false);
    }

    public void RemoveItem(Item item)
    {
        for(int i = 0; i < scrollViewContent.childCount; i++)
        {
            string currentUiName = scrollViewContent.GetChild(i).Find("Name").GetComponent<Text>().text;
            Debug.Log("Checking " + currentUiName);
            if(currentUiName == item.ItemName)
            {
                Debug.Log("Destroying i = " + i + " name: " + currentUiName);
                Destroy(scrollViewContent.GetChild(i).gameObject);
                Destroy(scrollViewContent.GetChild(i));
                ItemRemoved();
                break;
            }
        }
    }

    public void ItemRemoved(Item item = null)
    {
        UpdateItemNumbersUI();
    }

    void UpdateItemNumbersUI()
    {
        itemsNumber.text = InventoryController.instance.playerItems.Count.ToString() + " items in the inventory";
    }

    public void ItemAdded(Item item)
    {
        InventoryUIItem emptyItem = Instantiate(itemContainer);
        emptyItem.SetItem(item);
        emptyItem.transform.SetParent(scrollViewContent, false);
        UpdateItemNumbersUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            menuIsActive = !menuIsActive;
            inventoryPanel.gameObject.SetActive(menuIsActive);
        }
    }
}
