using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;

public class TitleController : MonoBehaviour {
	Button playButton;

	void Awake() {
		playButton = GameObject.Find("Play Button").GetComponent<Button>();

		AnalyticsManager.Instance.LogAppOpenEvent();
		
		if(!SocketManager.Instance.IsConnected) {
			SocketManager.Instance.Connected += HandleSocketConnected;
			SocketManager.Instance.Connect();
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

	void HandleSocketConnected(object sender, EventArgs args)
	{
		playButton.interactable = SocketManager.Instance.Socket.IsOpen;
	}

	public void SignUp() {
		SceneNavigator.Instance.Navigate("SignUpScene");
	}

	public void ViewLegalStatements() {
		Application.OpenURL("https://blockparty.gg/privacy-policy");
	}

	
}
