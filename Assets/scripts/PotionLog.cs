using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionLog : MonoBehaviour, IConsumable {
    public void consume()
    {
        Debug.Log("You drank a swig of the potion. Cool!");
    }

    public void consume(CharacterStats playerStats)
    {
        Debug.Log("You drank a swig of the potion. Red!");
    }
    
}
