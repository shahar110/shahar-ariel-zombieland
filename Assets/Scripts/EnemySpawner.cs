using System.Collections;
using System.Collections.Generic;
using System.IO;
// using UnityEditorInternal;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState { COUNTING, SPAWNING, WAITING};
    public GameObject[] enemyPrefabs;
    public Transform[] spwanPoints;

    private static int totalActiveEnemies = 0;
    public float spawnDeltaTime = 5.0f;
    public float timeBetweenWaves = 5.0f;
    public int enemiesInWave = 3;
    private float waveCountdown;
    private SpawnState state = SpawnState.COUNTING;

    // Start is called before the first frame update
    void Start()
    {
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
                StartCoroutine(SpawnRound());
            }
        }
        else
        {
            Debug.Log("waveCountdown=" + waveCountdown);
            waveCountdown -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnRound()
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

    void SpawnEnemy()
    {
        GameObject newEnemy;
        Vector3 randomSpawnPoint = spwanPoints[Random.Range(0, spwanPoints.Length)].position;
        newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                                    randomSpawnPoint, Quaternion.identity, transform) as GameObject;
        Debug.Log("SpawnEnemy() totalActiveEnemies=" + totalActiveEnemies);
        totalActiveEnemies++;
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
    }

    public static void decEnemyCount()
    {
        totalActiveEnemies--;
    }
}


