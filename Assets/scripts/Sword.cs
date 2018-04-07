using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon {

    public List<BaseStat> Stats { get; set; }
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        animator.SetTrigger("Base_Attack");
    }


    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Hit : " + collider.name);
        if(collider.tag == "Enemy")
        {
            collider.GetComponent<IEnemy>().takeDamage(2);
        }
    }

}
