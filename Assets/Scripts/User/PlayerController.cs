using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    // movement variables
    [Header("Movement Variables")]
    private Rigidbody2D rb2D;
    private Vector2 movement;
    [Range(2, 10)] public float speed = 4f;

    private void Start()
    {
        instance = this;

        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        movement.y = 0;
        rb2D.MovePosition(rb2D.position + movement * speed * Time.deltaTime);
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
}
