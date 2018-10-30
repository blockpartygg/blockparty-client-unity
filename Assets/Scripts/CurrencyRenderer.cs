using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyRenderer : MonoBehaviour {
	TMP_Text currencyText;

	void Awake() {
		currencyText = GetComponent<TMP_Text>();
	}

	void Update() {
		if(AuthenticationManager.Instance.CurrentUser != null) {
			currencyText.text = PlayerManager.Instance.Players[AuthenticationManager.Instance.CurrentUser.UserId].currency + " BITS";
		}
	}
}
