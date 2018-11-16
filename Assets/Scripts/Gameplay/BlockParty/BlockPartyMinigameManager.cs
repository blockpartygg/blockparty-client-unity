using UnityEngine;

public enum BlockPartyModes {
	TimeAttack,
	Survival
}

public class BlockPartyMinigameManager : MonoBehaviour {
	public BlockPartyModes Mode;
	public BoardController BoardController;
	public BlockManager BlockManager;

	public void EndGame() {
		BoardController.enabled = false;
		BlockManager.KillBlocks();
	}
}