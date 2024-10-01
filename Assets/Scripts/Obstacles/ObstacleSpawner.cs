using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    GameManager gm;

    // obstacle spawning objects
    [Header("Obstacle Spawning")]
    [SerializeField] private Transform obstacleParent;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private GameObject[] obstaclePrefabs;
    private GameObject obstacleToSpawn;

    // base values
    [Header("Obstacle Spawning Values")]
    public float obstacleSpawnTime = 2f;
    [Range(0, 1)] public float obstacleSpawnTimeFactor = 0.1f;
    public float obstacleSpeed = 1f;
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f;

    // runtime values, these will change.
    private float _obstacleSpawnTime;
    private float _obstacleSpeed;

    private float timeUntilObstacleSpawn;
    private float timeAlive;
    private float recalculateTime;
    private float calculateTime = 8f;

    [Header("Tracker of Spawn Level Progress")]
    [SerializeField] private TextMeshProUGUI levelProgressText;
    private int level = 0;
    private bool finishSpawned = false;

    private void Start()
    {
        level = 0;
        gm = GameManager.instance;

        gm.onGameOver.AddListener(ClearObstacles);
        gm.onPlay.AddListener(ResetFactors);
    }

    private void Update()
    {
        if (GameManager.instance.isPlaying)
        {
            recalculateTime += Time.deltaTime;
            timeAlive += Time.deltaTime;

            if ((recalculateTime >= calculateTime) && level < gm.winningLevel)
            {
                CalculateFactors();
                recalculateTime = 0;
                level++;
                UpdateLevelText();
            }
            if (!finishSpawned)
                    SpawnLoop();
        }

    }

    /// <summary>
    /// Spawn look that spawns obstacle at a certain time
    /// </summary>
    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= _obstacleSpawnTime)
        {
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    /// <summary>
    /// Spawns obstacles depending on what level you're on. 
    /// </summary>
    private void Spawn()
    {
        float randomX;
        if (level == gm.winningLevel)
        {
            randomX = 0;
            obstacleToSpawn = finishLine;
            finishSpawned = true;
        }
        else
        {
            randomX = Random.Range(-7f, 7f);
            obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        }

        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, new Vector2(randomX, transform.position.y), Quaternion.identity, obstacleParent);
        Obstacle obstacle = spawnedObstacle.GetComponent<Obstacle>();
        obstacle.speed = _obstacleSpeed;

        //Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        //obstacleRB.MovePosition(obstacleRB.position + (Vector2.down * GameManager.instance.moveSpeed * Time.deltaTime));
        //obstacleRB.MovePosition(obstacleRB.position + (Vector2.down) * _obstacleSpeed * Time.deltaTime);
        // obstacleRB.velocity = Vector2.down * _obstacleSpeed;

    }
    
    /// <summary>
    /// Clears all obstacles from level
    /// </summary>
    private void ClearObstacles()
    {
        foreach (Transform child in obstacleParent)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Calculates the obstacle spawn time and the speed
    /// </summary>
    private void CalculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * MathF.Pow(timeAlive, obstacleSpeedFactor);
    }

    /// <summary>
    /// Updates the level progress text so players know how far they are from the finish
    /// </summary>
    private void UpdateLevelText()
    {
        float progress = ((float)level / (float)GameManager.instance.winningLevel) * 100;
        levelProgressText.text = $"Level Progress: {progress}%";
    }

    /// <summary>
    /// Resets level values that need to be reset each run
    /// </summary>
    private void ResetFactors()
    {
        level = 0;
        UpdateLevelText();
        finishSpawned = false;
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
    }
}