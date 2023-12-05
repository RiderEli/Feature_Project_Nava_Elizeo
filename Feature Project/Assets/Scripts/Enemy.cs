using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxEnemyHP = 10000;
    public int currentEnemyHP;
    public int enemyDamage = 500;
    public Vector3 enemyPos;
    public float enemySpeed;
    public GameObject projectilePrefab;
    private void Awake()
    {
        currentEnemyHP = maxEnemyHP;        
    }

    private void Update()
    {
        Debug.Log("Enemy HP:" + currentEnemyHP + "/" + maxEnemyHP);
        if (currentEnemyHP < 0)
        {
            currentEnemyHP = 0;
        }
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
