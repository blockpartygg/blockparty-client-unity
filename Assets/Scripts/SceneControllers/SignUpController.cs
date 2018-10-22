using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignUpController : MonoBehaviour {
	TMP_InputField nameInputField;
	TMP_InputField emailInputField;
	TMP_InputField passwordInputField;
	TMP_InputField passwordConfirmationInputField;

	void Awake() {
		nameInputField = GameObject.Find("Name Input Field").GetComponent<TMP_InputField>();
		emailInputField = GameObject.Find("Email Input Field").GetComponent<TMP_InputField>();
		passwordInputField = GameObject.Find("Password Input Field").GetComponent<TMP_InputField>();
		passwordConfirmationInputField = GameObject.Find("Password Confirmation Input Field").GetComponent<TMP_InputField>();
	}

	public void GoBack() {
		SceneNavigator.Instance.Navigate("TitleScene");
	}

	public void SignUp() {
		if(passwordInputField.text == passwordConfirmationInputField.text) {
			Authentication.Instance.SignUp(emailInputField.text, passwordInputField.text, nameInputField.text, () => {
				SceneNavigator.Instance.Navigate("HomeScene");
			});
		}
	}

	public void SignIn() {
		SceneNavigator.Instance.Navigate("SignInScene");
	}
}
