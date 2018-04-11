﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }
    public Quest Quest { get; set; }

    public virtual void Init()
    {

    }

    public virtual void Finish()
    {

    }

    public void Evaluate()
    {
        Debug.Log("Current: " + CurrentAmount + " required: " + RequiredAmount);
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }

    public void Complete()
    {
        Debug.Log("Completed");
        Completed = true;
    }
}