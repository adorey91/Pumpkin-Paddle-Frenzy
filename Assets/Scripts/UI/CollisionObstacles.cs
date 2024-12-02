using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionObstacles : MonoBehaviour
{
    [Header("For Damage Holder")]
    [SerializeField] private Image[] collisionPlayer;
    [SerializeField] private Sprite[] collisionSprites;

    [SerializeField] private Animator playerAnimator;

    int spriteNumber = 0;
    Image collisionImage;

    private void Start()
    {
        spriteNumber = 0;

        collisionImage = GetComponent<Image>();
        collisionImage.sprite = collisionSprites[spriteNumber];
    }

    public void TakeDamage()
    {
        StartCoroutine(DamageFlicker());
    }

    public void StartPlayerAnimation(string direction)
    {
        playerAnimator.SetTrigger(direction);
        Debug.Log(direction);
    }

    public void ChangeSprite()
    {
        if (spriteNumber >= collisionSprites.Length - 1)
            spriteNumber = 0;
        else
            spriteNumber++;

        collisionImage.sprite = collisionSprites[spriteNumber];
    }

    private IEnumerator DamageFlicker()
    {
        for (int i = 0; i < 3; i++)
        {
            // Change player color to indicate damage
            foreach (var playerImage in collisionPlayer)
            {
                playerImage.color = Color.red;
            }
            yield return new WaitForSecondsRealtime(0.1f);

            // Revert player color
            foreach (var playerImage in collisionPlayer)
            {
                playerImage.color = Color.white;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // Small delay before applying health reduction
        yield return new WaitForSecondsRealtime(0.1f);
    }

}
