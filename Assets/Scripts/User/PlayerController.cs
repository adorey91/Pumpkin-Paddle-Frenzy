using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // movement variables
    [Header("Movement Variables")]
    private Rigidbody2D rb2D;
    private Vector2 movement;
    [Range(2, 10)] public float speed = 4f;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (GameManager.instance.isPlaying)
            Movement();
        else
            transform.position= new Vector2(0,-2.7f);
    }

    private void Movement()
    {
        movement.y = 0;
        rb2D.MovePosition(rb2D.position + movement * speed * Time.deltaTime);
    }

    public void ActivateSprite()
    {
        foreach (Transform spriteTransform in transform)
        {
            spriteTransform.gameObject.SetActive(true);
        }
    }

    public void DisableSprite()
    {
        foreach (Transform spriteTransform in transform)
        {
            spriteTransform.gameObject.SetActive(false);
        }
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
                    Actions.OnCollectApple();
                    break;
                case Obstacle.ObstacleType.AvoidThis:
                    Actions.OnPlayerHurt();
                    break;
                case Obstacle.ObstacleType.Finish:
                    Actions.OnGameOver();
                    break;
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnEnable()
    {
        Actions.OnGameplay += ActivateSprite;
        Actions.OnGameWin += DisableSprite;
    }

    private void OnDisable()
    {
        Actions.OnGameplay -= ActivateSprite;
        Actions.OnGameWin -= DisableSprite;
    }
}
