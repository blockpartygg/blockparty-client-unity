using UnityEngine;
using System;
using System.Collections.Generic;

public class BlockRenderer: MonoBehaviour {
    public Block Block;
    public BlockGarbage Garbage;
    public BlockSlider Slider;
    public BlockFaller Faller;
    public BlockMatcher Matcher;
    public BlockClearer Clearer;
    public SpriteRenderer SpriteRenderer;
    public List<Sprite> Sprites;
    public List<Sprite> MatchedSprites;
    public List<Sprite> ClearingSprites;

    ParticleManager particleManager;
    Vector3 garbageTranslation = Vector3.zero;

    void Awake() {
        particleManager = GameObject.Find("Minigame").GetComponent<ParticleManager>();
    }

    void Start() {
        UpdateSpriteState();
        UpdateSpriteType();
        UpdatePosition();
        Block.StateChanged += HandleStateChanged;
        Block.TypeChanged += HandleTypeChanged;
    }

    void HandleStateChanged(object sender, EventArgs args) {
        UpdateSpriteState();
        UpdatePosition();
    }

    void HandleTypeChanged(object sender, EventArgs args) {
        UpdateSpriteType();
        UpdatePosition();
    }

    void UpdatePosition() {
        transform.position = transform.parent.position + garbageTranslation + new Vector3(Block.Column, Block.Row, 0f);
    }

    void UpdateSpriteState() {
        switch(Block.State) {    
            case BlockState.Matched:
                if(Block.Type != -1) {
                    SpriteRenderer.sprite = MatchedSprites[Block.Type];
                }
                break;
            case BlockState.WaitingToClear:
                if(Block.Type != -1) {
                    SpriteRenderer.sprite = ClearingSprites[Block.Type];
                }
                break;
            case BlockState.Clearing:
                particleManager.Particles[Block.Column, Block.Row].GetComponent<ParticleSystem>().Play();
                break;
            case BlockState.WaitingToEmpty:
            case BlockState.Empty:
                break;
            default:
                SpriteRenderer.transform.localScale = Vector3.one;
                SpriteRenderer.color = Color.white;
                break;
        }
    }

    void UpdateSpriteType() {
        if(Block.Type == -1 || (Block.Type == 5 && Block.Garbage.IsNeighbor)) {
            SpriteRenderer.sprite = null;
        }
        else {
            SpriteRenderer.sprite = Sprites[Block.Type];
        }

        if(Block.Type == 5) {
            SpriteRenderer.size = new Vector2((float)Garbage.Width, (float)Garbage.Height);
            garbageTranslation = new Vector3((Garbage.Width - 1) * 0.5f, (Garbage.Height - 1) * 0.5f, 0f);
        }
        else {
            SpriteRenderer.size = Vector2.one;
            garbageTranslation = Vector3.zero;
        }
    }

    void Update() {
        float timePercentage = 0f;
        switch(Block.State) {
            case BlockState.Sliding:
                float distance = 0f;
                distance = Slider.Direction == SlideDirection.Left ? -transform.localScale.x : transform.localScale.x;
                timePercentage = Slider.Elapsed / BlockSlider.Duration;
                transform.position = transform.parent.position + Vector3.Lerp(new Vector3(Block.Column, Block.Row, 0f), new Vector3(Block.Column + distance, Block.Row, 0f), timePercentage);
                break;
            case BlockState.Falling:
                timePercentage = Faller.Elapsed / BlockFaller.Duration;
                transform.position = transform.parent.position + garbageTranslation + Vector3.Lerp(new Vector3(Block.Column, Block.Row, 0f), new Vector3(Block.Column, Block.Row - transform.localScale.y, 0f), timePercentage);
                break;
            case BlockState.Matched:
                SpriteRenderer.sprite = Matcher.Elapsed % 0.1f < 0.05f ? MatchedSprites[Block.Type] : Sprites[Block.Type];
                break;
            case BlockState.Clearing:
                timePercentage = Clearer.Elapsed / BlockClearer.Duration;
                SpriteRenderer.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timePercentage);
                break;
        }
    }
}