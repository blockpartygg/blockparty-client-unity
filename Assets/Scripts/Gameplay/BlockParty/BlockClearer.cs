using UnityEngine;

public class BlockClearer : MonoBehaviour {
    public Block Block;
    public BlockEmptier Emptier;
    public Score Score;
    float delayElapsed;
    public const float DelayInterval = 0.25f;
    public float DelayDuration;
    public float Elapsed;
    public const float Duration = 0.25f;

    void Awake() {
        Score = GameObject.Find("Minigame").GetComponent<Score>();
    }

    public void Clear() {
        Block.State = BlockState.WaitingToClear;
        delayElapsed = 0f;
    }

    void Update() {
        if(Block.State == BlockState.WaitingToClear) {
            delayElapsed += Time.deltaTime;
            
            if(delayElapsed >= DelayDuration) {
                Block.State = BlockState.Clearing;
                Elapsed = 0f;

                Score.ScoreMatch();
            }
        }

        if(Block.State == BlockState.Clearing) {
            Elapsed += Time.deltaTime;

            if(Elapsed >= Duration) {
                Emptier.Empty();
            }
        }
    }
}