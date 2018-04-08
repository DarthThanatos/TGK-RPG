using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon, IProjectileWeapon {

    public Transform projectileSpawn  {  get;  set; }

    public List<BaseStat> Stats { get; set; }
    private Animator animator;
    FireBall fireBall;

    public CharacterStats characterStats { get; set; }

    void Start()
    {
        fireBall = Resources.Load<FireBall>("Weapons/Projectiles/FireBall");
        animator = GetComponent<Animator>();

    }

    public void castProjectile()
    {
        FireBall fireBallInstance = Instantiate(fireBall, projectileSpawn.position, projectileSpawn.rotation);
        fireBallInstance.direction = projectileSpawn.forward;
    }

    public void PerformAttack(CharacterStats characterStats)
    {
        this.characterStats = characterStats;
        animator.SetTrigger("Staff_Cast");
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Hit : " + collider.name);
        if (collider.tag == "Enemy")
        {
            collider.GetComponent<IEnemy>().takeDamage(characterStats.GetStat(BaseStat.BaseStatType.Power).GetCalulatedStatValue());
        }
    }
}
