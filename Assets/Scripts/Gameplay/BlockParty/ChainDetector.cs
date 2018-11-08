using UnityEngine;

public class ChainDetector : MonoBehaviour {
	public BlockManager BlockManager;
	public Score Score;
	public int ChainLength;

	public void IncrementChain() {
		ChainLength++;
		Score.ScoreChain(ChainLength);
	}

	void Update() {
		bool stopChain = true;

		for(int column = 0; column < BlockManager.Columns; column++) {
			for(int row = 0; row < BlockManager.Rows; row++) {
				if(BlockManager.Blocks[column, row].Chainer.JustEmptied) {
					for(int chainEligibleRow = row + 1; chainEligibleRow < BlockManager.Rows; chainEligibleRow++) {
						if(BlockManager.Blocks[column, chainEligibleRow].State == BlockState.Idle ||
							BlockManager.Blocks[column, chainEligibleRow].State == BlockState.Sliding ||
							BlockManager.Blocks[column, chainEligibleRow].State == BlockState.WaitingToFall ||
							BlockManager.Blocks[column, chainEligibleRow].State == BlockState.Falling) {
							BlockManager.Blocks[column, chainEligibleRow].Chainer.ChainEligible = true;
							stopChain = false;
						}
					}
				}

				BlockManager.Blocks[column, row].Chainer.JustEmptied = false;
			}
		}

		for(int column = 0; column < BlockManager.Columns; column++) {
			for(int row = 0; row < BlockManager.Rows; row++) {
				if(BlockManager.Blocks[column, row].State != BlockState.Idle &&
					BlockManager.Blocks[column, row].State != BlockState.Empty &&
					BlockManager.Blocks[column, row].State != BlockState.Sliding) {
					stopChain = false;
				}
			}
		}

		if(stopChain) {
			for(int column = 0; column < BlockManager.Columns; column++) {
				for(int row = 0; row < BlockManager.Rows; row++) {
					BlockManager.Blocks[column, row].Chainer.ChainEligible = false;
				}
			}

			if(ChainLength > 1) {
				// Todo: play fanfare
			}

			ChainLength = 1;
		}
	}
}