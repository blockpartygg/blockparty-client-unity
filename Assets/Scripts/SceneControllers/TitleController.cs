using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using TMPro;

public class TitleController : MonoBehaviour {
	public Button playButton;
	public TMP_Text playButtonText;

	void Awake() {
		AnalyticsManager.Instance.LogAppOpenEvent();
		
		SocketManager.Instance.Connected += HandleSocketConnected;

		if(!SocketManager.Instance.IsConnected) {
			SocketManager.Instance.Connect();
		}
	}

	void HandleSocketConnected(object sender, EventArgs args)
	{
		UpdatePlayButton();
	}

	void Start() {
		UpdatePlayButton();
	}

	void UpdatePlayButton() {
		if(SocketManager.Instance.IsConnected) {
			playButton.interactable = true;
			playButtonText.text = "Join the Party!";
		}
		else {
			playButton.interactable = false;
			playButtonText.text = "Connecting...";
		}
	}

	void OnDestroy() {
		SocketManager.Instance.Connected -= HandleSocketConnected;
	}

	public void Play() {
		if(AuthenticationManager.Instance.CurrentUser != null) {
			SceneNavigator.Instance.Navigate("HomeScene");
		}
		else {
			SceneNavigator.Instance.Navigate("SignInScene");
		}
	}

	public void SignUp() {
		SceneNavigator.Instance.Navigate("SignUpScene");
	}

	public void ViewLegalStatements() {
		Application.OpenURL("https://blockparty.gg/privacy-policy");
	}

	
}
