using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
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
            CharacterController player = collision.gameObject.GetComponent<CharacterController>();
            switch (obstacleType)
            {
                case ObstacleType.Currency:
                    ScoreManager.applesThisRun++;
                    ScoreManager.appleCount++;

                    ScoreManager.Instance.UpdateText();
                    break;
                case ObstacleType.AvoidThis:
                    player.PlayerDamage();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
