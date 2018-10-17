﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignInController : MonoBehaviour {
	TMP_InputField emailInputField;
	TMP_InputField passwordInputField;

	void Awake() {
		emailInputField = GameObject.Find("Email Input Field").GetComponent<TMP_InputField>();
		passwordInputField = GameObject.Find("Password Input Field").GetComponent<TMP_InputField>();
	}
	public void Back() {
		SceneNavigator.Instance.Navigate("TitleScene");
	}

	public void SignIn() {
		Authentication.Instance.SignIn(emailInputField.text, passwordInputField.text, () => {
			SceneNavigator.Instance.Navigate("HomeScene");
		});
	}

	public void SignUp() {
		SceneNavigator.Instance.Navigate("SignUpScene");
	}
}