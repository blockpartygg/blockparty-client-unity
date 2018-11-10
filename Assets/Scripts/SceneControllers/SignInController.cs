using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignInController : MonoBehaviour {
	public TMP_InputField emailInputField;
	public TMP_InputField passwordInputField;
	public TMP_Text ErrorText;
	public Button SignInButton;

	void Start() {
		UpdateSignInButtonState();
	}

	public void GoBack() {
		SceneNavigator.Instance.Navigate("TitleScene");
	}

	public void UpdateSignInButtonState() {
		SignInButton.interactable = !string.IsNullOrEmpty(emailInputField.text) && !string.IsNullOrEmpty(passwordInputField.text);
	}

	public void SignIn() {
		AuthenticationManager.Instance.SignIn(emailInputField.text, passwordInputField.text, () => {
			SceneNavigator.Instance.Navigate("HomeScene");
		}, errorMessage => {
			ErrorText.text = errorMessage;
		});
	}

	public void SignUp() {
		SceneNavigator.Instance.Navigate("SignUpScene");
	}
}
