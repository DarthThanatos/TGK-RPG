
using UnityEngine;

public class ConsumableController : MonoBehaviour {

    CharacterStats stats;

	// Use this for initialization
	void Start () {
        stats = GetComponent<Player>().characterStats;

	}

    public void consumeItem(Item item)
    {
        GameObject itemToSpawn = Instantiate(Resources.Load<GameObject>("Consumables/" + item.ObjectSlug));
        if (item.ItemModifier)
        {
            itemToSpawn.GetComponent<IConsumable>().consume(stats);
        }
        else
        {
            itemToSpawn.GetComponent<IConsumable>().consume();
        }


    }
	
}
