using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("UI Stats")]
    [SerializeField] private GameObject[] stamina;
    [SerializeField] private Image staminaImage;
    [SerializeField] private Image healthFillImage;
    [SerializeField] private Image[] healthIcons;


    // PlayerUpgrades
    [Header("Upgrades")]
    private UpgradeAsset curHealthUpgrade;
    private UpgradeAsset curStaminaUpgrade;

    // Base Stats
    [Header("Base Stats")]
    public int baseMaxHealth = 1;
    public float baseStaminaDrain = 0.5f;
    public Sprite baseHealthSprite;
    public Sprite baseStaminaSprite;

    // Updating Stats
    [Header("Current Stats")]
    private int maxHealth;
    private int curHealth;
    private float staminaDrain;

    [Header("Player Collider")]
    private CircleCollider2D playerCollider;

    [Header("Damage Flicker Settings")]
    [SerializeField] private int flickerAmount = 3;
    [SerializeField] private float flickerDuration = 0.1f;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color staminaDrainColor;
    [SerializeField] private SpriteRenderer[] playerSprites;
    private bool staminaHurt;

    [Header("Spawner / Drain control")]
    [SerializeField] private Spawner spawner;

    public void Awake()
    {
        playerSprites = GetComponentsInChildren<SpriteRenderer>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        UpdateSpriteVisibility("Disable");
    }

    public void Update()
    {
        if (GameManager.instance.isPlaying && !GameManager.instance.gameIsEndless && spawner.spawnedFirstObstacle)
            StaminaDrain();
    }

    #region EnableDisable
    private void OnEnable()
    {
        Actions.OnPlayerHurt += TakeDamage;
        Actions.OnGameplay += UpdateHealthStats;
        Actions.ChangeSpriteVisibility += UpdateSpriteVisibility;
        Actions.ChangeEndlessVisibility += UpdateStaminaVisibility;
        Actions.ApplySettings += UpdateHealthStats;
        Actions.ResetStats += ResetStats;
    }

    private void OnDisable()
    {
        Actions.OnPlayerHurt -= TakeDamage;
        Actions.OnGameplay -= UpdateHealthStats;
        Actions.ChangeSpriteVisibility -= UpdateSpriteVisibility;
        Actions.ChangeEndlessVisibility -= UpdateStaminaVisibility;
        Actions.ApplySettings -= UpdateHealthStats;
        Actions.ResetStats -= ResetStats;
    }
    #endregion

    /// <summary>
    /// Drains stamina based on time, this shows visibly on the gameplay screen
    /// </summary>
    private void StaminaDrain()
    {
        float drainSpeedMultiplier = 0.1f;  // Adjust this value to control the drain rate

        if (staminaImage.fillAmount > 0)
        {
            staminaImage.fillAmount -= staminaDrain * drainSpeedMultiplier * Time.deltaTime;

            // If stamina hits zero, take damage but if the current health is greater than zero fill the stamina bar back to 1
            if (staminaImage.fillAmount <= 0)
            {
                staminaHurt = true;
                TakeDamage();
                if (curHealth > 0)
                    staminaImage.fillAmount = 1;
            }
        }
    }

    /// <summary>
    /// When player takes damage this will happen
    /// </summary>
    public void TakeDamage()
    {
        curHealth--;

        for (int i = curHealth; i < maxHealth; i++)
        {
            healthIcons[i].color = new Color(99, 99, 99);
        }

        if (staminaHurt)
        {
            Actions.OnPlaySFX("Stamina");
        }
        else
            Actions.OnPlaySFX("Obstacle");

        StartCoroutine(DamageFlicker());
    }


    // Updates health stats for new runs or when loading saved player stats
    private void UpdateHealthStats()
    {
        staminaImage.fillAmount = 1;
        curHealth = maxHealth;

        for (int i = 0; i < maxHealth + 1; i++)
        {
            healthIcons[i].fillAmount = 1;
        }

        //healthFillImage.fillAmount = (float)curHealth / (float)maxHealth;
    }

    // Updates Stamina visibility based on if the game is endless or not
    private void UpdateStaminaVisibility(string change)
    {
        foreach (GameObject staminaObj in stamina)
        {
            if (change == "Enable")
                staminaObj.SetActive(true);
            else
                staminaObj.SetActive(false);
        }
    }

    // Updates Sprite visibilty based on if the game is playing or not
    private void UpdateSpriteVisibility(string change)
    {
        foreach (SpriteRenderer sprite in playerSprites)
        {
            if (change == "Enable")
                sprite.enabled = true;
            else
                sprite.enabled = false;
        }
    }

    // This is a damage flicker for when the player is damaged, it also turns the collider off and on so if the player is dying they can't collect stuff at the same time.
    private IEnumerator DamageFlicker()
    {
        playerCollider.enabled = false;
        Color damageFlickerColor;

        if (staminaHurt)
        {
            damageFlickerColor = staminaDrainColor;
            staminaHurt = false;
        }
        else
            damageFlickerColor = damageColor;


        for (int i = 0; i < flickerAmount; i++)
        {
            foreach (SpriteRenderer sprite in playerSprites)
            {
                sprite.color = damageFlickerColor;
            }
            yield return new WaitForSecondsRealtime(flickerDuration);
            foreach (SpriteRenderer sprite in playerSprites)
            {
                sprite.color = Color.white;
            }
            yield return new WaitForSecondsRealtime(flickerDuration / 2);
        }
        if (curHealth <= 0)
            Actions.OnGameOver();

        playerCollider.enabled = true;
    }

    // Resets stats 
    public void ResetStats()
    {
        staminaDrain = baseStaminaDrain;
        maxHealth = baseMaxHealth;

        for (int i = 0; i < maxHealth; i++)
        {
            healthIcons[i].color = new Color(255, 255, 255);
        }

        playerSprites[0].sprite = baseHealthSprite;
        playerSprites[1].sprite = baseStaminaSprite;
    }


    #region GettersSetters
    public void SetUpgrade(UpgradeAsset upgrade, bool isHealth)
    {
        if (isHealth)
        {
            curHealthUpgrade = upgrade;
            SetMaxHealth((int)upgrade.newStats);
        }
        else
        {
            curStaminaUpgrade = upgrade;
            SetStaminaDrain(upgrade.newStats);
        }
    }

    private float GetStaminaDrain()
    { return staminaDrain; }

    private void SetStaminaDrain(float newDrain)
    { staminaDrain = newDrain; }

    private void SetMaxHealth(int health)
    {
        maxHealth = health;
    }
    #endregion
}