using UnityEngine;

public class BlockEmptier : MonoBehaviour {
    public Block Block;
    float delayElapsed;
    public const float DelayInterval = 0.25f;
    public float DelayDuration;

    public void Empty() {
        Block.State = BlockState.WaitingToEmpty;
        delayElapsed = 0f;
    }

    void Update() {
        if(Block.State == BlockState.WaitingToEmpty) {
            delayElapsed += Time.deltaTime;

            if(delayElapsed >= DelayDuration) {
                Block.State = BlockState.Empty;
                Block.Type = -1;
                Block.Chainer.JustEmptied = true;
            }
        }
    }
}