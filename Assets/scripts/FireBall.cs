
using UnityEngine;

public class FireBall : MonoBehaviour {
    public Vector3 direction { get; set; }
    public float range { get; set; }
    public int damage { get; set; }

    [SerializeField] private LayerMask aggroLayerMask;
    private ParticleSystem componentParticleSystem;
    Vector3 spawnPosition;

    private int chanceToExplode = 100;

    void Start()
    {
        range = 20;
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

                //MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
                //Material material = renderer.material;

                gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
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
