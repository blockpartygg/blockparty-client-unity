using UnityEngine;
using TMPro;

public class MinigameNameRenderer : MonoBehaviour {
	TMP_Text minigameNameText;

	void Awake() {
		minigameNameText = GetComponent<TMP_Text>();
	}

	void Update() {
		minigameNameText.text = GameManager.Instance.Minigame.Name;
	}
}
