using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;

public class TitleController : MonoBehaviour {
	Button playButton;

	void Awake() {
		playButton = GameObject.Find("Play Button").GetComponent<Button>();

		Analytics.Instance.LogAppOpenEvent();
		if(!SocketIO.Instance.IsConnected) {
			SocketIO.Instance.Connect();
		}
	}

	public void Play() {
		if(Authentication.Instance.CurrentUser != null) {
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

	void Update() {
		playButton.interactable = SocketIO.Instance.Socket.IsOpen;
	}
}
