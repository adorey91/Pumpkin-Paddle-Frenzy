using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private float timeUntilObstacleSpawn;
    private float timeUntilAppleSpawn;

    public float appleSpawnTime = 4;
    public float obstacleSpawnTime = 3;

    public GameObject[] obstacles;
    public GameObject apples;

    [SerializeField]private List<GameObject> itemList = new List<GameObject>();


    private void Update()
    {
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            GameObject randomObstacle = obstacles[Random.Range(0, obstacles.Length)];
            SpawnItem(randomObstacle);
            timeUntilObstacleSpawn = 0f;
        }

        timeUntilAppleSpawn += Time.deltaTime;
        if(timeUntilAppleSpawn >= appleSpawnTime)
        {
            SpawnItem(apples);
            timeUntilAppleSpawn = 0f;
        }

    }

    private void SpawnItem(GameObject item)
    {
        float randomX = Random.Range(-7, 7);
        Vector2 spawnLoc = new Vector2(randomX, transform.position.y);

        var obstacle = Instantiate(item, spawnLoc, Quaternion.identity, transform);
        itemList.Add(obstacle);
    }

    public void DestroyItems()
    {
        if(itemList.Count > 0)
        {
            foreach(var item in itemList)
                Destroy(item);
            
            itemList.Clear();
        }
        timeUntilObstacleSpawn = 0f;
        timeUntilAppleSpawn = 0f;
    }
}
