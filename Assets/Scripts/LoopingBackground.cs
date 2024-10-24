using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    private float baseSpeed = 0.3f;
    private float speed;
    private Renderer backgroundRenderer;

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
            backgroundRenderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
        }
    }

    private void IncreaseSpeed(float timeAlive)
    {
        speed = baseSpeed * Mathf.Pow(timeAlive, 0.2f);
    }

    private void ResetSpeed()
    {
        speed = baseSpeed;
    }
}
