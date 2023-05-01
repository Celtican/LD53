using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WaveController : MonoBehaviour
{
    public static WaveController instance; 

    public List<Wave> waves;
    public UnityEvent onEndWave;
    public StatusDisplay waveStatus;

    public string sceneOnWin;

    private Wave currentWave = null;
    
    private float timeSinceLastSpawn = 0f;
    private int numSpawnedEnemiesThisWave;
    private int numEnemiesAlive;
    private int numMaxWaves;
    private int numCurrentWave;

    private bool gameOver;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        onEndWave.AddListener(() => CameraController.instance.MoveHorizontally());
        numMaxWaves = waves.Count;
        StartWave(true);
    }

    private void Update()
    {
        waveStatus.SetStatus($"Wave {numCurrentWave}/{numMaxWaves}",
            -((currentWave == null ? 1 : (float)numEnemiesAlive / currentWave.numEnemiesToSpawn)-1));
        
        if (currentWave == null) return;

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= currentWave.timeBetweenSpawns) SpawnEnemy();

        if (numEnemiesAlive == 0) StartWave(false);
    }

    private void SpawnEnemy()
    {
        if (numSpawnedEnemiesThisWave >= currentWave.numEnemiesToSpawn) return;
        
        EnemyController enemy = Instantiate(currentWave.enemyPrefab, transform.localPosition, Quaternion.identity, transform).GetComponent<EnemyController>();
        enemy.onDie.AddListener(() => numEnemiesAlive--);
        
        timeSinceLastSpawn -= currentWave.timeBetweenSpawns;
        numSpawnedEnemiesThisWave++;
    }

    private void StartWave(bool firstWave)
    {
        if (!firstWave)
        {
            if (waves.Count == 0)
            {
                if (!gameOver)
                {
                    gameOver = true;
                    Timer.Register(5, () => SceneManager.LoadScene(sceneOnWin));
                }
                return;
            }
            waves.RemoveAt(0);
            onEndWave.Invoke();
        }

        if (waves.Count == 0) return;
        
        numCurrentWave++;
        RestartWave(firstWave);
    }

    public void RestartWave(bool firstWave)
    {
        float time = firstWave
            ? CameraController.instance.timeForInitialMove
            : CameraController.instance.timeBetweenMoves + (currentWave == null ? 0 : currentWave.timeAfterWave);

        currentWave = null;

        Timer.Register(time, () =>
        {
            currentWave = waves[0];
            timeSinceLastSpawn = 0;
            numSpawnedEnemiesThisWave = 0;
            numEnemiesAlive = currentWave.numEnemiesToSpawn;
        });
    }

    [Serializable]
    public class Wave
    {
        [Tooltip("The enemy prefab that will spawn during this wave.")]
        public GameObject enemyPrefab;
        [Tooltip("How many enemies will spawn during this wave.")]
        public int numEnemiesToSpawn = 5;
        [Tooltip("How much time (in seconds) between individual enemy spawns.")]
        public float timeBetweenSpawns = 1f;
        [Tooltip("How much time (in seconds) until the next wave.")]
        public float timeAfterWave = 1f;
    }
}
