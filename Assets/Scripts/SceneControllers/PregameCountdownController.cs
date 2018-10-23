using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameCountdownController : MonoBehaviour {
	GameObject precountdown;
	GameObject countdown;

	void Awake() {
		precountdown = GameObject.Find("Pre-Countdown");
		countdown = GameObject.Find("Countdown Scroll View");
	}

	public void Back() {
		SceneNavigator.Instance.Navigate("HomeScene");
	}

	void Update() {
		TimeSpan timeRemaining = GameManager.Instance.EndTime > DateTime.Now ? GameManager.Instance.EndTime - DateTime.Now : TimeSpan.Zero;
		Debug.Log(timeRemaining.TotalSeconds > 30);
		precountdown.SetActive(timeRemaining.TotalSeconds > 30);
		countdown.SetActive(timeRemaining.TotalSeconds <= 30);
	}
}
