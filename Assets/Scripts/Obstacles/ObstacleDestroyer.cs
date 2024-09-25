using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    private ObstacleSpawner spawner;  // Reference to the spawner

    private void Start()
    {
        spawner = FindObjectOfType<ObstacleSpawner>();  // Find the spawner in the scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacle obstacle = collision.GetComponent<Obstacle>();

        if (obstacle != null)
        {
            // Return the obstacle to the pool instead of destroying it
            spawner.ReturnToPool(collision.gameObject);
        }
    }
}
