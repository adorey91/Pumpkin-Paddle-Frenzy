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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            HealthSystem player = collision.gameObject.GetComponent<HealthSystem>();
            switch (obstacleType)
            {
                case ObstacleType.Currency:
                    ScoreManager.applesThisRun++;
                    ScoreManager.appleCount++;

                    ScoreManager.Instance.UpdateText();
                    break;
                case ObstacleType.AvoidThis:
                    player.TakeDamage();
                    break;
            }
            Destroy(gameObject);
        }
    }
}