
using System.Collections.Generic;
using UnityEngine;

public class DropTable {
    public List<LootDrop> loot;

    public Item GetDrop()
    {
        int roll = Random.Range(0, 100 + 1);
        int weightSum = 0;
        foreach(LootDrop drop in loot)
        {
            weightSum += drop.Weight;
            if (roll < weightSum)
            {
                return ItemDatabase.instance.GetNewInstanceOfItemWithSlug(drop.ObjectSlug);
            }
        }
        return null;
    }

}

public class LootDrop
{
    public string ObjectSlug { get; set; }
    public int Weight { get; set; }

    public LootDrop(string ObjectSlug, int Weight)
    {
        this.ObjectSlug = ObjectSlug;
        this.Weight = Weight;
    }
}