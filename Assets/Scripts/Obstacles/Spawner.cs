using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Obstacles To Be Spawned")]
    [SerializeField] private List<SpawnableObjects> spawnableObjects;

    [Header("Spawning Base Values")]
    [SerializeField] private float obstacleSpawnTime = 5f;
    [Range(0f, 1f)] public float obstacleSpawnFactor = 0.075f;
    [SerializeField] private float obstacleSpeed = 1f;
    [Range(0f, 1f)] public float obstacleSpeedFactor = 0.15f;
    public float backgroundSpeed;

    private float _obstacleSpawnTime;
    private float _obstacleSpeed;

    internal static float timeAlive = 1;
    [SerializeField] private float timeUntilObstacleSpawn;


    private float recalculateTime;

    public float calculateTime = 10f;

    public static int level = 0; // keep track of times the time increased
    internal static int winningLevel;

    private bool finishSpawned = false;


    private void Start()
    {
        winningLevel = GameManager.instance.winningLevel;
        Actions.OnLevelIncrease();
        timeAlive = 1;
    }

    private void Update()
    {
        if (!finishSpawned && GameManager.instance.isPlaying)
            SpawnLoop();
    }

    private void OnEnable()
    {
        Actions.OnGameplay += ResetValues;
        Actions.OnGameOver += ClearObstacles;
        Actions.OnGameWin += ClearObstacles;
    }

    private void OnDisable()
    {
        Actions.OnGameplay -= ResetValues;
        Actions.OnGameOver -= ClearObstacles;
        Actions.OnGameWin -= ClearObstacles;
    }

    private void CalculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);

        backgroundSpeed = _obstacleSpeed * 0.3f;

        foreach (Transform child in transform)
        {
            child.GetComponent<Obstacle>().UpdateSpeed(_obstacleSpeed);
        }
    }

    #region Spawn
    /// <summary>
    /// Spawn Loop
    /// </summary>
    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;
        recalculateTime += Time.deltaTime;
        timeAlive += Time.deltaTime;

        if ((recalculateTime >= calculateTime) && level < winningLevel)
        {
            CalculateFactors();
            recalculateTime = 0;
            level++;
            Debug.Log(level);
            Actions.OnLevelIncrease();
        }

        if (timeUntilObstacleSpawn >= _obstacleSpawnTime)
        {
            Spawn();
            timeUntilObstacleSpawn = 0;
        }
    }

    private void Spawn()
    {
            // Spawn the finish line if the player reaches the winning level if the game isnt endless
        if (level == winningLevel && !GameManager.instance.gameIsEndless)
        {
            SpawnObject(spawnableObjects.Find(obj => obj.type == SpawnableObjects.ObjectType.FinishLine));
            finishSpawned = true;
        }
        else
        {
            SpawnObject(GetRandomSpawnable());
        }
    }

    private void SpawnObject(SpawnableObjects spawnableObject)
    {
        float randomX = Random.Range(-6.7f, 6.7f);
        GameObject spawnedObject = Instantiate(spawnableObject.spawnPrefab, new Vector2(randomX, transform.position.y), Quaternion.identity, transform);

        spawnedObject.GetComponent<Obstacle>().Initialize(spawnableObject);
        spawnedObject.GetComponent<Obstacle>().UpdateSpeed(_obstacleSpeed);
    }

    private SpawnableObjects GetRandomSpawnable()
    {
        float totalWeight = 0f;
        foreach(var obj in spawnableObjects)
        {
            totalWeight += obj.spawnWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var obj in spawnableObjects)
        {
            cumulativeWeight += obj.spawnWeight;
            if (randomValue <= cumulativeWeight)
            {
                return obj;
            }
        }

        return spawnableObjects[0]; // Fallback
    }
    #endregion

    #region Resets
    private void ClearObstacles()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ResetValues()
    {
        level = 0;
        Actions.OnLevelIncrease();
        timeAlive = 1;
        finishSpawned = false;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
        backgroundSpeed = _obstacleSpeed * 0.3f;
    }
    #endregion
}