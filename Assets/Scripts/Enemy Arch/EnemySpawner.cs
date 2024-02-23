using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner Variables")]
    [SerializeField] private int delay = 3;
    [SerializeField] private int counter = 0;
    [SerializeField] private int maxEnemies = 4;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Transform[] homePoints;
    public Transform[] points;
    public UpdateMarkedPoints updateMarkedPoints;
    private bool spawned = true;

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        counter = enemies.Length;
        if (counter < maxEnemies && spawned)
        {
            StartCoroutine(SpawnEnemies());
            spawned = false;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(delay);
        GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        enemy.GetComponent<EnemyBehaviour>().points = points;
        enemy.GetComponent<EnemyBehaviour>().updateMarkedPoints = updateMarkedPoints;
        enemy.GetComponent<EnemyBehaviour>().home = homePoints[Random.Range(0,2)];
        counter++;
        spawned = true;
    }
}