
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour {

    public Item item;
    public Text itemText;
    public Image itemImage;

    public void SetItem(Item item)
    {
        this.item = item;
        SetupItemValues();
    }

    void SetupItemValues()
    {
        this.transform.Find("Name").GetComponent<Text>().text = item.ItemName;
        this.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Icons/" + item.ObjectSlug);
    }

    public void OnSelectItemButton()
    {
        InventoryController.instance.SetItemDetails(item, GetComponent<Button>());
    }
}
