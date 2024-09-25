using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public float appleSpawnTime = 4f;
    public float obstacleSpawnTime = 3f;

    public GameObject[] obstacles;
    public GameObject apples;

    [SerializeField] private Queue<GameObject> pooledObstacles = new Queue<GameObject>();
    [SerializeField] private Queue<GameObject> pooledApples = new Queue<GameObject>();

    public int poolSize = 10; // Number of objects to pre-instantiate in the object pool
    
    private Coroutine obstacleSpawnCoroutine;
    private Coroutine appleSpawnCoroutine;

    private void Start()
    {
        InitializeObjectPool();
        StartSpawning();
    }
    public void StartSpawning()
    {
        // Start the obstacle and apple spawning coroutines
        obstacleSpawnCoroutine = StartCoroutine(SpawnObstacles());
        appleSpawnCoroutine = StartCoroutine(SpawnApples());
    }

    private void InitializeObjectPool()
    {
        // Pre-instantiate obstacle objects
        for (int i = 0; i < poolSize; i++)
        {
            // Randomly choose an obstacle to pool
            GameObject randomObstacle = obstacles[Random.Range(0, obstacles.Length)];
            GameObject obj = Instantiate(randomObstacle, Vector2.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            pooledObstacles.Enqueue(obj);
        }

        // Pre-instantiate apples
        for (int i = 0; i < poolSize / 2; i++)
        {
            GameObject obj = Instantiate(apples, Vector2.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            pooledApples.Enqueue(obj);
        }
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(obstacleSpawnTime);

            GameObject randomObstacle = GetPooledObject(pooledObstacles, obstacles[Random.Range(0, obstacles.Length)]);
            SpawnItem(randomObstacle);
        }
    }

    private IEnumerator SpawnApples()
    {
        while (true)
        {
            yield return new WaitForSeconds(appleSpawnTime);

            GameObject apple = GetPooledObject(pooledApples, apples);
            SpawnItem(apple);
        }
    }

    private GameObject GetPooledObject(Queue<GameObject> pool, GameObject prefab)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(prefab);
            return newObj;
        }

    }

    private void SpawnItem(GameObject item)
    {
        float randomX = Random.Range(-7, 7);
        Vector2 spawnLoc = new Vector2(randomX, transform.position.y);


        var obstacle = Instantiate(item, spawnLoc, Quaternion.identity, transform);
        itemList.Add(obstacle);
    }

    public void ReturnToPool(GameObject item)
    {
        // Disable the object and return it to the appropriate pool
        item.SetActive(false);

        if (item.CompareTag("Obstacle"))
            pooledObstacles.Enqueue(item);
        else if (item.CompareTag("Apple"))
            pooledApples.Enqueue(item);
    }

    public void StopRoutines()
    {
        StopAllCoroutines();
    }
}