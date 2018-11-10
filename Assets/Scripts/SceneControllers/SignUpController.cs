using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignUpController : MonoBehaviour {
	public TMP_InputField nameInputField;
	public TMP_InputField emailInputField;
	public TMP_InputField passwordInputField;
	public TMP_InputField passwordConfirmationInputField;
	public TMP_Text ErrorText;
	public Button SignUpButton;

	void Start() {
		UpdateSignUpButtonState();
	}

	public void GoBack() {
		SceneNavigator.Instance.Navigate("TitleScene");
	}

	public void UpdateSignUpButtonState() {
		SignUpButton.interactable = !string.IsNullOrEmpty(nameInputField.text) && 
			!string.IsNullOrEmpty(emailInputField.text) && 
			!string.IsNullOrEmpty(passwordInputField.text) && 
			passwordInputField.text == passwordConfirmationInputField.text;
	}

	public void SignUp() {
		if(passwordInputField.text == passwordConfirmationInputField.text) {
			AuthenticationManager.Instance.SignUp(emailInputField.text, passwordInputField.text, nameInputField.text, () => {
				SceneNavigator.Instance.Navigate("HomeScene");
			}, errorMessage => {
				ErrorText.text = errorMessage;
			});
		}
		else {
			ErrorText.text = "Passwords don't match.";
		}
	}

	public void SignIn() {
		SceneNavigator.Instance.Navigate("SignInScene");
	}
}
