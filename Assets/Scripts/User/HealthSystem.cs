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
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer boatSprite;
    [SerializeField] private SpriteRenderer paddleSprite;

    public int flickerAmount = 3;
    public float flickerDuration = 0.1f;

    public void Start()
    {
        GameManager.instance.onPlay.AddListener(UpdateHealthStats);
    }

    public void Update()
    {
        if (GameManager.instance.isPlaying)
            StaminaDrain();
    }

    private void StaminaDrain()
    {
        float drainSpeedMultiplier = 0.1f;  // Adjust this value to control the drain rate

        if (staminaImage.fillAmount > 0)
        {
            staminaImage.fillAmount -= staminaDrain * drainSpeedMultiplier * Time.deltaTime;

            if (staminaImage.fillAmount <= 0)  // Changed from `==` to `<=` for precision
            {
                TakeDamage();
                if (curHealth > 0)
                    staminaImage.fillAmount = 1;
            }
        }
    }


    public void TakeDamage()
    {
        curHealth--;
        healthText.text = $"x {curHealth}";

        StartCoroutine(DamageFlicker());

    }

    public void UpdateHealthStats()
    {
        staminaImage.fillAmount = 1;
        curHealth = maxHealth;
        healthText.text = $"x {curHealth}";
    }

    private IEnumerator DamageFlicker()
    {
        for (int i = 0; i < flickerAmount; i++)
        {
           
            playerSprite.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(flickerDuration);
            playerSprite.color = Color.white;
            yield return new WaitForSeconds(flickerDuration/2);
        }
        if (curHealth <= 0)
            GameManager.instance.LoadState("Upgrades");
    }
}
