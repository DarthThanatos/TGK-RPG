using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopKeeperUI : Interactable {

    [SerializeField] private RectTransform shopDialog;
    [SerializeField] private RectTransform shopkeeperScrollContent, playerScrollContent;
    [SerializeField] private Text balanceLabel, playerGoldLabel;
    [SerializeField] private Text logAreaTxt, tooltipText;
    [SerializeField] private RectTransform toolTipArea;

    private GameObject shopItemPrefab;
    private static bool isDialogActive = false;

    private Shopkeeper shopkeeper;
    private Dictionary<Guid, GameObject> shopGameObjects = new Dictionary<Guid, GameObject>();
    private Dictionary<Guid, GameObject> playerGameObjects = new Dictionary<Guid, GameObject>();

    private Dictionary<GameObject, Item> representationToItemMap = new Dictionary<GameObject, Item>();

    void Start()
    {
        EconomyEventHandel.OnBalanceChanged += OnBalanceChanged;
        EconomyEventHandel.OnPlayerGoldChanged += OnPlayerGoldChanged;
        shopItemPrefab = Resources.Load<GameObject>("UI/ShopItem");
    }

    private void OnPlayerGoldChanged()
    {
        playerGoldLabel.text = "Your gold: " + EconomySystem.instance.PlayerMoney.ToString();
    }

    private void OnBalanceChanged(int amount)
    {
        balanceLabel.text = "Balance: " + amount.ToString();
    }

    public void OpenShopDialog(Shopkeeper shopkeeper, List<Item> shopItems, float priceMultiplayer)
    {
        if(this.shopkeeper != null) this.shopkeeper.CancelTransaction();
        this.shopkeeper = shopkeeper;

        FillShopDialog(shopItems, priceMultiplayer);
        logAreaTxt.text = "Select items to buy/sell.";

        shopDialog.gameObject.SetActive(true);
        isDialogActive = true;
    }
    
    private void FillShopDialog(List<Item> shopItems, float priceMultiplier)
    {
        representationToItemMap.Clear();

        shopGameObjects.Clear();
        playerGameObjects.Clear();

        DestroyChildrenOfParent(shopkeeperScrollContent);
        DestroyChildrenOfParent(playerScrollContent);

        AddItemToParentContent(shopItems, shopkeeperScrollContent, shopGameObjects, priceMultiplier: 1);
        AddItemToParentContent(InventoryController.instance.playerItems, playerScrollContent, playerGameObjects, priceMultiplier: priceMultiplier);

        balanceLabel.text = "Balance: 0";
        playerGoldLabel.text = "Your gold: " + EconomySystem.instance.PlayerMoney.ToString();
    }

    public void RefreshShopDialog(List<Item> shopItems, float priceMultiplier)
    {
        FillShopDialog(shopItems, priceMultiplier);
        logAreaTxt.text = "Transaction made successfully.";

    }

    public void DisplayMessage(string msg)
    {
        logAreaTxt.text = msg;
    }

    private void DestroyChildrenOfParent(RectTransform parent)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }


    public void OnMakeTransactionBtnClicked()
    {
        List<Item> boughtItems, soldItems;
        boughtItems = shopGameObjects.Values.Where(x => toggleSelected(x)).Select(x => representationToItemMap[x]).ToList();
        soldItems = playerGameObjects.Values.Where(x => toggleSelected(x)).Select(x => representationToItemMap[x]).ToList();
        shopkeeper.MakeTransaction(boughtItems, soldItems);
    }


    private Boolean toggleSelected(GameObject itemRepresentation)
    {
        return itemRepresentation.transform.Find("Toggle").GetComponent<Toggle>().isOn;
    }

    private void AddItemToParentContent(List<Item> items, RectTransform parent, Dictionary<Guid, GameObject> gameObjects, float priceMultiplier)
    {
        foreach (Item item in items)
        {
            GameObject itemRepresentation = Instantiate(shopItemPrefab);
            itemRepresentation.transform.Find("ItemImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Icons/" + item.ObjectSlug); ;
            itemRepresentation.transform.Find("ItemName").GetComponent<Text>().text = item.ItemName;
            itemRepresentation.transform.Find("ItemPrice").GetComponent<Text>().text = "Costs: " + (item.ItemPrice * priceMultiplier).ToString();
            itemRepresentation.transform.SetParent(parent);

            itemRepresentation.transform.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener(
                (value) => {
                    OnShopItemSelectionStatusChanged(itemRepresentation, playerGameObjects.ContainsKey(item.Uuid), priceMultiplier);       
                 }  
            );   
            itemRepresentation.GetComponent<Button>().onClick.AddListener(delegate { OnShopItemClicked(itemRepresentation);  });

            EventTrigger evTrigger = itemRepresentation.AddComponent<EventTrigger>();

            EventTrigger.Entry tooltipOnEntry = new EventTrigger.Entry();
            tooltipOnEntry.eventID = EventTriggerType.PointerEnter;
            tooltipOnEntry.callback.AddListener((eventData) => TurnOnTooltip(itemRepresentation));

            EventTrigger.Entry tooltipOffEntry = new EventTrigger.Entry();
            tooltipOffEntry.eventID = EventTriggerType.PointerExit;
            tooltipOffEntry.callback.AddListener((eventData) => TurnOffTooltip(itemRepresentation));


            evTrigger.triggers.Add(tooltipOnEntry);
            evTrigger.triggers.Add(tooltipOffEntry);

            gameObjects[item.Uuid] = itemRepresentation;
            representationToItemMap[itemRepresentation] = item;
        }
    }

    private void OnShopItemSelectionStatusChanged(GameObject itemRepresentation, bool playerItem, float priceMultiplier)
    {
        Toggle toggle = itemRepresentation.transform.Find("Toggle").GetComponent<Toggle>();
        Item item = representationToItemMap[itemRepresentation];
        shopkeeper.ChangeBalance((int)(item.ItemPrice * priceMultiplier), toggle.isOn, playerItem);
    }

    private void OnShopItemClicked(GameObject itemRepresentation)
    {
        Toggle toggle = itemRepresentation.transform.Find("Toggle").GetComponent<Toggle>();
        toggle.isOn = !toggle.isOn;
    }

    public void CloseShopDialog()
    {
        shopDialog.gameObject.SetActive(false);
        isDialogActive = false;
        shopkeeper.CancelTransaction();
    }

    public void TurnOnTooltip(GameObject itemRepresentation)
    {
        Item item = representationToItemMap[itemRepresentation];

        tooltipText.text =
            item.ItemName + "\n\n" +
            item.Description + "\n\n" +
            (item.Stats != null ? item.Stats.Aggregate("", (acc, current) => acc + current.StatName + ": " + current.BaseValue + "\n") : "");

        float shopLeft = shopDialog.transform.position.x - shopDialog.rect.width / 2; //shopDialog.transform.position.x returns center coords for some reason
        float x =
            itemRepresentation.transform.position.x < shopLeft + shopDialog.rect.width / 2
            ? itemRepresentation.transform.position.x + itemRepresentation.gameObject.GetComponent<RectTransform>().rect.width
            : itemRepresentation.transform.position.x - toolTipArea.rect.width;

        float y = toolTipArea.transform.position.y;

        toolTipArea.transform.position = new Vector3(x, y, toolTipArea.transform.position.z);
        toolTipArea.gameObject.SetActive(true);
    }

    public void TurnOffTooltip(GameObject itemRepresentation)
    {
        toolTipArea.gameObject.SetActive(false);
    }

}
