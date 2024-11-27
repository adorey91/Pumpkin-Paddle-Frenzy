using UnityEngine;
using UnityEngine.InputSystem;
using static SpawnableBehaviour;

public class PlayerController : MonoBehaviour
{
    // playerMovement variables
    [Header("Movement Variables")]
    private float baseMoveSpeed;
    [SerializeField] private float moveSpeed = 4f; // Speed of player left and right playerMovement
    [SerializeField] private float decelerationFactor = 3f; // How fast the player stops moving

    private Rigidbody2D rb;
    private Vector2 playerMovement;
    private Vector2 currentVelocity; // Used to store the current velocity of the player

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
    // used for touch screen when left button is pressed
    public void MoveLeftButtonPress()
    {
        isMovingLeft = true;
        isMovingRight = false;
        playerMovement = Vector2.Lerp(playerMovement, new Vector2(-1, 0), decelerationFactor * Time.deltaTime);
    }

    public void MoveRightButtonPress()
    {
        isMovingRight = true;
        isMovingLeft = false;
        playerMovement = Vector2.Lerp(playerMovement, new Vector2(1, 0), decelerationFactor * Time.deltaTime);
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
        SpawnableBehaviour obstacle = other.GetComponent<SpawnableBehaviour>() ?? other.GetComponentInParent<SpawnableBehaviour>();
        GameObject obstacleObj = obstacle.gameObject;

        if (obstacle == null)
        {
            Debug.LogWarning("SpawnableBehaviour not found on collided object or its parent.");
            return;
        }

        SpawnableObject spawnable = obstacle.GetSpawnableObject();

        switch (spawnable.type)
        {
            case PoolType.Obstacle:
                Actions.OnPlayerHurt();
                break;
            case PoolType.Collectable:
                string collectable = spawnable.collectableValue == 1 ? "apple" : "golden";
                Actions.OnReturn(PoolType.Collectable, obstacleObj, true);
                Actions.AppleCollection(collectable);
                break;
            case PoolType.FinishLine:
                Actions.OnGameWin();
                break;
            case PoolType.Kayak:
                Actions.OnPlayerHurt();
                break;
        }

        if (spawnable.type != PoolType.Collectable)
            Actions.OnReturn(spawnable.type, obstacleObj, false);
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
