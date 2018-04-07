using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat {

    public List<StatBonus> BaseAdditives { get; set; }
    public int BaseValue { get; set; }
    public string StatName { get; set; }
    public string StatDescription { get; set; }
    public int FinalValue { get; set; }

    public BaseStat(int baseVal, string statName, string statDescription)
    {
        BaseAdditives = new List<StatBonus>();
        this.BaseValue = baseVal;
        this.StatName = statName;
        this.StatDescription = statDescription;
    }

    [Newtonsoft.Json.JsonConstructor]
    public BaseStat(int baseVal, string statName)
    {
        BaseAdditives = new List<StatBonus>();
        this.BaseValue = baseVal;
        this.StatName = statName;
    }

    public void AddStatBonus(StatBonus statBonus)
    {
        BaseAdditives.Add(statBonus);
    }

    public void RemoveStatBonus(StatBonus statBonus)
    {
        BaseAdditives.Remove(BaseAdditives.Find(x => statBonus.BonusValue == x.BonusValue));
    }

    public int GetCalulatedStatValue()
    { 
        FinalValue = 0;
        BaseAdditives.ForEach(x => FinalValue += x.BonusValue);
        FinalValue += BaseValue;
        return FinalValue;
    }
}
