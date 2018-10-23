using UnityEngine;
using TMPro;

public class ModeInstructionsRenderer : MonoBehaviour {
	TMP_Text modeInstructionsText;

	void Awake() {
		modeInstructionsText = GetComponent<TMP_Text>();
	}

	void Update() {
		modeInstructionsText.text = GameManager.Instance.Mode.Instructions;
	}
}
