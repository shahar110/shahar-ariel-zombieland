using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState { COUNTING, SPAWNING, WAITING};
    public GameObject[] enemyPrefabs;
    public Transform[] spwanPoints;

    private static int totalActiveEnemies = 0;
    // Spawn
    public float spawnDeltaTime = 5.0f;
    public int minTimeBetweenWaves = 4;
    public int maxTimeBetweenWaves = 9;
    private static int timeBetweenWaves;
    public static int enemiesInWave = 7;
    private float waveCountdown;
    private SpawnState state = SpawnState.COUNTING;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenWaves = getWaveTime();
        waveCountdown = timeBetweenWaves;
        Debug.Log("Start() state=" + state);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (totalActiveEnemies == 0)
            {
                WaveCompleted();
                return;
            }
            else
                return;
        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                // Start spwaning wave
                StartCoroutine(SpawnWave());
            }
        }
        else
        {
            Debug.Log("waveCountdown=" + waveCountdown);
            waveCountdown -= Time.deltaTime;
        }
    }

    // Spawn wave, wait between each signle spawn 
    private IEnumerator SpawnWave()
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < enemiesInWave; i++)
        {
            Debug.Log("SpawnRound() i=" + i);
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDeltaTime);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    // Spawn new enemy in random spwan point prefab
    void SpawnEnemy()
    {
        GameObject newEnemy;
        Vector3 randomSpawnPoint = spwanPoints[Random.Range(0, spwanPoints.Length)].position;
        newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                                    randomSpawnPoint, Quaternion.identity, transform) as GameObject;
        Debug.Log("SpawnEnemy() totalActiveEnemies=" + totalActiveEnemies);
        totalActiveEnemies++;
    }

    // All enemies in wave are dead
    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = getWaveTime();
    }

    // decrease enemy count = enemy dead
    public static void decEnemyCount()
    {
        totalActiveEnemies--;
    }

    // Level up - increase num of enemies in each wave
    public static void levelUp()
    {
        enemiesInWave++;
        Debug.Log("Enemy Spawner levelup, enemiesInWave=");
    }

    // Generate random timer for "before starting spwning new wave"
    private int getWaveTime()
    {
        return Random.Range(minTimeBetweenWaves, maxTimeBetweenWaves);
    }
}