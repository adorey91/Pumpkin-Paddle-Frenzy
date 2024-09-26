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
    public static int maxHealth = 1;
    private int curHealth;
    public static float staminaDrain = 0.45f;

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
                TakeDamage();
        }
    }


    public void TakeDamage()
    {
        curHealth--;
        healthText.text = $"x {curHealth}";

        // might want to put in something to show they got hurt?

        if (curHealth <= 0)
            GameManager.instance.LoadState("Upgrades");
    }

    public void ResetHealthStats()
    {
        staminaImage.fillAmount = 1;
        curHealth = maxHealth;
        healthText.text = $"x {curHealth}";
    }
}
