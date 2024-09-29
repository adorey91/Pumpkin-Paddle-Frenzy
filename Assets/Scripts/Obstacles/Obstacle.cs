using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Currency, AvoidThis }
    public ObstacleType obstacleType;

    public float speed = 1.9f;

    void Update()
    {
        //Scroll down the object
        transform.position -= new Vector3(0, Time.deltaTime * GameManager.instance.moveSpeed * speed, 0);
    }
}