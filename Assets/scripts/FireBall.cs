using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    public Vector3 direction { get; set; }
    public float range { get; set; }
    public int damage { get; set; }

    Vector3 spawnPosition;

    void Start()
    {
        range = 20;
        damage = 1;
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
            collision.transform.GetComponent<IEnemy>().takeDamage(damage);
        }
        extinguish();
    }


    void extinguish()
    {
        Destroy(gameObject);
    }
}
