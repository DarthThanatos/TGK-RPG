
using UnityEngine;
using UnityEngine.AI;

public class PlayerWeaponController : MonoBehaviour {

    public GameObject playerHand;

    Transform spawnProjectile;
    CharacterStats characterStats;

    public GameObject equippedWeapon { get; set; }
    private Item currentlyEquipedWeaponItem;
    IWeapon weaponComponent;

    void Start()
    {
        spawnProjectile = transform.Find("ProjectileSpawn");
        characterStats = GetComponent<Player>().characterStats;
    }


    public int NumberOfItemsWithName(string name)
    {
        return currentlyEquipedWeaponItem != null && currentlyEquipedWeaponItem.ItemName == name ? 1 : 0;
    }
    
    public bool HasItemEquiped(Item item)
    {
        return currentlyEquipedWeaponItem != null && item.Uuid == currentlyEquipedWeaponItem.Uuid;
    }

    public void EquipWeapon(Item itemToEquip)
    {

        Item previousWeaponItem = currentlyEquipedWeaponItem;
        UnequipCurrentWeapon();
        currentlyEquipedWeaponItem = itemToEquip;

        equippedWeapon = (GameObject)Instantiate(
            Resources.Load("Weapons/" + itemToEquip.ObjectSlug), 
            playerHand.transform.position, 
            playerHand.transform.rotation
        );


        weaponComponent = equippedWeapon.GetComponent<IWeapon>();

        if (equippedWeapon.GetComponent<IProjectileWeapon>() != null) {
            equippedWeapon.GetComponent<IProjectileWeapon>().projectileSpawn = spawnProjectile;
        }

    
        equippedWeapon.GetComponent<IWeapon>().Stats = itemToEquip.Stats;
        equippedWeapon.transform.SetParent(playerHand.transform, false);

        Animator animator = equippedWeapon.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }

        characterStats.AddStatBonus(itemToEquip.Stats);

        UIEventHandler.ItemEquipped(currentlyEquipedWeaponItem);
        UIEventHandler.StatsChanged();
    }


    public void UnequipCurrentWeaponIfMatches(Item item)
    {
        // it comes from the outside,
        // we must check if the publisher really wanted to remove a weapon
        // (only relevant when other equipment controllers are in place, like e.g. an armour controller)
        if (item == null || currentlyEquipedWeaponItem == null) return;
        if (currentlyEquipedWeaponItem.Uuid == item.Uuid)
        {
            UnequipCurrentWeapon();
        }
    }

    private void UnequipCurrentWeapon()
    {
        if (equippedWeapon != null)
        {
            characterStats.RemoveStatBonus(equippedWeapon.GetComponent<IWeapon>().Stats);
            Destroy(playerHand.transform.GetChild(0).gameObject);
            UIEventHandler.ItemUnequipped(currentlyEquipedWeaponItem);
            UIEventHandler.StatsChanged();
            currentlyEquipedWeaponItem = null;
            weaponComponent = null;
        }
    }

    public void OnTargetInteraction(NavMeshAgent playerNavMeshAgent)
    {
        if(weaponComponent != null)
        {
            weaponComponent.OnTargetInteraction(playerNavMeshAgent);
        }
        else
        {
            playerNavMeshAgent.stoppingDistance = 2f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //PerformWeaponAttack();
        }
    }

    public void PerformWeaponAttack()
    {
        if(weaponComponent != null)
            weaponComponent.PerformAttack(CalculateDamage());
    }

    private int CalculateDamage()
    {
        int damageToDeal = ((characterStats.GetStat(BaseStat.BaseStatType.Power).GetCalulatedStatValue() *2) + Random.Range(2, 8));
        damageToDeal += CalculateCrit(damageToDeal);
        return damageToDeal;
    }

    private int CalculateCrit(int damage)
    {
        if(Random.value <= .1f)
        {
            int critDamage = (int)( (damage) * Random.Range(.25f, .5f));
            return critDamage; 
        }
        return 0;
    }
}
