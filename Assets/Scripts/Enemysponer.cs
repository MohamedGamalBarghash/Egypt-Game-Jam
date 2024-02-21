using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Changed type to GameObject[]
    public Transform[] spawnPoints;
    private bool totalEnemy = true;
    private int delay = 1;
    public int counter = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true && totalEnemy)
        {
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            counter++;
            yield return new WaitForSeconds(delay);
            if (counter == 4)
            {
                totalEnemy = false;
            }
            else
            {
                totalEnemy = true;
            }
        }
    }
}