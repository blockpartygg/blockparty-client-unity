using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveGameNavigator : MonoBehaviour {
	public void LeaveGame() {
		if(Authentication.Instance.CurrentUser != null) {
			PlayerManager.Instance.SetPlayerPlaying(Authentication.Instance.CurrentUser.UserId, false);
		}
		
		SceneNavigator.Instance.StopPlaying();
	}
}
