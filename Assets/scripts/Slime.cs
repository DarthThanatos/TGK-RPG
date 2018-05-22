
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour, IEnemy {


    public LayerMask aggroLayerMask;
    private NavMeshAgent navMeshAgent;
    public int ID { get; set; }
    public int currentHealth;
    public int maxHealth = 100;
    public int power = 5;
    public float attackSpeed = 3;

    private CharacterStats characterStats;
    private Player player;

    public int ExpModifier = 1;

    public int Experience { get; set; }
    public DropTable dropTable { get; set; }

    public Spawner spawner { get; set; }
    private HealthbarUI healthbarUI;

    private AudioSource woundedSound;
    private ParticleSystem woundSpotSystem;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        dropTable = new DropTable();
        dropTable.loot = new List<LootDrop>() {
            new LootDrop("Sword_01", 45),
            new LootDrop("Staff_01", 30),
            new LootDrop("Potion", 20)
        };


        Experience = 20 * ExpModifier;
        ID = 0;

        navMeshAgent = GetComponent<NavMeshAgent>();
        characterStats = new CharacterStats(5, 10, 2);

        healthbarUI = GetComponent<HealthbarUI>();
        currentHealth = maxHealth;
        healthbarUI.UpdateHealthBar(gameObject, currentHealth, maxHealth);

        woundedSound = GetComponent<AudioSource>();
        woundSpotSystem = gameObject.transform.Find("WoundSpot").GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        Collider[] withinAggroColliders = Physics.OverlapSphere(transform.position, 10, aggroLayerMask);
        if(withinAggroColliders.Length > 0)
        {
            CancelInvoke("BackToInitialPosition");
            CancelInvoke("Heal");
            Collider collider = withinAggroColliders[0];
            MusicHandler.PlayWarMusic();

            ChasePlayer(collider.GetComponent<Player>());
        } else if(navMeshAgent.velocity.Equals(Vector3.zero) && Vector3.Distance(transform.position, initialPosition) > navMeshAgent.stoppingDistance)
        {
            if(!IsInvoking("BackToInitialPosition"))
                Invoke("BackToInitialPosition", 5.0f);
        } else if(Vector3.Distance(transform.position, initialPosition) <= navMeshAgent.stoppingDistance)
        {
            if(IsInvoking("BackToInitialPosition"))
                CancelInvoke("BackToInitialPosition");
        }
    }

    void ChasePlayer(Player player)
    {
        this.player = player;
        navMeshAgent.SetDestination(player.transform.position);
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!IsInvoking("performAttack"))
                InvokeRepeating("performAttack", .5f, attackSpeed);
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
            player.takeDamage(power);
        }
    }

    public void takeDamage(int amount)
    {
        woundSpotSystem.Play();
        woundedSound.Play();
        currentHealth -= amount;
        healthbarUI.UpdateHealthBar(gameObject, currentHealth, maxHealth);
        if(currentHealth <= 0)
        {
            Invoke("Die", .5f);
        }

        Debug.Log("Taken damage");
        Debug.Log("IsInvoking(ChasePlayer)?: " + !IsInvoking("ChasePlayer"));

        if (!IsInvoking("ChasePlayer"))
        {
            Debug.Log("Going to player pos");
            navMeshAgent.SetDestination(GameObject.Find("Player").transform.position);
        }
    }


    private void BackToInitialPosition()
    {
        Debug.Log("Going back to initial pos");
        navMeshAgent.SetDestination(initialPosition);
        InvokeRepeating("Heal", .5f, .5f);
    }

    private void Heal()
    {
        if (this.currentHealth < this.maxHealth)
        {
            this.currentHealth = Math.Min(this.currentHealth + 10, this.maxHealth);
            healthbarUI.UpdateHealthBar(gameObject, currentHealth, maxHealth);
        }
        else
            CancelInvoke("Heal");
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


        int chanceToDropGold = 100;
        if (UnityEngine.Random.Range(0, 101) <= chanceToDropGold)
        {
            Vector3 goldPos = new Vector3(transform.position.x-1, 2f, transform.position.z-1);
            Quaternion quaternion = Quaternion.AngleAxis(90, new Vector3(1,0,0));
            Gold gold = Instantiate(Resources.Load<Gold>("GoldCoins/goldCoins_pref"), goldPos, quaternion);
            gold.Amount = 100 + UnityEngine.Random.Range(-15, 15); ;
        }
    }
}
