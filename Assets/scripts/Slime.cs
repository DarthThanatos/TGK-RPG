
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour, IEnemy {


    public LayerMask aggroLayerMask;
    private NavMeshAgent navMeshAgent;

    public float currentHealth;
    public float maxHealth = 100;

    private CharacterStats characterStats;
    private Player player;

    void Start()
    {
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
            die();
        }

    }

    void die()
    {
        Destroy(gameObject);
    }

}
