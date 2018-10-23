using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsController : MonoBehaviour {
	public void WatchAd() {
		Debug.Log("Watch ad");
	}

	public void PurchaseSkin() {
		Debug.Log("Purchase skin");
	}

	public void GoHome() {
		SceneNavigator.Instance.StopPlaying();
	}
}
