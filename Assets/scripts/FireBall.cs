
using UnityEngine;

public class FireBall : MonoBehaviour {
    public Vector3 direction { get; set; }
    public float range { get; set; }
    public int damage { get; set; }

    Vector3 spawnPosition;

    void Start()
    {
        range = 20;
        spawnPosition = transform.position;
        GetComponent<Rigidbody>().AddForce(direction * 50f);
    }

    void Update()
    {
        if (Vector3.Distance(spawnPosition, transform.position) >= range) 
        {
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
        Destroy(gameObject);
    }
}
