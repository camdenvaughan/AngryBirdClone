using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateRandomizer : MonoBehaviour
{
    [SerializeField] List<Sprite> boxSprites = new List<Sprite>();

    SpriteRenderer _spriteRenderer;

    // Cache Sprite Renderer Component
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create Random Variables
        int spriteRandomizer = Random.Range(0, 2);
        int flipRandomizer = Random.Range(0, 3);

        // Set Sprite to one of the two sprites
        _spriteRenderer.sprite = boxSprites[spriteRandomizer];

        // Randomly flip the x or y
        if (flipRandomizer > 1) _spriteRenderer.flipX = true;
        if (flipRandomizer < 1) _spriteRenderer.flipY = true;

    }
}
