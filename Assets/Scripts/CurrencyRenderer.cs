using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyRenderer : MonoBehaviour {
	TMP_Text currencyText;

	void Awake() {
		currencyText = GetComponent<TMP_Text>();

		PlayerManager.Instance.PlayersChanged += HandlePlayersChanged;
	}

	void OnDestroy() {
		PlayerManager.Instance.PlayersChanged -= HandlePlayersChanged;
	}

	void HandlePlayersChanged(object sender, EventArgs args) {
		SetupCurrency();
	}

	void Start() {
		SetupCurrency();
	}

	void SetupCurrency() {
		if(AuthenticationManager.Instance.CurrentUser != null) {
			currencyText.text = PlayerManager.Instance.Players[AuthenticationManager.Instance.CurrentUser.UserId].currency + " BITS";
		}
	}
}
