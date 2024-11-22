using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    private float baseSpeed = 0.3f;
    private float currentSpeed;
    private float newSpeed;
    private Renderer backgroundRenderer;
    private float smoothSpeedTransitionTime = 1.0f; // Time it takes to reach the target currentSpeed
    private float speedLerpProgress = 0f; // Keeps track of the interpolation progress
    bool increasingSpeed = false; // Used to keep track of if the currentSpeed is increasing

    //[SerializeField] private Spawner spawner;

    public void Start()
    {
        backgroundRenderer = GetComponent<Renderer>();
    }

    public void OnEnable()
    {
        Actions.SpeedChange += IncreaseSpeed;
        Actions.OnGameplay += ResetSpeed;
    }

    public void OnDisable()
    {
        Actions.SpeedChange -= IncreaseSpeed;
        Actions.OnGameplay -= ResetSpeed;
    }

    private void Update()
    {
        if(GameManager.instance.isPlaying)
        {
            backgroundRenderer.material.mainTextureOffset += new Vector2(0, currentSpeed * Time.deltaTime);

            if(increasingSpeed)
                IncreasingGradualSpeed();
        }
    }

    private void IncreaseSpeed(float timeAlive)
    {
        newSpeed = baseSpeed * Mathf.Pow(timeAlive, 0.2f);

        increasingSpeed = true;
    }

    private void IncreasingGradualSpeed()
    {
        speedLerpProgress += Time.deltaTime / smoothSpeedTransitionTime;
        currentSpeed = Mathf.Lerp(currentSpeed, newSpeed, speedLerpProgress);

        if (currentSpeed == newSpeed)
            increasingSpeed = false;
    }

    private void ResetSpeed()
    {
        increasingSpeed = false;
        currentSpeed = baseSpeed;
    }
}
