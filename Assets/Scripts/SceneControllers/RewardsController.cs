using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardsController : MonoBehaviour {
	public void WatchAd() {
		if(Advertisement.IsReady("rewardedVideo")) {
			ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
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
	}

	public void PurchaseSkin() {
		PlayerManager.Instance.TransactPlayerCurrency(Authentication.Instance.CurrentUser.UserId, -100);
	}

	public void GoHome() {
		SceneNavigator.Instance.StopPlaying();
	}
}
