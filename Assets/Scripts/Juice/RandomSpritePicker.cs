using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpritePicker : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null || sprites.Count == 0) return;

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count - 1)];
    }
}
