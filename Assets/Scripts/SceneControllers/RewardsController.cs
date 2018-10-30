﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardsController : MonoBehaviour {
	public PlayerRenderer PlayerRenderer;

	void Start() {
		Analytics.Instance.LogGameEnded();
		Analytics.Instance.LogAdOffered();
		if(Authentication.Instance.CurrentUser != null) {
			PlayerRenderer.SetPlayer(Authentication.Instance.CurrentUser.UserId);
		}
	}
	public void WatchAd() {
		if(Advertisement.IsReady("rewardedVideo")) {
			ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
			Analytics.Instance.LogAdStarted();
		}
	}

	void HandleShowResult(ShowResult result) {
		switch(result) {
			case ShowResult.Finished:
				PlayerManager.Instance.TransactPlayerCurrency(Authentication.Instance.CurrentUser.UserId, 100);
				break;
			case ShowResult.Skipped:
				Debug.Log("Ad was skipped");
				break;
			case ShowResult.Failed:
				Debug.Log("Ad failed to show");
				break;
		}
		Analytics.Instance.LogAdCompleted();
	}

	public void PurchaseSkin() {
		long skin = PlayerManager.Instance.Players[Authentication.Instance.CurrentUser.UserId].currentSkin == 0 ? 1 : 0;
		PlayerManager.Instance.PurchaseAndSetSkin(Authentication.Instance.CurrentUser.UserId, skin, 100);
	}

	public void GoHome() {
		if(Authentication.Instance.CurrentUser != null) {
			PlayerManager.Instance.SetPlayerPlaying(Authentication.Instance.CurrentUser.UserId, false);
		}
		
		SceneNavigator.Instance.StopPlaying();
	}
}
