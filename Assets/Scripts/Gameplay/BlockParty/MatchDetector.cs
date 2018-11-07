using UnityEngine;
using System.Collections.Generic;

public class MatchDetection {
    public Block Block;

    public MatchDetection(Block block) {
        Block = block;
    }
}

public class MatchDetector : MonoBehaviour {
    List<MatchDetection> matchDetections;
    public BlockManager BlockManager;
    public const int MinimumMatchLength = 3;

    void Awake() {
        matchDetections = new List<MatchDetection>();
    }

    public void RequestMatchDetection(Block block) {
        matchDetections.Add(new MatchDetection(block));
    }

    void Update() {
        while(matchDetections.Count > 0) {
            MatchDetection detection = matchDetections[0];
            matchDetections.Remove(detection);

            if(detection.Block.State == BlockState.Idle) {
                DetectMatch(detection.Block);
            }
        }
    }

    void DetectMatch(Block block) {
        int left = block.Column;
        while(left > 0 && BlockManager.Blocks[left - 1, block.Row].State == BlockState.Idle && BlockManager.Blocks[left - 1, block.Row].Type == block.Type) {
            left--;
        }

        int right = block.Column + 1;
        while(right < BlockManager.Columns && BlockManager.Blocks[right, block.Row].State == BlockState.Idle && BlockManager.Blocks[right, block.Row].Type == block.Type) {
            right++;
        }

        int bottom = block.Row;
        while(bottom > 0 && BlockManager.Blocks[block.Column, bottom - 1].State == BlockState.Idle && BlockManager.Blocks[block.Column, bottom - 1].Type == block.Type) {
            bottom--;
        }

        int top = block.Row + 1;
        while(top < BlockManager.Rows - 1 && BlockManager.Blocks[block.Column, top].State == BlockState.Idle && BlockManager.Blocks[block.Column, top].Type == block.Type) {
            top++;
        }

        int width = right - left;
        int height = top - bottom;
        int matchedBlockCount = 0;
        bool horizontalMatch = false;
        bool verticalMatch = false;

        if(width >= MinimumMatchLength) {
            horizontalMatch = true;
            matchedBlockCount += width;
        }

        if(height >= MinimumMatchLength) {
            verticalMatch = true;
            matchedBlockCount += height;
        }

        if(!horizontalMatch && !verticalMatch) {
            return;
        }

        if(horizontalMatch && verticalMatch) {
            matchedBlockCount--;
        }

        int delayCounter = matchedBlockCount;

        if(horizontalMatch) {
            for(int matchColumn = left; matchColumn < right; matchColumn++) {
                BlockManager.Blocks[matchColumn, block.Row].Matcher.Match(matchedBlockCount, delayCounter--);
            }
        }

        if(verticalMatch) {
            for(int matchRow = top - 1; matchRow >= bottom; matchRow--) {
                BlockManager.Blocks[block.Column, matchRow].Matcher.Match(matchedBlockCount, delayCounter--);
            }
        }
    }
}