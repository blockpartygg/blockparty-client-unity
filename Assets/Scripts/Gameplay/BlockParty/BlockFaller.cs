using UnityEngine;

public class BlockFaller : MonoBehaviour {
    public Block Block;
    float delayElapsed;
    const float delayDuration = 0.1f;
    public float Elapsed;
    public const float Duration = 0.1f;
    public Block Target;
    public bool JustFell;

    public void Fall() {
        Block.State = BlockState.WaitingToFall;
        delayElapsed = 0f;
    }

    public void ContinueFalling() {
        FinishWaitingToFall();
    }

    void FinishWaitingToFall() {
        Block.State = BlockState.Falling;
        Elapsed = 0f;
    }

    void Update() {
        if(Block.State == BlockState.WaitingToFall) {
            delayElapsed += Time.deltaTime;

            if(delayElapsed >= delayDuration) {
                FinishWaitingToFall();
            }
        }

        if(Block.State == BlockState.Falling) {
            Elapsed += Time.deltaTime;

            if(Elapsed >= Duration) {
                Target.Type = Block.Type;
                Target.State = BlockState.Falling;
                Target.Faller.JustFell = true;
                Target.Chainer.ChainEligible = Block.Chainer.ChainEligible;

                Block.State = BlockState.Empty;
                Block.Type = -1;
                // Block.Chainer.ChainEligible = false;
            }
        }
    }
}