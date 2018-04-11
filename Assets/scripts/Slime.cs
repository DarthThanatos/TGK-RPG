
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour, IEnemy {


    public LayerMask aggroLayerMask;
    private NavMeshAgent navMeshAgent;

    public float currentHealth;
    public float maxHealth = 100;

    private CharacterStats characterStats;
    private Player player;

    public int Experience {get; set;}
    public DropTable dropTable { get; set; }

    public Spawner spawner { get; set; }

    void Start()
    {
        dropTable = new DropTable();
        dropTable.loot = new List<LootDrop>() {
            new LootDrop("Sword_01", 5),
            new LootDrop("Staff_01", 0),
            new LootDrop("Potion", 60)
        };


        Experience = 20;
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterStats = new CharacterStats(6,10,2);
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        Collider[] withinAggroColliders = Physics.OverlapSphere(transform.position, 10, aggroLayerMask);
        if(withinAggroColliders.Length > 0)
        {
            Collider collider = withinAggroColliders[0];
            ChasePlayer(collider.GetComponent<Player>());
        }
    }

    void ChasePlayer(Player player)
    {
        this.player = player;
        navMeshAgent.SetDestination(player.transform.position);
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!IsInvoking("performAttack"))
                InvokeRepeating("performAttack", .5f, 3f);
        }
        else
        {
            if (IsInvoking("performAttack"))
            {
                CancelInvoke("performAttack");
            }
        }
    }

    public void performAttack()
    {
        if(player != null)
        {
            player.takeDamage(5);
        }
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Die();
        }

    }


    public void Die()
    {
        DropLoot();
        CombatEvents.EnemyDied(this);
        if(this.spawner != null)this.spawner.Respawn();
        Destroy(gameObject);
    }

    void DropLoot()
    {
        Item item = dropTable.GetDrop();
        if(item != null)
        {
            Debug.Log("Dropping " + item.AbsoluteSlug);
            PickupItem instance = Instantiate(Resources.Load<PickupItem>(item.AbsoluteSlug), transform.position, Quaternion.identity);
            instance.ItemDrop = item;
        }
    }
}
