using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableBehaviour : MonoBehaviour
{
    [SerializeField] private SpawnableObject spawnableObject;
    private Rigidbody2D rb;
    private bool moveLeft;
    private bool moveRight;
    private bool setMovement;

    // kayak movement
    Vector2 leftMovement = new Vector2(-1, 0);
    Vector2 rightMovement = new Vector2(1, 0);
    Vector2 downMovement = Vector2.down;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (spawnableObject.type == PoolType.Kayak)
        {
            if (!setMovement)
            {
                if (transform.position.x > 0)
                    moveLeft = true;
                else
                    moveRight = true;

                setMovement = true;
            }

            if (moveLeft)
            {
                rb.MovePosition(rb.position + (leftMovement * (spawnableObject.speed / 2) * Time.deltaTime) + (downMovement * spawnableObject.speed * Time.deltaTime));
                if (transform.position.x <= -6)
                    moveLeft = false;
            }
            if (moveRight)
            {
                rb.MovePosition(rb.position + (rightMovement * (spawnableObject.speed / 2) * Time.deltaTime) + (downMovement * spawnableObject.speed * Time.deltaTime));
                if (transform.position.x >= 6)
                    moveRight = false;
            }
        }
        else
        {
            rb.MovePosition(rb.position + (Vector2.down * spawnableObject.speed * Time.deltaTime));
        }
    }

    public SpawnableObject GetSpawnableObject() { return spawnableObject; }
}
