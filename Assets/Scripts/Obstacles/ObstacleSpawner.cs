using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;


    private float timeUntilObstacleSpawn;

    private void Update()
    {
        if (GameManager.instance.isPlaying) 
            SpawnLoop();
        else
            DestroyObstacles();

    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void Spawn()
    {
        float randomX = Random.Range(-7f, 7f);

        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, new Vector2(randomX, transform.position.y), Quaternion.identity, obstacleParent);

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.down * obstacleSpeed;
    }

    private void DestroyObstacles()
    {
        // Create a list to hold the child objects
        List<GameObject> childrenObj = new List<GameObject>();
        
        // Loop through all child objects and add them to the list
        foreach (Transform child in transform)
        {
            childrenObj.Add(child.gameObject);
        }
        
        // Now loop through the list and destroy each child object
        foreach (GameObject child in childrenObj)
        {
            Destroy(child);
        }
    }

}