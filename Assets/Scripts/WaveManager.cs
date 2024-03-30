using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public GameObject meleeEnemyPrefab;
    public GameObject rangeEnemyPrefab;
    public GameObject pistolPrefab; // Prefab for the pistol weapon
    public GameObject smgPrefab; // Prefab for the SMG weapon
    public Transform[] meleeSpawnPoints;
    public Transform[] rangeSpawnPoints;
    public Transform weaponSpawnPoint; // Spawn point for weapons
    public int baseMeleeEnemiesPerWave = 2;
    public int meleeEnemiesIncreasePerWave = 2;
    public int baseRangeEnemiesPerWave = 1;
    public int rangeEnemiesIncreasePerWave = 1;
    public float timeBetweenWaves = 3f;
    public AudioClip weaponSpawnSound;

    private int currentWave = 0;
    private List<GameObject> activeMeleeEnemies = new List<GameObject>();
    private List<GameObject> activeRangeEnemies = new List<GameObject>();
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

        // Spawn weapons at specific waves
        SpawnWeaponsForWave(currentWave);

        meleeEnemiesToSpawnForCurrentWave = baseMeleeEnemiesPerWave + (currentWave - 1) * meleeEnemiesIncreasePerWave;
        rangeEnemiesToSpawnForCurrentWave = currentWave >= 5 ? Mathf.Min(baseRangeEnemiesPerWave + (currentWave - 5) * rangeEnemiesIncreasePerWave, 4) : 0;

        StartCoroutine(SpawnEnemies());

        yield return new WaitUntil(() => activeMeleeEnemies.Count == 0 && activeRangeEnemies.Count == 0 &&
                                          meleeEnemiesToSpawnForCurrentWave == 0 && rangeEnemiesToSpawnForCurrentWave == 0);

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

    IEnumerator StartNextWave()
    {
        isStartingNextWave = true;
        Debug.Log("Starting next wave...");
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(StartWave());
        isStartingNextWave = false;
    }

    public void MeleeEnemyDefeated(GameObject enemy)
    {
        if (activeMeleeEnemies.Contains(enemy))
        {
            activeMeleeEnemies.Remove(enemy);
            Debug.Log("Melee Enemy defeated. Melee Enemies remaining: " + activeMeleeEnemies.Count);
            Debug.Log("Melee Enemies left to spawn: " + meleeEnemiesToSpawnForCurrentWave);
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

    void SpawnWeaponsForWave(int wave)
    {
        if (wave == 1)
        {
            // Spawn a pistol weapon at the weapon spawn point for wave 1
            GameObject pistol = Instantiate(pistolPrefab, weaponSpawnPoint.position, Quaternion.identity);
            // Play spawn sound
            PlaySpawnSound();
        }
        else if (wave == 6)
        {
            // Spawn an SMG weapon at the weapon spawn point for wave 6
            GameObject smg = Instantiate(smgPrefab, weaponSpawnPoint.position, Quaternion.identity);
            // Play spawn sound
            PlaySpawnSound();
        }
    }

    void PlaySpawnSound()
    {
        AudioSource.PlayClipAtPoint(weaponSpawnSound, transform.position);
    }
}
