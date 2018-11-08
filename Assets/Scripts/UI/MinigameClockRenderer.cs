using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameClockRenderer : MonoBehaviour {
	public TMP_Text ClockText;

	void Update() {
		switch(GameManager.Instance.State) {
			case GameManager.GameState.MinigameStart:
				ClockText.text = "30<sup>:00</sup>";
				break;
			case GameManager.GameState.MinigamePlay:
				TimeSpan timeRemaining = GameManager.Instance.EndTime > DateTime.Now ? GameManager.Instance.EndTime - DateTime.Now : TimeSpan.Zero;
				ClockText.text = string.Format("{0}:{1:D2}<sup>:{2:D3}</sup>", timeRemaining.Minutes, timeRemaining.Seconds, timeRemaining.Milliseconds);
				break;
			case GameManager.GameState.MinigameEnd:
				ClockText.text = "00<sup>:00</sup>";
				break;
		}
	}
}
