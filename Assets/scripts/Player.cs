using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public CharacterStats characterStats;
    public float currentHealth;
    public float maxHealth = 100;

    void Start()
    {
        Debug.Log("Setting player health to " + maxHealth);
        currentHealth = maxHealth;
        characterStats = new CharacterStats(10, 10, 10);
    }

    public void takeDamage(int amount)
    {

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            die();
        }
    }


    private void die()
    {
        Debug.Log("Player dead, restoring health of the Nameless One");
        currentHealth = maxHealth;
    }
}
