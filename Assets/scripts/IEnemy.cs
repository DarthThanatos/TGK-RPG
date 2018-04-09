using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IEnemy {
    int Experience { get; set; }
    void takeDamage(int amount);
    void performAttack();
    void Die();
}
