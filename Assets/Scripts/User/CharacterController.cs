using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public static CharacterController instance;

    [SerializeField] private GameObject rightBound;
    [SerializeField] private GameObject leftBound;
    private Rigidbody2D rb2D;

    public UpgradeAsset currentHealthUpgrade;
    public UpgradeAsset currentStaminaUpgrade;
    public int appleCount;
    public int healthAmount;
    public int staminaAmount;

   

    private void Start()
    {
        instance = this;

        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Theres a better way to handle this. it goes past the barrier then gets stuck.
        if (gameObject.transform.position.x < leftBound.transform.position.x || gameObject.transform.position.x > rightBound.transform.position.x) 
            return;
        else
            gameObject.transform.Translate(movementVector);
    }

    private void OnPause()
    {
        if(GameManager.instance.state == GameManager.GameState.Gameplay || GameManager.instance.state == GameManager.GameState.Pause)
            GameManager.instance.EscapeState();
    }
}
