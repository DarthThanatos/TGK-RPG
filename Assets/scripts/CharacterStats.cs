using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public List<BaseStat> stats = new List<BaseStat>();

	// Use this for initialization
	void Start () {
        stats.Add(new BaseStat(4, "Power", "Your power lvl."));
        stats.Add(new BaseStat(2, "Speed", "Your Speed lvl."));
    }
	

    public void AddStatBonus(List<BaseStat> baseWeaponStats)
    {
        foreach (BaseStat baseWeaponStat in baseWeaponStats)
        {
            stats
                .Find(x => x.StatName == baseWeaponStat.StatName)
                .AddStatBonus(
                    new StatBonus(baseWeaponStat.BaseValue)
                );
        }
    }

    public void RemoveStatBonus(List<BaseStat> baseWeaponStats)
    {
        foreach (BaseStat baseWeaponStat in baseWeaponStats)
        {
            stats
                .Find(x => x.StatName == baseWeaponStat.StatName)
                .RemoveStatBonus(
                    new StatBonus(baseWeaponStat.BaseValue)
                );
        }
    }
}
