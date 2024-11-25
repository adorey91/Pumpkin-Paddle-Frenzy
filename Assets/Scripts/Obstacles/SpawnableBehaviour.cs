using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableBehaviour : MonoBehaviour
{
    [SerializeField] private SpawnableObject spawnableObject;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (Vector2.down * spawnableObject.speed * Time.deltaTime));
    }

    public SpawnableObject GetSpawnableObject() { return spawnableObject; }
}
