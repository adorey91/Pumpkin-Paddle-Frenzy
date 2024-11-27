using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    // Timers for spawning and recalculating
    private CustomTimer obstacleSpawnTimer; // used to count how long until next spawn
    private CustomTimer recalculateTimer; // used to recalculate spawning speeds/spawn values 

    [Header("Spawning Base Values")]
    [SerializeField] private float obstacleSpawnTime = 5f; // time between spawns
    [Range(0f, 1)]
    [SerializeField] private float obstacleSpawnFactor = 0.075f; // used to increase the spawn time
    [Range(0f, 1f)]
    [SerializeField] private float collectableProbability = 0.4f; // probability of spawning a collectable
    [Range(0f, 1f)]
    [SerializeField] private float kayakProbability = 0.3f; // probability of spawning a kayak
    [SerializeField] private float calculateTime = 10f; // time between recalculating the spawn values (starting value)
    private float recalculateTime; // used to store the current recalculate time
    private float _obstacleSpawnTime; // used to store the current spawn time
    private float timeAlive = 1; // spawner uses this to increase the spawntime & currentSpeed
    private bool canSpawn = true;

    // Endless Mode Stats
    private float timeAliveInRun; // used to keep track of time alive in the run - for endless mode

    [Header("Level Progress Stats")]
    [SerializeField] private Slider levelProgressSlider;
    private GameObject levelSlider;
    bool isLevelSliderActive; // used to keep track of the level slider
    private int level = 0; // keep track of times the time increased
    private float levelInFloat = 0; // for progress bar
    private bool increaseLevel; // used to keep track of when to increase the level

    private int winningLevel; // the level the player needs to reach to win the game
    private bool finishSpawned = false; // used to keep track of if the finish line was spawned
    internal bool spawnedFirstObstacle = false; // used to keep track of if the first obstacle was spawned


    private void Start()
    {
        // Set up the timers
        obstacleSpawnTimer = new CustomTimer(obstacleSpawnTime);
        recalculateTimer = new CustomTimer(calculateTime);

        // Get the winning level from the game manager
        winningLevel = GameManager.instance.winningLevel;

        // Set up the level slider
        levelSlider = levelProgressSlider.gameObject;
        UpdateMaxProgressBarValue();

        // Reset the values
        ResetValues();
    }

    private void Update()
    {
        // if the game is playing and the finish line hasnt been spawned
        if (!finishSpawned && GameManager.instance.isPlaying)
        {
            HandleSpawning();
            HandleProgressSlider();
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

    private void HandleProgressSlider()
    {
        // if game is endless turn off the level slider and dont update it
        if (GameManager.instance.gameIsEndless)
        {
            if (isLevelSliderActive)
            {
                levelSlider.SetActive(false);
                isLevelSliderActive = false;
            }
        }
        else
        {
            // if game is not endless turn on the level slider and update it
            if (!isLevelSliderActive)
            {
                levelSlider.SetActive(true);
                isLevelSliderActive = true;
            }

            UpdateProgressSlider();
        }
    }

    private void UpdateMaxProgressBarValue()
    {
        float maxValue = 0f;
        float tempTimeAlive = 1f;

        // Calculate the max value of the progress bar
        for (int i = 0; i < winningLevel; i++)
        {
            float recalculation = calculateTime / Mathf.Pow(tempTimeAlive, 0.15f);
            maxValue += recalculation;
            tempTimeAlive += recalculation;
        }
        levelProgressSlider.maxValue = maxValue;
        levelProgressSlider.value = 0;
    }

    private void CalculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnFactor);
        recalculateTime = calculateTime / Mathf.Pow(timeAlive, 0.15f);
        Actions.SpeedChange(timeAlive);
    }

    private void UpdateProgressSlider()
    {
        levelInFloat += Time.deltaTime;

        if (increaseLevel)
        {
            level++;
            increaseLevel = false;
        }

        // Smoothly interpolate the slider value to targetValue
        levelProgressSlider.value = levelInFloat;
    }


    #region Spawn
    // Spawn Loop
    private void HandleSpawning()
    {
        timeAlive += Time.deltaTime;

        // if the recalculate timer is done, calculate the new factors
        if (recalculateTimer.UpdateTimer(Time.deltaTime))
        {
            increaseLevel = true;
            CalculateFactors();
            Actions.SpeedChange(timeAlive);
            recalculateTimer.StartTimer(recalculateTime);
        }

        // if the obstacle spawn timer is done, spawn an obstacle
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

        if (value < kayakProbability & level >= 6 && canSpawn)
        {
            Actions.OnSpawn(PoolType.Kayak);
            canSpawn = false;
        }
        else
        {
            Actions.OnSpawn(PoolType.Obstacle);
            canSpawn = true;
        }

    }
    #endregion

    #region Resets
    private void ResetValues()
    {
        spawnedFirstObstacle = false;
        level = 0;
        levelInFloat = 0;
        timeAlive = 1;
        timeAliveInRun = 0;
        Actions.SpeedChange(timeAlive);
        finishSpawned = false;
        _obstacleSpawnTime = obstacleSpawnTime;
        obstacleSpawnTimer.StartTimer(_obstacleSpawnTime);
        recalculateTimer.StartTimer(calculateTime);
    }

    private void ResetProcessSlider()
    {
        UpdateMaxProgressBarValue();
    }
    #endregion
}