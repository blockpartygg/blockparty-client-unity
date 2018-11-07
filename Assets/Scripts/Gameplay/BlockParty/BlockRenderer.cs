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
    public List<Color> Colors;

    void Start() {
        UpdatePosition();
        UpdateSprite();
        UpdateSpriteType();
        Block.StateChanged += HandleStateChanged;
        Block.TypeChanged += HandleTypeChanged;
    }

    void HandleStateChanged(object sender, EventArgs args) {
        UpdatePosition();
        UpdateSprite();
    }

    void HandleTypeChanged(object sender, EventArgs args) {
        UpdateSpriteType();
    }

    void UpdatePosition() {
        transform.position = transform.parent.position + new Vector3(Block.Column, Block.Row, 0f);
    }

    void UpdateSprite() {
        switch(Block.State) {
            case BlockState.Empty:
            case BlockState.WaitingToEmpty:
                SpriteRenderer.enabled = false;
                break;
            default:
                SpriteRenderer.enabled = true;
                SpriteRenderer.transform.localScale = Vector3.one;
                break;
        }
    }

    void UpdateSpriteType() {
        if(Block.Type != -1) {
            SpriteRenderer.color = Colors[Block.Type];   
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
                SpriteRenderer.color = Matcher.Elapsed % 0.1f < 0.05f ? Color.white : Colors[Block.Type];
                break;
            case BlockState.Clearing:
                timePercentage = Clearer.Elapsed / BlockClearer.Duration;
                SpriteRenderer.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timePercentage);
                SpriteRenderer.color = Color.Lerp(Colors[Block.Type], new Color(Colors[Block.Type].r, Colors[Block.Type].g, Colors[Block.Type].b, 0f), timePercentage);
                break;
        }
    }
}