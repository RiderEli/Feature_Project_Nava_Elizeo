using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public int maxEnemyHP = 10000;
    public int currentEnemyHP;
    public int enemyDamage = 500;
    public Vector3 enemyPos;
    public float enemySpeed;
    public Transform projectileSpawn;
    public GameObject projectilePrefab;
    private GameObject Player;
    public float enemyATKTime;
    private float lastEnemyATK;
    private Rigidbody enemyRB;
    static public Enemy instance;
    private void Awake()
    {
        instance = this;
        currentEnemyHP = maxEnemyHP;
        Player = GameObject.FindGameObjectWithTag("Player");
        enemyRB = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        if (currentEnemyHP < 0)
        {
            Destroy(gameObject);
        }
    }
    public virtual void Update()
    {
      //This is the override script for inheritance classes.   
    }
    public void EnemyMove()
    {
        enemyPos = transform.position;

        enemyPos = Vector3.MoveTowards(this.transform.position, Player.transform.position, enemySpeed * Time.deltaTime);

        transform.position = enemyPos;
    }


    public void EnemyShoot()
    {
        if (Time.time - lastEnemyATK < enemyATKTime)
        {
            return;
        }
        lastEnemyATK = Time.time;
        Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            KokomiController.instance.currentHealth -= enemyDamage;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            currentEnemyHP -= KokomiController.instance.damage;
            if (KokomiController.instance.ultimateActive)
            {
                currentEnemyHP -= KokomiController.instance.buffedDamage;
            }
        }
        if (other.gameObject.tag == "Skill")
        {
            currentEnemyHP -= KokomiController.instance.skillDamage;
            if (KokomiController.instance.ultimateActive)
            {
                currentEnemyHP -= KokomiController.instance.buffedSkillDamage;
            }
        }
    }
}
