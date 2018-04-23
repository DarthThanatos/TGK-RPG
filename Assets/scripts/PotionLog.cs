using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionLog : MonoBehaviour, IConsumable {
    public Player player;

    public void consume()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Debug.Log("player: " + player);
          player.CurrentHealth += 25;
        Debug.Log("You drank a swig of the potion. Cool!");
    }

    public void consume(CharacterStats playerStats)
    {
        Debug.Log("You drank a swig of the potion. Red!");

    }
    
}
