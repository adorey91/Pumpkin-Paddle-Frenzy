using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class CharacterController : MonoBehaviour
{
    public static CharacterController instance;

    private Rigidbody2D rb2D;
    private Vector2 movement;
    [Range(2, 10)]public float speed = 2f;
    [SerializeField] private Image staminaImage;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text currencyCount;

    [Header("Upgrades")]
    public UpgradeAsset currentHealthUpgrade;
    public UpgradeAsset currentStaminaUpgrade;

    [Header("Counts")]
    public int appleCount;
    private int maxHealth =1; // setting this to 1 for now.
    public int healthAmount = 1;
    public float staminaDrain = 0.1f;  // the higher the upgrades the lower the drain



    private void Start()
    {
        instance = this;

        rb2D = GetComponent<Rigidbody2D>();
        appleCount = 0;
        currencyCount.text = $"x {appleCount}";
    }

    private void FixedUpdate()
    {
        Movement();
        StaminaDrain();
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

    private void StaminaDrain()
    {
        if(staminaImage.fillAmount > 0)
        {
            staminaImage.fillAmount -= staminaDrain * Time.deltaTime;

            if(staminaImage.fillAmount == 0)
                PlayerDamage();
        }
    }

    public void PlayerReset()
    {
        staminaImage.fillAmount = 1;
        healthAmount = maxHealth;
        healthText.text = $"x {healthAmount}";
    }

    public void PlayerDamage()
    {
        healthAmount--;
        healthText.text = $"x {healthAmount}";

        if(healthAmount <= 0)
        {
            GameManager.instance.LoadState("Upgrades");
        }
    }
}
