using UnityEngine;
using UnityEngine.InputSystem;
using static Obstacle;

public class PlayerController : MonoBehaviour
{
    // playerMovement variables
    [Header("Movement Variables")]
    private float baseMoveSpeed;
    public float moveSpeed = 4f; // Speed of player left and right playerMovement

    private Rigidbody2D rb;
    private Vector2 playerMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isPlaying)
            Movement();
        else
            transform.position = new Vector2(0, -2.7f);
    }

    private void OnEnable()
    {
        Actions.OnLevelIncrease += IncreaseMovementSpeed;
        Actions.OnGameplay += ResetMovementSpeed;
    }

    private void OnDisable()
    {
        Actions.OnLevelIncrease -= IncreaseMovementSpeed;
        Actions.OnGameplay -= ResetMovementSpeed;
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + new Vector2(playerMovement.x, 0) * moveSpeed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerMovement = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Pauses when player presses the button set in the PlayerInputManager it will go to pause or back to gameplay ONLY in gameplay / pause
    /// </summary>
    /// <param name="context"></param>
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.instance.state == GameManager.GameState.Gameplay || GameManager.instance.state == GameManager.GameState.Pause)
                GameManager.instance.EscapeState();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();

        switch (obstacle.spawnableObject.type)
        {
            case SpawnableObjects.ObjectType.Collectable:
                if (obstacle.spawnableObject.collectableValue == 1)
                    Actions.OnCollectApple();
                else if (obstacle.spawnableObject.collectableValue > 1)
                    Actions.OnCollectGoldenApple();
                break;

            case SpawnableObjects.ObjectType.Obstacle:
                Actions.OnPlayerHurt();
                break;

            case SpawnableObjects.ObjectType.FinishLine:
                Actions.OnGameWin();
                break;
        }

        Destroy(other.gameObject);
    }

    private void IncreaseMovementSpeed()
    {
        moveSpeed = baseMoveSpeed * Mathf.Pow(Spawner.timeAlive, 0.15f);
    }

    private void ResetMovementSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
}
