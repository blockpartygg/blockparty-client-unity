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
    BoardRaiser boardRaiser;
    Vector3 garbageTranslation = Vector3.zero;
    Vector3 blockTranslation;

    void Awake() {
        particleManager = GameObject.Find("Minigame").GetComponent<ParticleManager>();
        boardRaiser = GameObject.Find("Minigame").GetComponent<BoardRaiser>();
    }

    void Start() {
        blockTranslation = new Vector3(Block.Column, Block.Row);

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
        Vector3 raiseTranslation = new Vector3(0, boardRaiser.Elapsed / BoardRaiser.Duration);
        transform.position = transform.parent.position + blockTranslation + raiseTranslation + garbageTranslation;
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
            SpriteRenderer.size = new Vector3((float)Garbage.Width, (float)Garbage.Height);
            garbageTranslation = new Vector3((Garbage.Width - 1) * 0.5f, (Garbage.Height - 1) * 0.5f);
        }
        else {
            SpriteRenderer.size = Vector2.one;
            garbageTranslation = Vector2.zero;
        }
    }

    void Update() {
        Vector3 raiseTranslation = new Vector3(0, boardRaiser.Elapsed / BoardRaiser.Duration);
        float timePercentage;

        switch(Block.State) {
            case BlockState.Idle:
                transform.position = transform.parent.position + blockTranslation + raiseTranslation + garbageTranslation;
                break;
            case BlockState.Sliding:
                float direction = Slider.Direction == SlideDirection.Left ? -1 : 1;
                timePercentage = Slider.Elapsed / BlockSlider.Duration;
                Vector3 slideTranslation = new Vector3(direction * timePercentage, 0);
                transform.position = transform.parent.position + blockTranslation + raiseTranslation + slideTranslation;
                break;
            case BlockState.Falling:
                timePercentage = Faller.Elapsed / BlockFaller.Duration;
                Vector3 fallTranslation = new Vector3(0, -1 * timePercentage);
                transform.position = transform.parent.position + blockTranslation + garbageTranslation + raiseTranslation + fallTranslation;
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