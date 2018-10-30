using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class PlayerBadgeRenderer : MonoBehaviour {
	public TMP_Text NameText;
	public TMP_Text CurrencyText;
	
	void Awake() {
		PlayerManager.Instance.PlayersChanged += HandlePlayersChanged;
	}

	void OnDestroy() {
		PlayerManager.Instance.PlayersChanged -= HandlePlayersChanged;
	}

	void Start() {
		SetupPlayerBadge();
	}

	void HandlePlayersChanged(object sender, EventArgs args) {
		SetupPlayerBadge();		
	}

	void SetupPlayerBadge() {
		if(AuthenticationManager.Instance.CurrentUser != null) {
			if(PlayerManager.Instance.Players.ContainsKey(AuthenticationManager.Instance.CurrentUser.UserId)) {
				Player currentPlayer = PlayerManager.Instance.Players[AuthenticationManager.Instance.CurrentUser.UserId];
				NameText.text = currentPlayer.name;
				CurrencyText.text = currentPlayer.currency + " BITS";
			}
		}
	}
}
