using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay_UI : MonoBehaviour
{
    [Header("For Stamina Holder")]
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image[] healthIcons;
    [SerializeField] private Image[] staminaPlayer;
    [SerializeField] private Sprite[] boats;
    [SerializeField] private Sprite[] paddles;
    [SerializeField] private float[] staminaDrain;
    [SerializeField] private int[] healthAmount;
    private int maxHealth;
    private int currentHealth;
    [SerializeField] private Color drainColor;
    bool startDrain = false;
    bool changeSprites;
    private bool staminaDrained = false;
    private int upgradeIndex = 0;


    public void InitializeDefaults()
    {
        // Set initial player sprites
        upgradeIndex = 0;
        staminaPlayer[0].sprite = boats[upgradeIndex];
        staminaPlayer[1].sprite = paddles[upgradeIndex];

        // Reset stamina and state
        staminaBar.fillAmount = 1;
        maxHealth = healthAmount[upgradeIndex];
        currentHealth = maxHealth;

        for (int i = 0; i < maxHealth; i++)
        {
            healthIcons[i].color = new Color32(255, 255, 255, 255);
        }

        //startDrain = true;
        staminaDrained = false;
    }

    void Update()
    {
        if (startDrain)
        {
            // Drain stamina over time
            staminaBar.fillAmount -= staminaDrain[upgradeIndex] * Time.unscaledDeltaTime;
        }

        if (staminaBar.fillAmount <= 0 && !staminaDrained)
        {
            staminaDrained = true;
            startDrain = false;

            // Reduce health
            currentHealth--;

            for (int i = currentHealth; i < maxHealth; i++)
            {
                healthIcons[i].color = new Color32(120, 120, 120, 255);
            }

            StartCoroutine(HandleStaminaDepletion());
        }
    }

    public void StartDrain() => startDrain = true;
    public void StopDrain() => startDrain = false;

    private IEnumerator HandleStaminaDepletion()
    {
        // Trigger flicker effect to indicate damage
        yield return StartCoroutine(DrainFlicker());


        // Wait before resetting stamina
        yield return new WaitForSecondsRealtime(0.75f);

        // Check if health is depleted
        if (currentHealth <= 0)
        {
            // Reset player sprites and health
            changeSprites = true;
        }

        // Only update sprites and reset health if health reaches 0
        if (changeSprites)
        {
            if (upgradeIndex >= boats.Length - 1)
                upgradeIndex = 0;
            else
                upgradeIndex++;

            staminaPlayer[0].sprite = boats[upgradeIndex];
            staminaPlayer[1].sprite = paddles[upgradeIndex];


            maxHealth = healthAmount[upgradeIndex];

            for (int i = 0; i < maxHealth; i++)
            {
                healthIcons[i].color = new Color32(255, 255, 255, 255);
            }

            currentHealth = maxHealth;
            changeSprites = false;
        }

        // Reset stamina
        staminaBar.fillAmount = 1;
        staminaDrained = false;
        startDrain = true;
    }

    private IEnumerator DrainFlicker()
    {
        for (int i = 0; i < 3; i++)
        {
            // Change player color to indicate damage
            foreach (var playerImage in staminaPlayer)
            {
                playerImage.color = drainColor;
            }
            yield return new WaitForSecondsRealtime(0.1f);

            // Revert player color
            foreach (var playerImage in staminaPlayer)
            {
                playerImage.color = Color.white;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // Small delay before applying health reduction
        yield return new WaitForSecondsRealtime(0.1f);
    }
}