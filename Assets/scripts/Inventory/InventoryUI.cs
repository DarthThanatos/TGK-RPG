
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public RectTransform inventoryPanel;
    public RectTransform scrollViewContent;

    InventoryUIItem itemContainer { get; set; }

    bool menuIsActive { get; set; }
    Item currentSelectedItem { get; set; }


	void Start () {
        itemContainer = Resources.Load<InventoryUIItem>("UI/Item_Container");
        UIEventHandler.OnItemAddedToInventory += ItemAdded;
        inventoryPanel.gameObject.SetActive(false);
	}
	
    public void ItemAdded(Item item)
    {
        InventoryUIItem emptyItem = Instantiate(itemContainer);
        emptyItem.SetItem(item);
        emptyItem.transform.SetParent(scrollViewContent, false);
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
