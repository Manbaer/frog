using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public GameObject meleeEnemyPrefab;
    public GameObject rangeEnemyPrefab;
    public GameObject bigMeleeEnemyPrefab;
    public GameObject pistolPrefab; // Prefab for the pistol weapon
    public GameObject smgPrefab; // Prefab for the SMG weapon
    public GameObject sniperPrefab; // Prefab for the sniper weapon
    public Transform[] meleeSpawnPoints;
    public Transform[] rangeSpawnPoints;
    public Transform weaponSpawnPoint;
    public int baseMeleeEnemiesPerWave = 2;
    public int meleeEnemiesIncreasePerWave = 2;
    public int baseRangeEnemiesPerWave = 1;
    public int rangeEnemiesIncreasePerWave = 1;
    public float timeBetweenWaves = 3f;
    public AudioClip weaponSpawnSound;

    private int currentWave = 0;
    private List<GameObject> activeMeleeEnemies = new List<GameObject>();
    private List<GameObject> activeRangeEnemies = new List<GameObject>();
    private List<GameObject> activeBigMeleeEnemies = new List<GameObject>(); // List to track active big melee enemies
    private int meleeEnemiesToSpawnForCurrentWave = 0;
    private int rangeEnemiesToSpawnForCurrentWave = 0;
    private bool isStartingNextWave = false;

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        currentWave++;
        Debug.Log("Wave " + currentWave + " started!");

        SpawnWeaponsForWave(currentWave);

        meleeEnemiesToSpawnForCurrentWave = baseMeleeEnemiesPerWave + (currentWave - 1) * meleeEnemiesIncreasePerWave;
        rangeEnemiesToSpawnForCurrentWave = currentWave >= 5 ? Mathf.Min(baseRangeEnemiesPerWave + (currentWave - 5) * rangeEnemiesIncreasePerWave, 4) : 0;

        if (currentWave >= 10)
        {
            int bigMeleeEnemyCount = Mathf.Min((currentWave - 10) / 2 + 1, 4); // Calculate big melee enemy count based on wave number
            for (int i = 0; i < bigMeleeEnemyCount; i++)
            {
                SpawnBigMeleeEnemy();
            }
        }

        StartCoroutine(SpawnEnemies());

        // Wait until all enemies are defeated before progressing to the next wave
        yield return new WaitUntil(() => activeMeleeEnemies.Count == 0 && activeRangeEnemies.Count == 0 &&
                                          meleeEnemiesToSpawnForCurrentWave == 0 && rangeEnemiesToSpawnForCurrentWave == 0 &&
                                          activeBigMeleeEnemies.Count == 0);

        Debug.Log("Wave " + currentWave + " completed!");

        if (!isStartingNextWave)
        {
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (meleeEnemiesToSpawnForCurrentWave > 0 || rangeEnemiesToSpawnForCurrentWave > 0)
        {
            if (meleeEnemiesToSpawnForCurrentWave > 0 && activeMeleeEnemies.Count < 8)
            {
                Transform meleeSpawnPoint = meleeSpawnPoints[UnityEngine.Random.Range(0, meleeSpawnPoints.Length)];
                GameObject meleeEnemy = Instantiate(meleeEnemyPrefab, meleeSpawnPoint.position, Quaternion.identity);
                activeMeleeEnemies.Add(meleeEnemy);
                meleeEnemiesToSpawnForCurrentWave--;
            }

            if (rangeEnemiesToSpawnForCurrentWave > 0 && activeRangeEnemies.Count < 4)
            {
                Transform rangeSpawnPoint = GetAvailableRangeSpawnPoint();
                GameObject rangeEnemy = Instantiate(rangeEnemyPrefab, rangeSpawnPoint.position, Quaternion.identity);
                if (Array.IndexOf(rangeSpawnPoints, rangeSpawnPoint) == 1)
                {
                    rangeEnemy.transform.localScale = new Vector3(1f, -1f, 1f);
                }
                activeRangeEnemies.Add(rangeEnemy);
                rangeEnemiesToSpawnForCurrentWave--;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnBigMeleeEnemy()
    {
        Transform meleeSpawnPoint = meleeSpawnPoints[UnityEngine.Random.Range(0, meleeSpawnPoints.Length)];
        GameObject bigMeleeEnemy = Instantiate(bigMeleeEnemyPrefab, meleeSpawnPoint.position, Quaternion.identity);
        activeBigMeleeEnemies.Add(bigMeleeEnemy);
    }

    IEnumerator StartNextWave()
    {
        isStartingNextWave = true;
        Debug.Log("Starting next wave...");
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(StartWave());
        isStartingNextWave = false;
    }

    void SpawnWeaponsForWave(int wave)
    {
        if (wave == 1)
        {
            GameObject pistol = Instantiate(pistolPrefab, weaponSpawnPoint.position, Quaternion.identity);
            PlaySpawnSound();
        }
        else if (wave == 6)
        {
            GameObject smg = Instantiate(smgPrefab, weaponSpawnPoint.position, Quaternion.identity);
            PlaySpawnSound();
        }
        else if (wave == 11)
        {
            GameObject sniper = Instantiate(sniperPrefab, weaponSpawnPoint.position, Quaternion.identity);
            PlaySpawnSound();
        }
    }

    void PlaySpawnSound()
    {
        AudioSource.PlayClipAtPoint(weaponSpawnSound, transform.position);
    }

    Transform GetAvailableRangeSpawnPoint()
    {
        List<Transform> shuffledSpawnPoints = new List<Transform>(rangeSpawnPoints);
        shuffledSpawnPoints.Shuffle();
        foreach (Transform spawnPoint in shuffledSpawnPoints)
        {
            if (!IsRangeSpawnPointOccupied(spawnPoint))
            {
                return spawnPoint;
            }
        }
        return null;
    }

    bool IsRangeSpawnPointOccupied(Transform spawnPoint)
    {
        foreach (GameObject enemy in activeRangeEnemies)
        {
            if (enemy.transform.position == spawnPoint.position)
            {
                return true;
            }
        }
        return false;
    }

    public void MeleeEnemyDefeated(GameObject enemy)
    {
        if (activeMeleeEnemies.Contains(enemy))
        {
            activeMeleeEnemies.Remove(enemy);
            Debug.Log("Melee Enemy defeated. Melee Enemies remaining: " + activeMeleeEnemies.Count);
            Debug.Log("Melee Enemies left to spawn: " + meleeEnemiesToSpawnForCurrentWave);
        }

        // Check if the defeated enemy is a big melee enemy
        if (activeBigMeleeEnemies.Contains(enemy))
        {
            activeBigMeleeEnemies.Remove(enemy);
            Debug.Log("Big Melee Enemy defeated. Big Melee Enemies remaining: " + activeBigMeleeEnemies.Count);
        }
    }

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
