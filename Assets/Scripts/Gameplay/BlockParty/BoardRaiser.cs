using UnityEngine;

public class BoardRaiser : MonoBehaviour {
	public BlockPartyMinigameManager MinigameManager;
	public BlockManager BlockManager;
	public float Elapsed;
	public const float Duration = 10f;
	public MatchDetector MatchDetector;


	void Update() {
		if(MinigameManager.Mode == BlockPartyModes.Survival) {
			Elapsed += Time.deltaTime;

			if(Elapsed >= Duration) {
				Elapsed = 0f;

				for(int column = 0; column < BlockManager.Columns; column++) {
					for(int row = BlockManager.Rows - 2; row >= 1; row--) {
						BlockManager.Blocks[column, row].State = BlockManager.Blocks[column, row - 1].State;
						BlockManager.Blocks[column, row].Type = BlockManager.Blocks[column, row - 1].Type;
					}

					BlockManager.Blocks[column, 0].State = BlockManager.NewRowBlocks[column].State;
					BlockManager.Blocks[column, 0].Type = BlockManager.NewRowBlocks[column].Type;

					MatchDetector.RequestMatchDetection(BlockManager.Blocks[column, 0]);
				}

				BlockManager.CreateNewRowBlocks();
			}
		}
	}
}
