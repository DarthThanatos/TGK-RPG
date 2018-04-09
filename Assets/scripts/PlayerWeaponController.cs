
using UnityEngine;

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

    public Item EquipWeapon(Item itemToEquip)
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
        equippedWeapon.transform.SetParent(playerHand.transform);
        characterStats.AddStatBonus(itemToEquip.Stats);

        UIEventHandler.ItemEquipped(currentlyEquipedWeaponItem);
        UIEventHandler.StatsChanged();

        return previousWeaponItem;
    }

    public void UnequipCurrentWeapon(bool notify = false)
    {
        if (equippedWeapon != null)
        {
            characterStats.RemoveStatBonus(equippedWeapon.GetComponent<IWeapon>().Stats);
            Destroy(playerHand.transform.GetChild(0).gameObject);
            if(notify) UIEventHandler.StatsChanged();
            currentlyEquipedWeaponItem = null;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PerformWeaponAttack();
        }
    }

    public void PerformWeaponAttack()
    {
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
