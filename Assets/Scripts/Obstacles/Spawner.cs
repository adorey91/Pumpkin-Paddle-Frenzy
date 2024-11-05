using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private CustomTimer obstacleSpawnTimer; // used to count how long until next spawn
    private CustomTimer recalculateTimer; // used to recalculate spawning speeds/spawn values 

    [Header("Spawning Base Values")]
    [SerializeField] private float obstacleSpawnTime = 5f;
    [Range(0f, 1f)] public float obstacleSpawnFactor = 0.075f;
    [Range(0f, 1f)] public float collectableProbability = 0.4f;
    [Range(0f, 1f)] public float kayakProbability = 0.3f;
    [SerializeField] private float calculateTime = 10f;
    private float _obstacleSpawnTime;
    private float timeAlive = 1; // spawner uses this to increase the spawntime & speed
    private float timeAliveInRun;

    private int level = 0; // keep track of times the time increased
    private float targetValue; // keep track of level float
    private int winningLevel;
    private bool finishSpawned = false;
    internal bool spawnedFirstObstacle = false;
    [SerializeField] private Slider levelProgressSlider;


    private void Start()
    {
        obstacleSpawnTimer = new CustomTimer(obstacleSpawnTime);
        recalculateTimer = new CustomTimer(calculateTime);
        winningLevel = GameManager.instance.winningLevel;

        levelProgressSlider.maxValue = winningLevel;
        levelProgressSlider.value = level;

        ResetValues();
    }

    private void Update()
    {
        if (!finishSpawned && GameManager.instance.isPlaying)
        {
            UpdateProgressSlider();
            SpawnLoop();
        }
    }

    #region ActionsEnableDisable
    private void OnEnable()
    {
        Actions.OnGameplay += ResetValues;
        Actions.OnGameOver += ResetProcessSlider;
        Actions.OnGameWin += ResetProcessSlider;
    }

    private void OnDisable()
    {
        Actions.OnGameplay -= ResetValues;
        Actions.OnGameOver -= ResetProcessSlider;
        Actions.OnGameWin -= ResetProcessSlider;

    }
    #endregion

    private void CalculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnFactor);

        Actions.SpeedChange(timeAlive);
    }

    private void UpdateProgressSlider()
    {
        timeAliveInRun += Time.deltaTime;
        // Update the progress bar based on fractional timeAlive progress rather than level increments
        if(timeAliveInRun > calculateTime)
            timeAliveInRun -= calculateTime;

        float fractionalLevelProgress = level + (timeAliveInRun / calculateTime); 
        targetValue = Mathf.Clamp(fractionalLevelProgress, 0, winningLevel); 

        // Smoothly interpolate the slider value to targetValue
        levelProgressSlider.value = Mathf.Lerp(levelProgressSlider.value, targetValue, Time.deltaTime * 5f);
    }


    #region Spawn
    // Spawn Loop
    private void SpawnLoop()
    {
        timeAlive += Time.deltaTime;

        if (recalculateTimer.UpdateTimer(Time.deltaTime))
        {
            level++;
            CalculateFactors();
            Actions.SpeedChange(timeAlive);
            recalculateTimer.StartTimer(calculateTime);
        }

        if (obstacleSpawnTimer.UpdateTimer(Time.deltaTime))
        {
            if (!spawnedFirstObstacle)
                spawnedFirstObstacle = true;

            Spawn();
            obstacleSpawnTimer.StartTimer(_obstacleSpawnTime);
        }
    }

    private void Spawn()
    {
        // Spawn the finish line if the player reaches the winning level if the game isnt endless
        if (level == winningLevel && !GameManager.instance.gameIsEndless)
        {
            Actions.OnSpawn(PoolType.FinishLine);
            finishSpawned = true;
        }
        else
            PickSpawn();
    }

    private void PickSpawn()
    {
        float randomValue = Random.Range(0f, 1f);

        // Spawn collectable
        if (randomValue < collectableProbability)
            Actions.OnSpawn(PoolType.Collectable);
        else
            SpawnObstacle();
    }

    private void SpawnObstacle()
    {
        float value = Random.Range(0f, 1f);

        if (value < kayakProbability)
            Actions.OnSpawn(PoolType.Kayak);
        else
            Actions.OnSpawn(PoolType.Obstacle);

    }
    #endregion

    #region Resets
    private void ResetValues()
    {
        spawnedFirstObstacle = false;
        level = 0;
        timeAlive = 1;
        Actions.SpeedChange(timeAlive);
        finishSpawned = false;
        _obstacleSpawnTime = obstacleSpawnTime;
        obstacleSpawnTimer.StartTimer(_obstacleSpawnTime);
        recalculateTimer.StartTimer(calculateTime);
    }

    private void ResetProcessSlider()
    {
        levelProgressSlider.value = 0;
    }
    #endregion
}