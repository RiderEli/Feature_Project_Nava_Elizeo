using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the base class for the enemies in the game]
 */
public class Enemy : MonoBehaviour
{
    //These are the Data codes for the Enemy
    public int maxEnemyHP = 10000;
    public int currentEnemyHP;
    public int enemyDamage = 500;
    public Vector3 enemyPos;
    public float enemySpeed;

    //These are the codes for a Enemy child that shoots projectiles.
    public Transform projectileSpawn;
    public GameObject projectilePrefab;
    private GameObject Player;
    
    //This is meant to limit the attack's time so that there is no infinite rapidfire.
    public float enemyATKTime;
    private float lastEnemyATK;
    
    //This rigidbody is reserved for the Moving Enemy child. 
    private Rigidbody enemyRB;

    //This is the Singleton pattern for the KokomiController and AttackFish codes.
    static public Enemy instance;
    public Text enemyHPText;
    private void Awake()
    {
        instance = this;
        currentEnemyHP = maxEnemyHP;
        Player = GameObject.FindGameObjectWithTag("Player");
        enemyRB = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        enemyHPText.text = "HP: " + currentEnemyHP.ToString() + "/" + maxEnemyHP.ToString();
        if (currentEnemyHP < 0)
        {
            Destroy(gameObject);
        }
    }
    public virtual void Update()
    {
      //This is the override script for inheritance classes.   
    }
    //Reserved for Moving Enemy child
    public void EnemyMove()
    {
        enemyPos = transform.position;

        enemyPos = Vector3.MoveTowards(this.transform.position, Player.transform.position, enemySpeed * Time.deltaTime);

        transform.position = enemyPos;
    }

    //Reserved for Gun Enemy child
    public void EnemyShoot()
    {
        if (Time.time - lastEnemyATK < enemyATKTime)
        {
            return;
        }
        lastEnemyATK = Time.time;
        Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
    }

    //Collision Stuff
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            KokomiController.instance.currentHealth -= enemyDamage;
        }

        if (collision.gameObject.tag == "Water")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "GroundUnderWater")
        {
            Destroy(gameObject);
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
