using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the code for the Gun Enemy, basically the same codes as the AttackFish script]
 */
public class EnemyProjectile : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Rigidbody bulletRB;


    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
        bulletRB.velocity = -transform.forward * bulletSpeed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "I-Wall")
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            KokomiController.instance.currentHealth -= Enemy.instance.enemyDamage;
        }
    }
}
