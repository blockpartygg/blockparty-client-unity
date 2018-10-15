using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class TitleController : MonoBehaviour {
	void Awake() {
		Analytics.Instance.LogAppOpenEvent();
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
}
