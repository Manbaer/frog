using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public GameObject meleeEnemyPrefab;
    public Transform[] spawnPoints;
    public int numberOfEnemiesPerWave = 2;
    public float timeBetweenWaves = 3f;

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        currentWave++;
        Debug.Log("Wave " + currentWave + " started!");

        // Spawn enemies for the current wave
        SpawnEnemies();

        // Wait until all enemies are defeated
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

        // Wait for some time before starting the next wave
        yield return new WaitForSeconds(timeBetweenWaves);

        Debug.Log("Wave " + currentWave + " completed!");

        // Start the next wave if there are more waves to go
        if (currentWave < 5)
            StartCoroutine(StartWave());
        else
            Debug.Log("All waves completed!");
    }

    void SpawnEnemies()
    {
        // Shuffle the spawn points to randomize enemy spawns
        ShuffleSpawnPoints();

        for (int i = 0; i < Mathf.Min(numberOfEnemiesPerWave, spawnPoints.Length); i++)
        {
            GameObject enemy = Instantiate(meleeEnemyPrefab, spawnPoints[i].position, Quaternion.identity);
        }
    }

    void ShuffleSpawnPoints()
    {
        // The one and only shuffle algorithm for shuffling spawn points
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int randomIndex = Random.Range(i, spawnPoints.Length);
            Transform temp = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = spawnPoints[i];
            spawnPoints[i] = temp;
        }
    }
}
