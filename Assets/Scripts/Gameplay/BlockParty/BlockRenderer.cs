using UnityEngine;
using System;
using System.Collections.Generic;

public class BlockRenderer: MonoBehaviour {
    public Block Block;
    public BlockSlider Slider;
    public BlockFaller Faller;
    public BlockMatcher Matcher;
    public BlockClearer Clearer;
    public SpriteRenderer SpriteRenderer;
    public ParticleManager ParticleManager;
    public List<Sprite> Sprites;
    public List<Sprite> MatchedSprites;
    public List<Sprite> ClearingSprites;

    void Awake() {
        ParticleManager = GameObject.Find("Minigame").GetComponent<ParticleManager>();
    }

    void Start() {
        UpdatePosition();
        UpdateSpriteState();
        UpdateSpriteType();
        Block.StateChanged += HandleStateChanged;
        Block.TypeChanged += HandleTypeChanged;
    }

    void HandleStateChanged(object sender, EventArgs args) {
        UpdatePosition();
        UpdateSpriteState();
    }

    void HandleTypeChanged(object sender, EventArgs args) {
        UpdateSpriteType();
    }

    void UpdatePosition() {
        transform.position = transform.parent.position + new Vector3(Block.Column, Block.Row, 0f);
    }

    void UpdateSpriteState() {
        switch(Block.State) {    
            case BlockState.Matched:
                SpriteRenderer.sprite = MatchedSprites[Block.Type];
                break;
            case BlockState.WaitingToClear:
                SpriteRenderer.sprite = ClearingSprites[Block.Type];
                break;
            case BlockState.Clearing:
                ParticleManager.Particles[Block.Column, Block.Row].GetComponent<ParticleSystem>().Play();
                break;
            case BlockState.WaitingToEmpty:
            case BlockState.Empty:
                SpriteRenderer.enabled = false;
                break;
            default:
                SpriteRenderer.enabled = true;
                SpriteRenderer.transform.localScale = Vector3.one;
                SpriteRenderer.color = Color.white;
                break;
        }
    }

    void UpdateSpriteType() {
        if(Block.Type != -1) {
            SpriteRenderer.sprite = Sprites[Block.Type];
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
                transform.position = transform.parent.position + Vector3.Lerp(new Vector3(Block.Column, Block.Row, 0f), new Vector3(Block.Column, Block.Row - transform.localScale.y, 0f), timePercentage);
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