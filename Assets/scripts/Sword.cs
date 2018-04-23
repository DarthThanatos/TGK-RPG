
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sword : MonoBehaviour, IWeapon {

    public List<BaseStat> Stats { get; set; }
    private Animator animator;
    private int damageToGive;

    public int CurrentDamage{ get; set; }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PerformAttack(int damage)
    {
        damageToGive = damage;
        animator.SetTrigger("Base_Attack");
    }


    void OnTriggerEnter(Collider collider)
    {

        if(collider.tag == "Enemy")
        {
            collider.GetComponent<IEnemy>().takeDamage(damageToGive);
        }
    }

    public void OnTargetInteraction(NavMeshAgent playerNavMeshAgent)
    {
        playerNavMeshAgent.stoppingDistance = 3;
        Debug.Log("Sword: stopping distance: 3");
    }
}
