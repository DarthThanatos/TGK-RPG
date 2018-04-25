
using UnityEngine;
using System;

public class Player : MonoBehaviour {

    public CharacterStats characterStats;
    public PlayerLevel playerLevel { get; set; }

    public int maxHealth = 100;

    private ParticleSystem woundSpotSystem;
    private AudioSource woundedSound;

    private int _currentHealth;
    public int CurrentHealth {
        get { return _currentHealth; }
        set {
            int prevHealth = _currentHealth;
            _currentHealth = Math.Min(value, maxHealth);
            if (prevHealth != _currentHealth)
                 UIEventHandler.HealthChanged( _currentHealth, maxHealth);
        }
    }


    void Start()
    {
        playerLevel = GetComponent<PlayerLevel>();

        woundSpotSystem = gameObject.transform.Find("WoundSpot").GetComponent<ParticleSystem>();
        woundedSound = GetComponent<AudioSource>();

        CurrentHealth = maxHealth;
        characterStats = new CharacterStats(10, 10, 10);
    }

    public void takeDamage(int amount)
    {
        woundSpotSystem.Play();
        woundedSound.Play();

        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            die();
        }
    }


    private void die()
    {
        Debug.Log("Player dead, restoring health of the Nameless One");
        CurrentHealth = maxHealth;
    }
}
