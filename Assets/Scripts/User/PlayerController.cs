using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool isMovingLeft = false;
    public bool isMovingRight = false;

    private bool isNotMoving = true; // tracking current movement state
    private bool wasNotMoving = true; // used to track previous movement state

    public CustomTimer movementTimer;
    public float forceSpawnCount = 14f;
    bool restartedTimer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed;
        movementTimer = new CustomTimer(forceSpawnCount);
    }

    private void Update()
    {
        if (GameManager.instance.isPlaying)
        {
            if (isNotMoving)
            {
                // if the player is still not moving update the timer
                if (wasNotMoving)
                {
                    if (movementTimer.UpdateTimer(Time.deltaTime))
                    {
                        Actions.ForceSpawn(transform.position.x);
                        movementTimer.StartTimer(forceSpawnCount);
                    }
                }
                else
                {
                    // player just stopped moving, reset the timer
                    movementTimer.ResetTimer();
                    wasNotMoving = true;
                }
            }
            else
            {
                // player is moving
                wasNotMoving = false;
            }
        }
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

        if (playerMovement.x > 0)
        {
            isMovingLeft = false;
            isMovingRight = true;
            isNotMoving = false;
        }
        else if (playerMovement.x < 0)
        {
            isMovingLeft = true;
            isMovingRight = false;
            isNotMoving = false;
        }
        else
        {
            StopMovement();
        }
    }

    public void IncreaseSpeed(InputAction.CallbackContext context)
    {
       if(context.performed)
            Actions.OnUseEnergy();
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerMovement = context.ReadValue<Vector2>();
    }

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
        isNotMoving = true;
        playerMovement = Vector2.zero; // Stop movement
        restartedTimer = false;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obstacleObj = collision.gameObject;
        SpawnableBehaviour obstacle = obstacleObj.gameObject.GetComponent<SpawnableBehaviour>() ?? obstacleObj.GetComponentInParent<SpawnableBehaviour>();

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
                if (spawnable.name == "Apple" || spawnable.name == "GoldenApple")
                {
                    string collectable = spawnable.collectableValue == 1 ? "apple" : "golden";
                    Actions.OnReturn(PoolType.Collectable, obstacleObj, true);
                    Actions.AppleCollection(collectable);
                }
                else
                {
                    Actions.OnReturn(PoolType.Collectable, obstacleObj, true);
                    Actions.EnergyCollection();
                }
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
        movementTimer.StartTimer(forceSpawnCount);
        moveSpeed = baseMoveSpeed;
        transform.position = new Vector2(0, -2.7f);
    }
}
