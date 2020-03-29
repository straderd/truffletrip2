using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //Config parameters
    [SerializeField] GameObject blockVFX;

    [SerializeField] Sprite[] hitSprites;

    // Cached reference
    Level level;
    GameStatus gameStatus;
    [SerializeField] int hitCount; //only serialized for debug purposes

    // State variables

    private void Start()
    {
        TrackTotalBlocks();
    }

    private void TrackTotalBlocks()
    {
        HandleHit();
    }

    private void HandleHit()
    {
        level = FindObjectOfType<Level>();
        if (CompareTag("Block"))
        {
            level.CountBreakableBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompareTag("Block"))
        {
            hitCount++;
            int maxHits = hitSprites.Length + 1;
            if (hitCount >= maxHits)
            {
                DestroyBlock();
            }
            else
            {
                ShowNextHitSprite();
            }
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = hitCount - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
    }

    private void DestroyBlock()
    {
        gameStatus = FindObjectOfType<GameStatus>();
        gameStatus.AddToScore();
        Destroy(gameObject);
        TriggerBlockVFX();
        level.BlockDestroyed();
    }

    private void TriggerBlockVFX()
    {
        GameObject sparkle = Instantiate(blockVFX, transform.position, transform.rotation);
        Destroy(sparkle, 1f);
    }
}
