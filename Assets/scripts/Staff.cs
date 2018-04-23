
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Staff : MonoBehaviour, IWeapon, IProjectileWeapon {

    public Transform projectileSpawn  {  get;  set; }

    public List<BaseStat> Stats { get; set; }
    private Animator animator;
    FireBall fireBall;

    private int damageToGive;

    public int CurrentDamage { get; set; }

    void Start()
    {
        fireBall = Resources.Load<FireBall>("Weapons/Projectiles/FireBall");
        animator = GetComponent<Animator>();

    }

    public void castProjectile()
    {
        FireBall fireBallInstance = Instantiate(fireBall, projectileSpawn.position, projectileSpawn.rotation);
        fireBallInstance.damage = damageToGive;
        fireBallInstance.direction = projectileSpawn.forward;
    }

    public void PerformAttack(int damage)
    {
        damageToGive = damage;
        animator.SetTrigger("Staff_Cast");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy")
        {
            collider.GetComponent<IEnemy>().takeDamage(damageToGive);
        }
    }

    public void OnTargetInteraction(NavMeshAgent playerNavMeshAgent)
    {
        playerNavMeshAgent.stoppingDistance = fireBall.range;
        Debug.Log("Staff: stopping distance: 20");
    }
}
