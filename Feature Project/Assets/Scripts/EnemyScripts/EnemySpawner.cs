using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject movingEnemyPrefab;
    public GameObject gunEnemyPrefab;
    public Transform movingEnemySpawn;
    public bool Spawner1;
    public bool Spawner2;

    public void SpawnMoving()
    {
        Instantiate(movingEnemyPrefab, movingEnemySpawn.position, Quaternion.identity);
    }

    public void SpawnGun()
    {
        Vector3 randomSpawnPos = new Vector3(Random.Range(-5, 7), 0, 4);
        Instantiate(gunEnemyPrefab, randomSpawnPos, Quaternion.identity);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Spawner1)
            {
                SpawnMoving();
            }
            if (Spawner2)
            {
                SpawnGun();
            }
        }
    }
}
