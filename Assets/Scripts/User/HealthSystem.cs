using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private Image staminaImage;
    [SerializeField] private TMP_Text healthText;

    [Header("Upgrades")]
    public UpgradeAsset curHealthUpgrade;
    public UpgradeAsset curStaminaUpgrade;

    [Header("Amount")]
    public int maxHealth = 1;
    private int curHealth;
    public float staminaDrain = 0.5f;

    public void Start()
    {

        ResetHealthStats();
    }

    public void Update()
    {
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

        // might want to put in something to show they got hurt?

        if (curHealth <= 0)
        {
            PlayerController.instance.ActiveSprite(false);
            GameManager.instance.LoadState("Upgrades");
        }
    }

    public void ResetHealthStats()
    {
        staminaImage.fillAmount = 1;
        curHealth = maxHealth;
        healthText.text = $"x {curHealth}";
    }
}
