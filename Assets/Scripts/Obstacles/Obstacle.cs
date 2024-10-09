using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Apple, GoldenApple, AvoidThis, Finish }
    public ObstacleType obstacleType;
    public float speed;


    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //transform.position -= new Vector3(0, Time.deltaTime * GameManager.instance.moveSpeed * speed, 0);
        rb.MovePosition(rb.position + (Vector2.down * GameManager.instance.moveSpeed * speed * Time.deltaTime));
    }
}