using UnityEngine;

public enum SlideDirection {
    Left,
    Right,
    None
}

public class BlockSlider : MonoBehaviour {   
    public Block Block;
    public SlideDirection Direction;
    public BlockState TargetState;
    public int TargetType;
    public float Elapsed;
    public const float Duration = 0.1f;
    MatchDetector matchDetector;

    void Awake() {
        matchDetector = GameObject.Find("Minigame").GetComponent<MatchDetector>();
    }

    public void Slide(SlideDirection direction) {
        Block.State = BlockState.Sliding;
        Elapsed = 0f;
        Direction = direction;
    }

    void Update() {
        if(Block.State == BlockState.Sliding) {
            Elapsed += Time.deltaTime;

            if(Elapsed >= Duration) {
                Block.State = TargetState;
                Block.Type = TargetType;
                Direction = SlideDirection.None;

                if(Block.State == BlockState.Idle) {
                    matchDetector.RequestMatchDetection(Block);
                }
            }
        }
    }
}