using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public SpawnableObjects spawnableObject;
    public float movementSpeed;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (Vector2.down * movementSpeed * Time.deltaTime));
    }

    public void Initialize(SpawnableObjects spawnable)
    {
        spawnableObject = spawnable;
    }

    public void UpdateSpeed(float newSpeed)
    {
        movementSpeed = newSpeed;
    }
}