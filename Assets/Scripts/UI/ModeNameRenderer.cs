using UnityEngine;
using TMPro;

public class ModeNameRenderer : MonoBehaviour {
	TMP_Text modeNameText;

	void Awake() {
		modeNameText = GetComponent<TMP_Text>();
	}

	void Update() {
		modeNameText.text = GameManager.Instance.Mode.Name;
	}
}
