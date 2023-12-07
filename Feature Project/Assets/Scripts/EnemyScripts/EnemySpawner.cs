using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the code for the two Enemy spawners on the Feature Scene]
 */
public class EnemySpawner : MonoBehaviour
{
    //General Codes for the Spammer
    public GameObject movingEnemyPrefab;
    public GameObject gunEnemyPrefab;
    public Transform movingEnemySpawn;
    
    //These Bools are meant to be an alternative for inheritance, since the Spawner objects are not prefabs.
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
