using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Rigidbody bulletRB;


    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
        bulletRB.velocity = transform.position * bulletSpeed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "I-Wall")
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            KokomiController.instance.currentHealth -= Enemy.instance.enemyDamage;
        }
    }
}
