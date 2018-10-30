using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveGameNavigator : MonoBehaviour {
	public void LeaveGame() {
		if(AuthenticationManager.Instance.CurrentUser != null) {
			PlayerManager.Instance.SetPlayerPlaying(AuthenticationManager.Instance.CurrentUser.UserId, false);
		}

		SceneNavigator.Instance.StopPlaying();
	}
}
