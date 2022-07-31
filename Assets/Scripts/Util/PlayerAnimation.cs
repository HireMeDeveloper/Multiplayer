using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int currentIndex;
    [Space]
    [SerializeField] private List<Sprite> idleSprites;
    [SerializeField] private List<Sprite> walkSprites;
    [SerializeField] private List<Sprite> rollSprites;

    public void setStartIdleSprite()
    {
        currentIndex = 0;
        spriteRenderer.sprite = idleSprites[currentIndex];
    }

    public void setStartWalkSprite()
    {
        currentIndex = 0;
        spriteRenderer.sprite = walkSprites[currentIndex];
    }

    public void nextWalkSprite()
    {
        if (walkSprites.Count == 0) return;
        currentIndex = (currentIndex < walkSprites.Count - 1) ? currentIndex + 1 : 0;
        spriteRenderer.sprite = walkSprites[currentIndex];
    }

    public void setStartRollSprite()
    {
        currentIndex = 0;
        spriteRenderer.sprite = rollSprites[currentIndex];
    }

    public void nextRollSprite()
    {
        if (rollSprites.Count == 0) return;
        currentIndex = (currentIndex < rollSprites.Count - 1) ? currentIndex + 1 : 0;
        spriteRenderer.sprite = rollSprites[currentIndex];
    }
}
