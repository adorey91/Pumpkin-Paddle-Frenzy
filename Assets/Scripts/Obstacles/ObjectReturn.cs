using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReturn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.name);
        SpawnableBehaviour obstacle = other.GetComponent<SpawnableBehaviour>();
        SpawnableObject spawnable = obstacle.GetSpawnableObject();
        Actions.OnReturn(spawnable.type, other.gameObject, false);
    }
}
