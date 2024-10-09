using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("UI Stats")]
    [SerializeField] private Image staminaImage;
    [SerializeField] private TMP_Text healthText;

    [Header("Upgrades")]
    public UpgradeAsset curHealthUpgrade;
    public UpgradeAsset curStaminaUpgrade;

    [Header("Current Stats")]
    public int maxHealth = 1;
    private int curHealth;
    public float staminaDrain = 0.5f;

    [Header("PlayerSprite")]
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer boatSprite;
    [SerializeField] private SpriteRenderer paddleSprite;
    private CircleCollider2D playerCollider;

    public int flickerAmount = 3;
    public float flickerDuration = 0.1f;

    public void Update()
    {
        if (GameManager.instance.isPlaying)
            StaminaDrain();
    }

    private void OnEnable()
    {
        Actions.OnPlayerHurt += TakeDamage;
        Actions.OnGameplay += UpdateHealthStats;
        Actions.OnGameWin += DisableSprite;
        Actions.OnGameOver += DisableSprite;
    }

    private void OnDisable()
    {
        Actions.OnPlayerHurt -= TakeDamage;
        Actions.OnGameplay -= UpdateHealthStats;
        Actions.OnGameWin -= DisableSprite;
        Actions.OnGameOver -= DisableSprite;
    }

    private void StaminaDrain()
    {
        float drainSpeedMultiplier = 0.1f;  // Adjust this value to control the drain rate

        if (staminaImage.fillAmount > 0)
        {
            staminaImage.fillAmount -= staminaDrain * drainSpeedMultiplier * Time.deltaTime;

            if (staminaImage.fillAmount <= 0)  // Changed from `==` to `<=` for precision
            {
                Actions.OnPlayerHurt();
                if (curHealth > 0)
                    staminaImage.fillAmount = 1;
            }
        }
    }


    private void TakeDamage()
    {
        curHealth--;
        healthText.text = $"x {curHealth}";
        StartCoroutine(DamageFlicker());
    }

    public void UpdateHealthStats()
    {
        ActivateSprite();

        staminaImage.fillAmount = 1;
        curHealth = maxHealth;
        healthText.text = $"x {curHealth}";
    }

    private void ActivateSprite()
    {
        playerSprite.enabled = true;
        boatSprite.enabled = true;
        paddleSprite.enabled = true;
    }   
    
    private void DisableSprite()
    {
        playerSprite.enabled = false;
        boatSprite.enabled = false;
        paddleSprite.enabled = false;
    }

    private IEnumerator DamageFlicker()
    {
        playerCollider = player.GetComponent<CircleCollider2D>();
        playerCollider.enabled = false;

        for (int i = 0; i < flickerAmount; i++)
        {
            boatSprite.color = new Color(1f, 1f, 1f, 0.5f);
            paddleSprite.color = new Color(1f, 1f, 1f, 0.5f);
            playerSprite.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(flickerDuration);
            boatSprite.color = Color.white;
            paddleSprite.color = Color.white;
            playerSprite.color = Color.white;
            yield return new WaitForSeconds(flickerDuration/2);
        }
        if (curHealth <= 0)
            Actions.OnGameOver();

        playerCollider.enabled = true;
    }
}
