using UnityEngine;
using TMPro;
using System;

public class CountdownRenderer : MonoBehaviour {
	TMP_Text countdownText;
	float updateElapsed;
	const float updateDuration = 1f;

	void Awake() {
		countdownText = GetComponent<TMP_Text>();
	}

	void Update() {
		// Only update once per second
		updateElapsed += Time.deltaTime;
		if(updateElapsed >= updateDuration) {
			updateElapsed = 0f;
			TimeSpan countdown = GameManager.Instance.EndTime > DateTime.Now ? GameManager.Instance.EndTime - DateTime.Now : TimeSpan.Zero;
			countdownText.text = string.Format("{0:D2}:{1:D2}", countdown.Minutes, countdown.Seconds);
		}
	}
}
