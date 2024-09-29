using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    // movement variables
    [Header("Movement Variables")]
    private Rigidbody2D rb2D;
    private Vector2 movement;
    [Range(2, 10)] public float speed = 4f;
    [SerializeField] private GameObject playerSprite;

    // health
    [SerializeField] private HealthSystem healthSystem;
    // sound
    [SerializeField] private SoundManager soundManager;
    // score
    [SerializeField] private ScoreManager scoreManager;


    private void Awake()
    {
        instance = this;
        rb2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (GameManager.instance.isPlaying)
            Movement();
    }

    private void Movement()
    {
        movement.y = 0;
        rb2D.MovePosition(rb2D.position + movement * speed * Time.deltaTime);
    }

    public void ActiveSprite(bool isActive)
    {
        if (playerSprite == null)
        {
            Debug.LogError("playerSprite is null!");
            playerSprite = gameObject.GetComponentInChildren<GameObject>();
            return;
        }

        playerSprite.SetActive(isActive);
    }


    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.instance.state == GameManager.GameState.Gameplay || GameManager.instance.state == GameManager.GameState.Pause)
                GameManager.instance.EscapeState();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Obstacle>())
        {
            Obstacle obs = collision.gameObject.GetComponent<Obstacle>();

            switch (obs.obstacleType)
            {
                case Obstacle.ObstacleType.Currency:
                    scoreManager.applesThisRun++;
                    scoreManager.appleCount++;
                    scoreManager.UpdateText();
                    soundManager.PlaySfxAudio("Collect");
                    break;
                case Obstacle.ObstacleType.AvoidThis:
                    healthSystem.TakeDamage();
                    soundManager.PlaySfxAudio("Crash");
                    break;
            }
            Destroy(collision.gameObject);
        }
    }
}
