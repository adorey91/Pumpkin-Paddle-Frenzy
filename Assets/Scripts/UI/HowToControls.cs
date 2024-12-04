using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToControls : MonoBehaviour
{
    [SerializeField] private Animator obstacle;
    [SerializeField] private Animator player;

    public void IncreaseSpeed()
    {
        obstacle.speed += 0.5f;
        player.speed += 0.5f;
    }

    public void DecreaseSpeed()
    {
        obstacle.speed -= 0.5f;
        player.speed -= 0.5f;
    }
}
