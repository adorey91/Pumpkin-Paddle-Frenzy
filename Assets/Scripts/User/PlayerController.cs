using UnityEditor.EditorTools;
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
    }

    private void OnDisable()
    {
        Actions.SpeedChange -= IncreaseMovementSpeed;
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
