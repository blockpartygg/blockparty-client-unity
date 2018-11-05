using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownScrollViewController : MonoBehaviour {
	Image countdownDot;
	Image rulesDot;
	public Sprite SelectedDot;
	public Sprite DeselectedDot;

	void Awake() {
		countdownDot = GameObject.Find("Countdown Dot").GetComponent<Image>();
		countdownDot.sprite = SelectedDot;

		rulesDot = GameObject.Find("Rules Dot").GetComponent<Image>();
		rulesDot.sprite = DeselectedDot;
	}

	public void HandleValueChanged(Vector2 value) {
		countdownDot.sprite = value.x == 0.0f ? SelectedDot : DeselectedDot;
		rulesDot.sprite = value.x == 1.0f ? SelectedDot : DeselectedDot;
	}
}
