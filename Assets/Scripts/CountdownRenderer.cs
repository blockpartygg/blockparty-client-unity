﻿using UnityEngine;
using TMPro;
using System;

public class CountdownRenderer : MonoBehaviour {
	TMP_Text countdownText;

	void Awake() {
		countdownText = GetComponent<TMP_Text>();
	}

	void Update() {
		TimeSpan countdown = GameManager.Instance.EndTime > DateTime.Now ? GameManager.Instance.EndTime - DateTime.Now : TimeSpan.Zero;
		countdownText.text = string.Format("{0:D2}:{1:D2}", countdown.Minutes, countdown.Seconds);
	}
}
