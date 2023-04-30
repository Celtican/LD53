using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveController : MonoBehaviour
{
    [Tooltip("The list of waves that are spawned in this level.")]
    public List<Wave> waves;

    public UnityEvent onEndWave;
    
    private float timeSinceLastSpawn = 0f;

    private void Start()
    {
        onEndWave.AddListener(() => CameraController.instance.MoveHorizontally());
    }

    // Update is called once per frame
    void Update()
    {
        // If we don't have any waves left, exit this function (don't do anything).
        if (waves.Count == 0)
        {
            return;
        }
        
        // Add how much time has passed since last frame.
        timeSinceLastSpawn += Time.deltaTime;

        // Get the current wave. We'll reference this a bunch later.
        Wave currentWave = waves[0];
        // If we have enemies to spawn...
        if (currentWave.numEnemiesToSpawn > 0)
        {
            // ... and if enough time has passed that we should spawn an enemy...
            if (timeSinceLastSpawn > currentWave.timeBetweenSpawns)
            {
                // ... spawn the enemy!
                currentWave.numEnemiesToSpawn -= 1;
                Instantiate(currentWave.enemyPrefab, transform.localPosition, Quaternion.identity, transform);
                
                // And reset the time since last spawn to 0.
                timeSinceLastSpawn = 0;
            }
        }
        else if (timeSinceLastSpawn >= currentWave.timeAfterWave)
        {
            // If we don't have any enemies to spawn, and we've waited enough time, clear the current wave.
            waves.RemoveAt(0);
            
            onEndWave.Invoke();
            
            // And reset the time since last spawn.
            timeSinceLastSpawn = 0;
        }
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
