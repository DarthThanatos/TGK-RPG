
using UnityEngine;

public class UIEventHandler : MonoBehaviour {

    public delegate void ItemEventHandler(Item item);
    public static event ItemEventHandler OnItemAddedToInventory;
    public static event ItemEventHandler OnItemRemovedFromInventory;
    public static event ItemEventHandler OnRemoveItemFromInventory;
    public static event ItemEventHandler OnItemEquipped;
    public static event ItemEventHandler OnItemUnequipped;

    public delegate void PlayerHealthEventHandler(int currentHealth, int maxHealth);
    public static event PlayerHealthEventHandler OnPlayerHealthChanged;

    public delegate void StatsEventHandler();
    public static event StatsEventHandler OnStatsChanged;

    public delegate void PlayerLevelEventHandler();
    public static event PlayerLevelEventHandler OnPlayerLevelChanged;

    public static void ItemAddedToInventory(Item item)
    {
        if(OnItemAddedToInventory != null) OnItemAddedToInventory(item);
    }

    public static void ItemRemovedFromInventory(Item item)
    {
        if (OnItemRemovedFromInventory != null)
        {
            OnItemRemovedFromInventory(item);
        }
    }

    public static void RemoveItemFromInventory(Item item)
    {
        if(OnRemoveItemFromInventory != null)
        {
            OnRemoveItemFromInventory(item);
        }
    }

    public static void ItemEquipped(Item item)
    {
        if(OnItemEquipped != null) OnItemEquipped(item);
    }

    public static void ItemUnequipped(Item item)
    {
        if (OnItemUnequipped != null) OnItemUnequipped(item);
    }

    public static void HealthChanged(int currentHealth, int maxHealth)
    {
        if(OnPlayerHealthChanged!= null) OnPlayerHealthChanged(currentHealth, maxHealth);
    }


    public static void StatsChanged()
    {
        if(OnStatsChanged != null) OnStatsChanged();
    }

    public static void PlayerLevelChanged()
    {
        if(OnPlayerLevelChanged != null) OnPlayerLevelChanged();
    }

}
