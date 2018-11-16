using UnityEngine;

public class BoardGravity : MonoBehaviour {
    public BlockManager BlockManager;
    public MatchDetector MatchDetector;
    public BlockPartyMinigameManager MinigameManager;
    public AudioSource AudioSource;
    public AudioClip LandClip;

    void Update() {
        // Traverse columns right-to-left so that when you encounter the leftmost garbage block, you can walk back right to see if the entire group can fall
        for(int column = BlockManager.Columns - 1; column >= 0; column--) {
            bool emptyBlockDetected = false;
            
            // Traverse from bottom to top to detect whether there are any empty blocks underneath the current one
            for(int row = 0; row < BlockManager.Rows; row++) {
                if(BlockManager.Blocks[column, row].State == BlockState.Empty) {
                    emptyBlockDetected = true;
                }

                // If the current block is idle and there's atleast one empty one underneath it...
                if(BlockManager.Blocks[column, row].State == BlockState.Idle && emptyBlockDetected) {
                    // For normal blocks, just make them fall immediately
                    if (BlockManager.Blocks[column, row].Type != 5) {
                        BlockManager.Blocks[column, row].Faller.Target = BlockManager.Blocks[column, row - 1];
                        BlockManager.Blocks[column, row].Faller.Fall();
                    }

                    // For garbage blocks, mark them as being able to fall
                    else {
                        BlockManager.Blocks[column, row].Garbage.CanFall = true;

                        // If this is the leftmost garbage block, go back through them to see if they can all fall
                        if(!BlockManager.Blocks[column, row].Garbage.IsNeighbor) {
                            bool shouldFall = true;

                            for(int garbageColumn = column; garbageColumn < column + BlockManager.Blocks[column, row].Garbage.Width; garbageColumn++) {
                                if(!BlockManager.Blocks[garbageColumn, row].Garbage.CanFall) {
                                    shouldFall = false;
                                }
                            }

                            // If all of the garbage blocks in a group can fall, make them fall together
                            if(shouldFall) {
                                for(int garbageColumn = column; garbageColumn < column + BlockManager.Blocks[column, row].Garbage.Width; garbageColumn++) {
                                    for(int garbageRow = row; garbageRow < row + BlockManager.Blocks[column, row].Garbage.Height; garbageRow++) {
                                        BlockManager.Blocks[garbageColumn, garbageRow].Faller.Target = BlockManager.Blocks[garbageColumn, garbageRow - 1];
                                        BlockManager.Blocks[garbageColumn, garbageRow].Faller.Fall();
                                    }
                                }
                            }
                        }
                    }
                }

                // If the current block just fell...
                if(BlockManager.Blocks[column, row].Faller.JustFell) {
                    // If the block underneath (assuming there is one) is empty or currently falling
                    if(row > 0 && (BlockManager.Blocks[column, row - 1].State == BlockState.Empty || BlockManager.Blocks[column, row - 1].State == BlockState.Falling)) {
                        // For normal blocks, make them continue falling immediately
                        if(BlockManager.Blocks[column, row].Type != 5) {
                            BlockManager.Blocks[column, row].Faller.Target = BlockManager.Blocks[column, row - 1];
                            BlockManager.Blocks[column, row].Faller.ContinueFalling();
                        }

                        // For garbage blocks, mark them as being able to continue falling
                        else {
                            BlockManager.Blocks[column, row].Garbage.CanContinueFalling = true;

                            // Similar pattern as above. If this is the leftmost garbage block, go back through to see if they can all continue falling
                            if(!BlockManager.Blocks[column, row].Garbage.IsNeighbor) {
                                bool shouldContinueFalling = true;

                                for(int garbageColumn = column; garbageColumn < column + BlockManager.Blocks[column, row].Garbage.Width; garbageColumn++) {
                                    if(!BlockManager.Blocks[garbageColumn, row].Garbage.CanContinueFalling) {
                                        shouldContinueFalling = false;
                                    }
                                }

                                // If all of the garbage blocks in a group can continue falling, make them continue falling together
                                if(shouldContinueFalling) {
                                    for(int garbageColumn = column; garbageColumn < column + BlockManager.Blocks[column, row].Garbage.Width; garbageColumn++) {
                                        for(int garbageRow = row; garbageRow < row + BlockManager.Blocks[column, row].Garbage.Height; garbageRow++) {
                                            BlockManager.Blocks[garbageColumn, garbageRow].Faller.Target = BlockManager.Blocks[garbageColumn, garbageRow - 1];
                                            BlockManager.Blocks[garbageColumn, garbageRow].Faller.ContinueFalling();
                                        }
                                    }
                                }

                                // Otherwise, make them all land together
                                else {
                                    for(int garbageColumn = column; garbageColumn < column + BlockManager.Blocks[column, row].Garbage.Width; garbageColumn++) {
                                        for(int garbageRow = row; garbageRow < row + BlockManager.Blocks[column, row].Garbage.Height; garbageRow++) {
                                            BlockManager.Blocks[garbageColumn, garbageRow].State = BlockState.Idle;
                                            AudioSource.clip = LandClip; // Todo: Make this a garbage block landing clip
                                            AudioSource.Play();
                                       }
                                    }
                                }
                            }
                        }
                    }
                    else {
                        BlockManager.Blocks[column, row].State = BlockState.Idle;
                        MatchDetector.RequestMatchDetection(BlockManager.Blocks[column, row]);
                        AudioSource.clip = LandClip;
                        AudioSource.Play();
                    }

                    BlockManager.Blocks[column, row].Faller.JustFell = false;
                }
            }
        }

        // In Time Attack mode, spawn in new blocks from the top when there's space to add them
        if(MinigameManager.Mode == BlockPartyModes.TimeAttack) {
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
}