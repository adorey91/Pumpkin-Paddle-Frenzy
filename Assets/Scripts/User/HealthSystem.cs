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
    private CircleCollider2D playerCollider;

    public int flickerAmount = 3;
    public float flickerDuration = 0.1f;
    public Color flickerColor = Color.red;
    private SpriteRenderer[] playerSprites;

    public void Awake()
    {
        playerSprites = GetComponentsInChildren<SpriteRenderer>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        DisableSprite();
    }

    public void Update()
    {
        if (GameManager.instance.isPlaying)
            StaminaDrain();
    }

    private void OnEnable()
    {
        Actions.OnPlayerHurt += TakeDamage;
        Actions.OnGameplay += ActivateSprite;
        Actions.OnGameplay += UpdateHealthStats;
        Actions.OnGameWin += DisableSprite;
        Actions.OnGameOver += DisableSprite;
        Actions.LoadSettings += UpdateHealthStats;
    }

    private void OnDisable()
    {
        Actions.OnPlayerHurt -= TakeDamage;
        Actions.OnGameplay -= ActivateSprite;
        Actions.OnGameplay -= UpdateHealthStats;
        Actions.OnGameWin -= DisableSprite;
        Actions.OnGameOver -= DisableSprite;
        Actions.LoadSettings -= UpdateHealthStats;
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

    private void UpdateHealthStats()
    {
        staminaImage.fillAmount = 1;
        curHealth = maxHealth;
        healthText.text = $"x {curHealth}";
    }

    private void ActivateSprite()
    {
        foreach (SpriteRenderer sprite in playerSprites)
        {
            sprite.enabled = true;
        }
    }

    private void DisableSprite()
    {
        foreach (SpriteRenderer sprite in playerSprites)
        {
            sprite.enabled = false;
        }
    }

    private IEnumerator DamageFlicker()
    {
        playerCollider.enabled = false;

        for (int i = 0; i < flickerAmount; i++)
        {
            foreach (SpriteRenderer sprite in playerSprites)
            {
                sprite.color = flickerColor;
            }

            yield return new WaitForSeconds(flickerDuration);
            foreach (SpriteRenderer sprite in playerSprites)
            {
                sprite.color = Color.white;
            }
            yield return new WaitForSeconds(flickerDuration / 2);
        }
        if (curHealth <= 0)
            Actions.OnGameOver();

        playerCollider.enabled = true;
    }
}
