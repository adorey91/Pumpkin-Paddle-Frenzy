using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public float appleSpawnTime = 4f;
    public float obstacleSpawnTime = 3f;

    public GameObject[] obstacles;
    public GameObject apples;

    [SerializeField] private List<GameObject> itemList = new List<GameObject>();

    private Coroutine obstacleSpawnCoroutine;
    private Coroutine appleSpawnCoroutine;

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        // Start the obstacle and apple spawning coroutines
        obstacleSpawnCoroutine = StartCoroutine(SpawnObstacles());
        appleSpawnCoroutine = StartCoroutine(SpawnApples());
    }

    public void StopSpawning()
    {
        // Stop both coroutines when needed
        if (obstacleSpawnCoroutine != null)
        {
            StopCoroutine(obstacleSpawnCoroutine);
            obstacleSpawnCoroutine = null;
        }

        if (appleSpawnCoroutine != null)
        {
            StopCoroutine(appleSpawnCoroutine);
            appleSpawnCoroutine = null;
        }
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(obstacleSpawnTime);

            GameObject randomObstacle = obstacles[Random.Range(0, obstacles.Length)];
            SpawnItem(randomObstacle);
        }
    }

    private IEnumerator SpawnApples()
    {
        while (true)
        {
            yield return new WaitForSeconds(appleSpawnTime);

            SpawnItem(apples);
        }
    }

    private void SpawnItem(GameObject item)
    {
        float randomX = Random.Range(-7, 7);
        Vector2 spawnLoc = new Vector2(randomX, transform.position.y);
        GameObject obstacle = Instantiate(item, spawnLoc, Quaternion.identity);
        itemList.Add(obstacle);
    }

    // This method clears spawned items and resets state
    public void DestroyItemsAndReset()
    {
        foreach (var item in itemList)
        {
            Destroy(item);
        }

        itemList.Clear();

        // Optionally reset timers or any variables here
    }
}
