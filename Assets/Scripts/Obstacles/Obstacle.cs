using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Currency, AvoidThis, Finish }
    public ObstacleType obstacleType;
}