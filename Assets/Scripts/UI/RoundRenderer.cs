using UnityEngine;
using TMPro;

public class RoundRenderer : MonoBehaviour {
	TMP_Text roundText;

	void Awake() {
		roundText = GetComponent<TMP_Text>();
	}

	void Update() {
		roundText.text = "Round " + GameManager.Instance.Round.ToString();
	}
}
