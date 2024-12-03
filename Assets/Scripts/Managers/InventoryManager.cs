using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private TMP_Text energyTextAmnt;
    public int energyAmount;
    [SerializeField] private Button energyButton;
    private bool canUseEnergy;
    private bool usingEnergy = false;
    [SerializeField] private float energyDuration = 10f;

   [SerializeField] private CustomTimerSO energyTimer;

    private void Start()
    {
        //Reset();
    }

    private void OnEnable()
    {
        Actions.EnergyCollection += CollectEnergy;
        Actions.OnUseEnergy += UseEnergy;
        Actions.OnGameplay += Reset;
    }

    private void OnDisable()
    {
        Actions.EnergyCollection -= CollectEnergy;
        Actions.OnUseEnergy -= UseEnergy;
        Actions.OnGameplay -= Reset;
    }

    private void Update()
    {
        if (usingEnergy)
        {
            Time.timeScale = 2;

            energyButton.image.fillAmount = energyTimer.GetRemainingTime() / energyTimer.duration;

            if (energyTimer.UpdateTimer(Time.deltaTime))
            {
                usingEnergy = false;
                Time.timeScale = 1;
                energyButton.image.fillAmount = 1;

                if (energyAmount > 0)
                {
                    canUseEnergy = true;
                    energyButton.interactable = true;
                }
            }
        }

    }
    private void CollectEnergy()
    {
        energyAmount++;
        energyTextAmnt.text = $"x {energyAmount}";

        if (energyAmount > 0)
        {
            energyButton.interactable = true;
            canUseEnergy = true;
        }
    }

    public void UseEnergy()
    {
        if (usingEnergy || energyAmount == 0)
        {
            canUseEnergy = false;
            energyButton.interactable = false;
            return;
        }

        if (energyAmount > 0 && GameManager.instance.isPlaying)
        {

            usingEnergy = true;
            canUseEnergy = false;
            energyButton.interactable = false;

            energyAmount--;
            energyTextAmnt.text = $"x {energyAmount}";

            energyTimer.StartTimer(energyDuration);
            Debug.Log($"Timer Started: {energyTimer.isRunning}, Duration: {energyTimer.duration}");

            if (energyAmount == 0)
                energyButton.interactable = false;
        }

    }


    private void Reset()
    {
        energyAmount = 0;
        energyTextAmnt.text = $"x {energyAmount}";
        energyButton.interactable = false;
        energyTimer.ResetTimer();
        canUseEnergy = false;
        usingEnergy = false;
    }
}
