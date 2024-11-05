using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Attempt to get the SpawnableBehaviour component from other or its parent
        SpawnableBehaviour obstacle = other.GetComponent<SpawnableBehaviour>() ?? other.GetComponentInParent<SpawnableBehaviour>();
        GameObject obstacleObj = obstacle.gameObject;

        // Retrieve SpawnableObject and check for null
        SpawnableObject spawnable = obstacle.GetSpawnableObject();
        if (spawnable == null)
        {
            Debug.LogWarning("SpawnableObject not found in SpawnableBehaviour.");
            return;
        }

        // Trigger the appropriate action
        Actions.OnReturn(spawnable.type, obstacleObj, false);
    }
}
