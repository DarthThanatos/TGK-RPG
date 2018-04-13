﻿
using System.Collections.Generic;
using Newtonsoft.Json;

public class Item  {

    public enum itemTypes {  Weapon, Consumable, Quest }

    public List<BaseStat> Stats { get; set; }
    public string ObjectSlug { get; set; }
    public string Description { get; set; }
    public string ActionName { get; set; }
    public string ItemName { get; set; }
    public bool ItemModifier { get; set; }
    public string AbsoluteSlug { get; set; }
    public System.Guid Uuid { get; set; }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public itemTypes ItemType { get; set; }

    public Item(List<BaseStat> Stats, string ObjectSlug)
    {
        this.Stats = Stats;
        this.ObjectSlug = ObjectSlug;
        Uuid = System.Guid.NewGuid();
    }

    [JsonConstructor]
    public Item(List<BaseStat> Stats, string ObjectSlug, string Description, string ActionName, string ItemName, bool ItemModifier, itemTypes ItemType, string AbsoluteSlug)
    {
        this.Stats = Stats;
        this.ObjectSlug = ObjectSlug;
        this.Description = Description;
        this.ActionName = ActionName;
        this.ItemName = ItemName;
        this.ItemModifier = ItemModifier;
        this.ItemType = ItemType;
        this.AbsoluteSlug = AbsoluteSlug;
        Uuid = System.Guid.NewGuid();
    }

    public Item(Item item)
    {
        Stats = item.Stats;
        ObjectSlug = item.ObjectSlug;
        Description = item.Description;
        ActionName = item.ActionName;
        ItemName = item.ItemName;
        ItemModifier = item.ItemModifier;
        ItemType = item.ItemType;
        AbsoluteSlug = item.AbsoluteSlug;
        Uuid = System.Guid.NewGuid();
    }
}
