
using System.Collections.Generic;
using UnityEngine.AI;

public interface IWeapon {
    List<BaseStat> Stats { get; set; }
    int CurrentDamage { get; set; }
    void PerformAttack(int damage);
    void OnTargetInteraction(NavMeshAgent playerNavMeshAgent);
}
