using UnityEngine;

public class BlockMatcher : MonoBehaviour {
    public Block Block;
    public BlockClearer Clearer;
    public BlockEmptier Emptier;
    public float Elapsed;
    const float duration = 1f;

    public void Match(int matchedBlockCount, int delayCounter) {
        Block.State = BlockState.Matched;
        Elapsed = 0f;
        Clearer.DelayDuration = (matchedBlockCount - delayCounter) * BlockClearer.DelayInterval;
        Emptier.DelayDuration = delayCounter * BlockEmptier.DelayInterval;
    }

    void Update() {
        if(Block.State == BlockState.Matched) {
            Elapsed += Time.deltaTime;

            if(Elapsed >= duration) {
                Clearer.Clear();
            }
        }
    }
}