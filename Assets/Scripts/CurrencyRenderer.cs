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
		currencyText.text = PlayerManager.Instance.Players[Authentication.Instance.CurrentUser.UserId].currency + " BITS";
	}
}
