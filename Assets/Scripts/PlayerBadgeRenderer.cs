using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class PlayerBadgeRenderer : MonoBehaviour {
	public TMP_Text NameText;
	public TMP_Text CurrencyText;
	
	// Update is called once per frame
	void Update () {
		if(Authentication.Instance.CurrentUser != null) {
			if(PlayerManager.Instance.Players.ContainsKey(Authentication.Instance.CurrentUser.UserId)) {
				Player currentPlayer = PlayerManager.Instance.Players[Authentication.Instance.CurrentUser.UserId];
				NameText.text = currentPlayer.name;
				CurrencyText.text = currentPlayer.currency + " BITS";
			}
		}
	}
}
