using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IEnemy {

    public float currentHealth, power, thoughness;
    public float maxHealth;

    private CharacterStats characterStats;

    void Start()
    {
        characterStats = new CharacterStats(6,10,2);

        currentHealth = maxHealth;
    }

    public void performAttack()
    {
        throw new System.NotImplementedException();
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            die();
        }

    }

    void die()
    {
        Destroy(gameObject);
    }

}
