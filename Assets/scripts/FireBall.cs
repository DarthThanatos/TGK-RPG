
using UnityEngine;

public class FireBall : MonoBehaviour {
    public Vector3 direction { get; set; }
    public float range = 20;
    public int damage { get; set; }

    [SerializeField] private LayerMask aggroLayerMask;
    [SerializeField] private Material transparentMaterial;

    private ParticleSystem componentParticleSystem;
    Vector3 spawnPosition;

    private int chanceToExplode = 25;

    void Start()
    {
        spawnPosition = transform.position;
        GetComponent<Rigidbody>().AddForce(direction * 50f);
        componentParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Vector3.Distance(spawnPosition, transform.position) >= range) 
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; ;
            extinguish();
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            Debug.Log("Fireball dealt " + damage + " damage");
            collision.transform.GetComponent<IEnemy>().takeDamage(damage);
        }
        extinguish();
    }


    void extinguish()
    {

        if (!componentParticleSystem.isPlaying )
        {
            if (chanceToExplode >= Random.Range(0, 100 + 1))
            {
                Debug.Log("Exploding");
                componentParticleSystem.Play();

                MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
                renderer.material = transparentMaterial;
                
                Collider[] withinAggroColliders = Physics.OverlapSphere(transform.position, 10, aggroLayerMask);
                foreach(Collider collider in withinAggroColliders)
                {
                    collider.GetComponent<IEnemy>().takeDamage(damage);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
