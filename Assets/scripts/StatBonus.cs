﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBonus {


    public int BonusValue { get; set; }

    public StatBonus(int bonusVal)
    {
        this.BonusValue = bonusVal;
        Debug.Log("Bonus initiated");
    }
}
