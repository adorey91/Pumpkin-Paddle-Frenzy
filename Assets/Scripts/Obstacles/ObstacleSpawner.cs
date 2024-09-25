using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public float timeSinceLastObstacle;

    public GameObject[] obstacles;
    public GameObject apples;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
            InstantiateObstacle();
    }

    private void InstantiateObstacle()
    {
        float randomX = Random.Range(-7, 7);
        Vector2 spawnLoc = new Vector2 (randomX, transform.position.y);

        int randomObstacle = Random.Range(0, obstacles.Length);

        var obstacle = Instantiate(obstacles[randomObstacle], spawnLoc, Quaternion.identity);
    }
}
