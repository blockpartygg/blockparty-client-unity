using UnityEngine;
using TMPro;
using DG.Tweening;

public enum BlockPartyModes {
	TimeAttack,
	Survival
}

public class BlockPartyMinigameManager : MonoBehaviour {
	public BlockPartyModes Mode;
	public BoardController BoardController;
	public BlockManager BlockManager;
	public TMP_Text EliminatedText;

	public void EndGame() {
		BoardController.enabled = false;
		BlockManager.KillBlocks();
		EliminatedText.enabled = true;
		EliminatedText.transform.DOMoveY(1f, 1f);
		EliminatedText.DOFade(1f, 1f);
	}
}