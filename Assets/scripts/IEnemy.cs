using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IEnemy {
    int ID { get; set; }
    Spawner spawner { get; set; }
    int Experience { get; set; }
    void takeDamage(int amount);
    void performAttack();
    void Die();
}
