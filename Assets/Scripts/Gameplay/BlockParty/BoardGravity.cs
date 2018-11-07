using UnityEngine;

public class BoardGravity : MonoBehaviour {
    public BlockManager BlockManager;
    public MatchDetector MatchDetector;

    void Update() {
        for(int column = 0; column < BlockManager.Columns; column++) {
            bool emptyBlockDetected = false;
            for(int row = 0; row < BlockManager.Rows; row++) {
                if(BlockManager.Blocks[column, row].State == BlockState.Empty) {
                    emptyBlockDetected = true;
                }

                if(BlockManager.Blocks[column, row].State == BlockState.Idle && emptyBlockDetected) {
                    BlockManager.Blocks[column, row].Faller.Target = BlockManager.Blocks[column, row - 1];
                    BlockManager.Blocks[column, row].Faller.Fall();
                }

                if(BlockManager.Blocks[column, row].Faller.JustFell) {
                    if(row > 0 && (BlockManager.Blocks[column, row - 1].State == BlockState.Empty || BlockManager.Blocks[column, row - 1].State == BlockState.Falling)) {
                        BlockManager.Blocks[column, row].Faller.Target = BlockManager.Blocks[column, row - 1];
                        BlockManager.Blocks[column, row].Faller.ContinueFalling();
                    }
                    else {
                        BlockManager.Blocks[column, row].State = BlockState.Idle;
                        MatchDetector.RequestMatchDetection(BlockManager.Blocks[column, row]);
                    }

                    BlockManager.Blocks[column, row].Faller.JustFell = false;
                }
            }
        }

        for(int column = 0; column < BlockManager.Columns; column++) {
            if(BlockManager.Blocks[column, BlockManager.Rows - 1].State == BlockState.Empty) {
                BlockManager.Blocks[column, BlockManager.Rows - 1].Type = BlockManager.GetRandomBlockType(column, BlockManager.Rows - 1);

                if(BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Idle) {
                    BlockManager.Blocks[column, BlockManager.Rows - 1].State = BlockState.Idle;
                }

                if(BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Empty || BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Falling) {
                    BlockManager.Blocks[column, BlockManager.Rows - 1].Faller.Target = BlockManager.Blocks[column, BlockManager.Rows - 2];
                    BlockManager.Blocks[column, BlockManager.Rows - 1].Faller.ContinueFalling();
                }
            }
        }
    }
}