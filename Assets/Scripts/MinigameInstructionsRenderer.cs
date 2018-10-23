using UnityEngine;
using TMPro;

public class MinigameInstructionsRenderer : MonoBehaviour {
	TMP_Text minigameInstructionsText;

	void Awake() {
		minigameInstructionsText = GetComponent<TMP_Text>();
	}

	void Update() {
		minigameInstructionsText.text = GameManager.Instance.Minigame.Instructions;
	}
}
