using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    private float backgroundSpeed;
    private Renderer backgroundRenderer;

    [SerializeField] private Spawner spawner;

    public void Start()
    {
        backgroundRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(GameManager.instance.isPlaying)
        {
            backgroundSpeed = spawner.backgroundSpeed;
            backgroundRenderer.material.mainTextureOffset += new Vector2(0, backgroundSpeed * Time.deltaTime);
        }
    }
}
