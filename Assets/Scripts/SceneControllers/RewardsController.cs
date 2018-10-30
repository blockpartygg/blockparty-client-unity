using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardsController : MonoBehaviour {
	public PlayerRenderer PlayerRenderer;

	void Start() {
		AnalyticsManager.Instance.LogGameEnded();
		AnalyticsManager.Instance.LogAdOffered();
		if(AuthenticationManager.Instance.CurrentUser != null) {
			PlayerRenderer.SetPlayer(AuthenticationManager.Instance.CurrentUser.UserId);
		}
	}
	public void WatchAd() {
		if(Advertisement.IsReady("rewardedVideo")) {
			ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
			AnalyticsManager.Instance.LogAdStarted();
		}
	}

	void HandleShowResult(ShowResult result) {
		switch(result) {
			case ShowResult.Finished:
				PlayerManager.Instance.TransactPlayerCurrency(AuthenticationManager.Instance.CurrentUser.UserId, 100);
				break;
			case ShowResult.Skipped:
				Debug.Log("Ad was skipped");
				break;
			case ShowResult.Failed:
				Debug.Log("Ad failed to show");
				break;
		}
		AnalyticsManager.Instance.LogAdCompleted();
	}

	public void PurchaseSkin() {
		long skin = PlayerManager.Instance.Players[AuthenticationManager.Instance.CurrentUser.UserId].currentSkin == 0 ? 1 : 0;
		PlayerManager.Instance.PurchaseAndSetSkin(AuthenticationManager.Instance.CurrentUser.UserId, skin, 100);
	}

	public void GoHome() {
		if(AuthenticationManager.Instance.CurrentUser != null) {
			PlayerManager.Instance.SetPlayerPlaying(AuthenticationManager.Instance.CurrentUser.UserId, false);
		}
		
		SceneNavigator.Instance.StopPlaying();
	}
}
