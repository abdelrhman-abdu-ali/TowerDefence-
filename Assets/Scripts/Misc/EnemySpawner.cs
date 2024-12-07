using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform targetTower;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int enemiesPerWave = 10;

    private float lastSpawnTime;
    private int currentWave = 0;
    private int enemiesSpawned = 0;

    private void Update()
    {
        if (Time.time - lastSpawnTime >= spawnInterval && enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
            enemiesSpawned++;
        }

        if (enemiesSpawned >= enemiesPerWave && FindObjectsOfType<BaseEnemy>().Length == 0)
        {
            StartNextWave();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || targetTower == null) return;

        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject spawnedEnemy = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

        BaseEnemy enemyComponent = spawnedEnemy.GetComponent<BaseEnemy>();
        if (enemyComponent != null)
        {
            enemyComponent.SetTargetTower(targetTower);
        }
    }

    private void StartNextWave()
    {
        currentWave++;
        enemiesSpawned = 0;
    }
}