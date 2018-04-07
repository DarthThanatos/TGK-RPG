using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

    public GameObject playerHand;
    public GameObject equippedWeapon { get; set; }

    Transform spawnProjectile;
    IWeapon weaponComponent;
    CharacterStats characterStats;

    void Start()
    {
        spawnProjectile = transform.Find("ProjectileSpawn");
        characterStats = GetComponent<CharacterStats>();
    }

    public void EquipWeapon(Item itemToEquip)
    {
        if(equippedWeapon != null)
        {
            characterStats.RemoveStatBonus(equippedWeapon.GetComponent<IWeapon>().Stats);
            Destroy(playerHand.transform.GetChild(0).gameObject);
        }

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

        Debug.Log(weaponComponent.Stats[0]);
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
        weaponComponent.PerformAttack();
    }

}
