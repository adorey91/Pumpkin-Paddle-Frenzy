using UnityEngine;
using UnityEngine.InputSystem;
using static SpawnableBehaviour;

public class PlayerController : MonoBehaviour
{
    // playerMovement variables
    [Header("Movement Variables")]
    private float baseMoveSpeed;
    public float moveSpeed = 4f; // Speed of player left and right playerMovement

    private Rigidbody2D rb;
    private Vector2 playerMovement;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isPlaying)
            Movement();
    }


    #region ActionsEnableDisable
    private void OnEnable()
    {
        Actions.SpeedChange += IncreaseMovementSpeed;
        Actions.OnGameplay += ResetPlayer;
    }

    private void OnDisable()
    {
        Actions.SpeedChange -= IncreaseMovementSpeed;
        Actions.OnGameplay -= ResetPlayer;
    }
    #endregion

    private void Movement()
    {
        rb.MovePosition(rb.position + new Vector2(playerMovement.x, 0) * moveSpeed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerMovement = context.ReadValue<Vector2>();
    }
       
    // Pauses when player presses the button set in the PlayerInputManager it will go to pause or back to gameplay ONLY in gameplay / pause
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.instance.state == GameManager.GameState.Gameplay || GameManager.instance.state == GameManager.GameState.Pause)
                GameManager.instance.EscapeState();
        }
    }

    #region TouchScreenMovement
    // Called when left button is pressed
    public void MoveLeftButtonPress()
    {
        isMovingLeft = true;
        isMovingRight = false; // Disable right movement if left is pressed
        playerMovement = new Vector2(-1, 0); // Simulate left movement
    }

    // Called when right button is pressed
    public void MoveRightButtonPress()
    {
        isMovingRight = true;
        isMovingLeft = false; // Disable left movement if right is pressed
        playerMovement = new Vector2(1, 0); // Simulate right movement
    }

    // Optional: if you want to stop movement when the button is released
    public void StopMovement()
    {
        isMovingLeft = false;
        isMovingRight = false;
        playerMovement = Vector2.zero; // Stop movement
    }
    #endregion
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        SpawnableBehaviour obstacle = other.GetComponent<SpawnableBehaviour>();
        SpawnableObject spawnable = obstacle.GetSpawnableObject();

        switch (spawnable.type)
        {
            case PoolType.Obstacle: 
                Actions.OnPlayerHurt(); 
                break;
            case PoolType.Collectable: 
                string collectable = spawnable.collectableValue == 1 ? "apple" : "golden"; 
                Actions.AppleCollection(collectable);
                break;
            case PoolType.FinishLine: 
                Actions.OnGameWin(); 
                break;
        }
        Actions.OnReturn(spawnable.type, other.gameObject);
    }

    private void IncreaseMovementSpeed(float timeAlive)
    {
        moveSpeed = baseMoveSpeed * Mathf.Pow(timeAlive, 0.15f);
    }

    private void ResetPlayer()
    {
        moveSpeed = baseMoveSpeed;
        transform.position = new Vector2(0, -2.7f);
    }
}
