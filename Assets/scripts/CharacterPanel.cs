using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterPanel : MonoBehaviour {

    [SerializeField] private Text health, level;
    [SerializeField] private Image healthFill, levelFill;
    [SerializeField] private Player player;

    [SerializeField] private Text playerStatPrefab;
    [SerializeField] private Transform playerStatPanel;
    
    private List<Text> playerStatTexts;

    private PlayerWeaponController playerWeaponController;

    [SerializeField] private Sprite defaultWeaponSprite;
    [SerializeField] private Text weaponNameText;

    [SerializeField] private Text weaponStatPrefab;
    [SerializeField] private Transform weaponStatPanel;
    [SerializeField] private Image weaponIcon;

    [SerializeField] private Text goldAmountText;

    private List<Text> weaponStatTexts;

    private Item currentlyEquipedWeapon;

    void Start ()
    {
        playerWeaponController = player.GetComponent<PlayerWeaponController>();
        UIEventHandler.OnPlayerHealthChanged += UpdateHealth;
        UIEventHandler.OnPlayerLevelChanged += UpdateExperience;
        UIEventHandler.OnStatsChanged += UpdateStats;

        UIEventHandler.OnItemUnequipped += UnequipWeapon;
        UIEventHandler.OnItemEquipped += UpdateEquippedWeapon;

        UIEventHandler.OnItemRemovedFromInventory += UnequipWeapon;

        EconomyEventHandel.OnPlayerGoldChanged += OnPlayerGoldChanged;

        InitializeStats();
        InitializeUnequippedWeaponDetails();
        OnPlayerGoldChanged();
    }

    private void OnPlayerGoldChanged()
    {
        goldAmountText.text = "Gold: " + EconomySystem.instance.PlayerMoney.ToString();
    }

    void UpdateHealth(int currentHealth, int maxHealth)
    {
        int healthPercentage = (int)((float)(currentHealth) / maxHealth * 100);
        health.text = healthPercentage.ToString();
        healthFill.fillAmount = healthPercentage / 100f;
    }

    void UpdateExperience()
    {
        level.text = player.playerLevel.Level.ToString();
        levelFill.fillAmount = (float)(player.playerLevel.CurrentExperience) / player.playerLevel.RequiredExperience;
    }

    void InitializeStats()
    {
        playerStatTexts = new List<Text>();
        for (int i = 0; i < player.characterStats.stats.Count; i++)
        {
            Text instantiatedText = Instantiate(playerStatPrefab);
            instantiatedText.transform.SetParent(playerStatPanel,false);
            playerStatTexts.Add(instantiatedText);
        }
        UpdateStats();
    }

    void UpdateStats()
    {
        List<BaseStat> playerStats = player.characterStats.stats;
        for (int i = 0; i < playerStats.Count; i++)
        {
            Text playerStatText = playerStatTexts[i];
            playerStatText.text = playerStats[i].StatName + ": " + playerStats[i].GetCalulatedStatValue();
        }
    }
    
    void UpdateEquippedWeapon(Item item)
    {
        currentlyEquipedWeapon = item;
        weaponIcon.sprite = Resources.Load<Sprite>("UI/Icons/" + item.ObjectSlug);
        weaponNameText.text = item.ItemName;
        weaponStatTexts = new List<Text>();
        destroyWeaponPanelStats();
        for(int i = 0; i < item.Stats.Count; i++)
        {
            Text instantiatedText = Instantiate(weaponStatPrefab);
            instantiatedText.text = item.Stats[i].StatName + ": " + item.Stats[i].BaseValue;
            instantiatedText.transform.SetParent(weaponStatPanel, false);
            weaponStatTexts.Add(instantiatedText);
        }
    }


    public void  OnUnequipButtonClicked()
    {
        InventoryController.instance.UnequipItem(currentlyEquipedWeapon);
    }

    private  void UnequipWeapon(Item item)
    {
        if (currentlyEquipedWeapon != null && currentlyEquipedWeapon == item)
        {
            InitializeUnequippedWeaponDetails();
            currentlyEquipedWeapon = null;
        }
        
    }


    void destroyWeaponPanelStats()
    {
        for (int i = 0; i < weaponStatPanel.childCount; i++)
        {
            Destroy(weaponStatPanel.GetChild(i).gameObject);
        }
    }

    void InitializeUnequippedWeaponDetails()
    {
        destroyWeaponPanelStats();
        weaponNameText.text = "No weapon equipped";
        weaponIcon.sprite = defaultWeaponSprite;
    }


}
