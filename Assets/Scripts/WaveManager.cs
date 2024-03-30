using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WaveManager : MonoBehaviour
{
    public GameObject meleeEnemyPrefab;
    public GameObject rangeEnemyPrefab;
    public Transform[] meleeSpawnPoints;
    public Transform[] rangeSpawnPoints;
    public int baseMeleeEnemiesPerWave = 2; // Number of melee enemies for the first wave
    public int meleeEnemiesIncreasePerWave = 2; // Number of melee enemies added per wave
    public int baseRangeEnemiesPerWave = 1; // Number of range enemies for the first wave
    public int rangeEnemiesIncreasePerWave = 1; // Number of range enemies added per wave
    public float timeBetweenWaves = 3f;

    private int currentWave = 0;
    private List<GameObject> activeMeleeEnemies = new List<GameObject>();
    private List<GameObject> activeRangeEnemies = new List<GameObject>();
    private int meleeEnemiesToSpawnForCurrentWave = 0;
    private int rangeEnemiesToSpawnForCurrentWave = 0;
    private bool isStartingNextWave = false; // Flag to track whether the next wave is being started

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        currentWave++;
        Debug.Log("Wave " + currentWave + " started!");

        // Calculate the number of melee enemies to spawn for the current wave
        meleeEnemiesToSpawnForCurrentWave = baseMeleeEnemiesPerWave + (currentWave - 1) * meleeEnemiesIncreasePerWave;
        Debug.Log("Melee Enemies to spawn for Wave " + currentWave + ": " + meleeEnemiesToSpawnForCurrentWave);

        // Calculate the number of range enemies to spawn for the current wave (start spawning from wave 5)
        if (currentWave >= 5)
        {
            // Increase the number of range enemies gradually until it reaches 4
            rangeEnemiesToSpawnForCurrentWave = Mathf.Min(baseRangeEnemiesPerWave + (currentWave - 5) * rangeEnemiesIncreasePerWave, 4);
        }
        else
        {
            // Range enemies do not spawn before round 5
            rangeEnemiesToSpawnForCurrentWave = 0;
        }
        Debug.Log("Range Enemies to spawn for Wave " + currentWave + ": " + rangeEnemiesToSpawnForCurrentWave);

        // Start spawning enemies for the current wave
        StartCoroutine(SpawnEnemies());

        // Wait until all enemies for the current wave are defeated
        yield return new WaitUntil(() => activeMeleeEnemies.Count == 0 && activeRangeEnemies.Count == 0 &&
                                          meleeEnemiesToSpawnForCurrentWave == 0 && rangeEnemiesToSpawnForCurrentWave == 0);

        Debug.Log("Wave " + currentWave + " completed!");

        // Start the next wave
        if (!isStartingNextWave) // Check if the next wave is not already being started
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator SpawnEnemies()
    {
        // Keep spawning enemies until all enemies for the current wave are spawned
        while (meleeEnemiesToSpawnForCurrentWave > 0 || rangeEnemiesToSpawnForCurrentWave > 0)
        {
            // Check if there's space to spawn more melee enemies
            if (meleeEnemiesToSpawnForCurrentWave > 0 && activeMeleeEnemies.Count < 8)
            {
                // Randomly select a melee spawn point
                Transform meleeSpawnPoint = meleeSpawnPoints[UnityEngine.Random.Range(0, meleeSpawnPoints.Length)];

                // Instantiate the melee enemy at the spawn point
                GameObject meleeEnemy = Instantiate(meleeEnemyPrefab, meleeSpawnPoint.position, Quaternion.identity);
                activeMeleeEnemies.Add(meleeEnemy);

                // Decrement the number of melee enemies left to spawn for the current wave
                meleeEnemiesToSpawnForCurrentWave--;
            }

            // Check if there's space to spawn more range enemies
            if (rangeEnemiesToSpawnForCurrentWave > 0 && activeRangeEnemies.Count < 4)
            {
                // Randomly select a range spawn point
                Transform rangeSpawnPoint = GetAvailableRangeSpawnPoint();

                // Instantiate the range enemy at the spawn point
                GameObject rangeEnemy = Instantiate(rangeEnemyPrefab, rangeSpawnPoint.position, Quaternion.identity);

                // Check if the range enemy should be flipped vertically
                if (Array.IndexOf(rangeSpawnPoints, rangeSpawnPoint) == 1) 
                {
                    // Flip the range enemy sprite horizontally
                    rangeEnemy.transform.localScale = new Vector3 (1f, -1f, 1f);
                }

                activeRangeEnemies.Add(rangeEnemy);

                // Decrement the number of range enemies left to spawn for the current wave
                rangeEnemiesToSpawnForCurrentWave--;
            }

            // Wait for a short delay before spawning the next enemy
            yield return new WaitForSeconds(0.5f);
        }
    }


    Transform GetAvailableRangeSpawnPoint()
    {
        // Shuffle the range spawn points to randomize the selection
        List<Transform> shuffledSpawnPoints = new List<Transform>(rangeSpawnPoints);
        shuffledSpawnPoints.Shuffle();

        // Iterate through the shuffled spawn points and return the first available one
        foreach (Transform spawnPoint in shuffledSpawnPoints)
        {
            if (!IsRangeSpawnPointOccupied(spawnPoint))
            {
                return spawnPoint;
            }
        }

        // If no available spawn point is found, return null
        return null;
    }

    bool IsRangeSpawnPointOccupied(Transform spawnPoint)
    {
        // Check if the given range spawn point is occupied by an active range enemy
        foreach (GameObject enemy in activeRangeEnemies)
        {
            if (enemy.transform.position == spawnPoint.position)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator StartNextWave()
    {
        isStartingNextWave = true; // Set the flag to indicate that the next wave is being started
        Debug.Log("Starting next wave...");

        // Wait for some time before starting the next wave
        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(StartWave());

        isStartingNextWave = false; // Reset the flag after starting the next wave
    }

    // Method to remove defeated melee enemies from the activeMeleeEnemies list
    public void MeleeEnemyDefeated(GameObject enemy)
    {
        if (activeMeleeEnemies.Contains(enemy))
        {
            activeMeleeEnemies.Remove(enemy);
            Debug.Log("Melee Enemy defeated. Melee Enemies remaining: " + activeMeleeEnemies.Count);
            Debug.Log("Melee Enemies left to spawn: " + meleeEnemiesToSpawnForCurrentWave);
        }
    }

    // Method to remove defeated range enemies from the activeRangeEnemies list
    public void RangeEnemyDefeated(GameObject enemy)
    {
        if (activeRangeEnemies.Contains(enemy))
        {
            activeRangeEnemies.Remove(enemy);
            Debug.Log("Range Enemy defeated. Range Enemies remaining: " + activeRangeEnemies.Count);
            Debug.Log("Range Enemies left to spawn: " + rangeEnemiesToSpawnForCurrentWave);
        }
    }
}
