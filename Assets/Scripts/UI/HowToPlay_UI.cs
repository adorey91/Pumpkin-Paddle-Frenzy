using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay_UI : MonoBehaviour
{
    [Header("For Stamina Holder")]
    [SerializeField] private Image staminaBar;
    [SerializeField] private Image health;
    [SerializeField] private Image[] staminaPlayer;
    [SerializeField] private Sprite[] boats;
    [SerializeField] private Sprite[] paddles;
    [SerializeField] private float[] staminaDrain;
    [SerializeField] private float[] healthAmount;
    private float maxHealth;
    private float currentHealth;
    [SerializeField] private Color drainColor;
    bool startDrain = false;
    private bool staminaDrained = false;
    private int upgradeIndex = 0;

    public void Start()
    {
        InitializeDefaults();
    }

    private void InitializeDefaults()
    {
        // Set initial player sprites
        upgradeIndex = 0;
        staminaPlayer[0].sprite = boats[upgradeIndex];
        staminaPlayer[1].sprite = paddles[upgradeIndex];

        // Reset stamina and state
        staminaBar.fillAmount = 1;
        maxHealth = healthAmount[upgradeIndex];
        currentHealth = maxHealth;
        health.fillAmount = 1;
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
            StartCoroutine(HandleStaminaDepletion());
        }
    }

    public void StartDrain() => startDrain = true;

    private IEnumerator HandleStaminaDepletion()
    {
        // Trigger flicker effect to indicate damage
        yield return StartCoroutine(DrainFlicker());

        // Reduce health
        currentHealth--;
        health.fillAmount = currentHealth/ maxHealth;

        // Wait before resetting stamina
        yield return new WaitForSecondsRealtime(0.75f);

        // Only update sprites and reset health if health reaches 0
        if (health.fillAmount <= 0)
        {
            if (upgradeIndex >= boats.Length - 1)
                upgradeIndex = 0;
            else
                upgradeIndex++;

            staminaPlayer[0].sprite = boats[upgradeIndex];
            staminaPlayer[1].sprite = paddles[upgradeIndex];

            maxHealth = healthAmount[upgradeIndex];
            currentHealth = maxHealth;
            health.fillAmount = 1;
        }

        // Reset stamina
        staminaBar.fillAmount = 1;
        staminaDrained = false;
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