using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RLGLPlayerController : MonoBehaviour {
	RLGLPlayerManager playerManager;
	RLGLGreenLightManager greenLightManager;
	RLGLPlayer player;

	void Awake() {
		playerManager = GetComponent<RLGLPlayerManager>();
		greenLightManager = GetComponent<RLGLGreenLightManager>();
	}

	void Update() {
		if(player == null) {
			if(playerManager.Players.ContainsKey(Authentication.Instance.CurrentUser.UserId)) {
				player = playerManager.Players[Authentication.Instance.CurrentUser.UserId];
			}
		}
		else {
			if(Input.GetMouseButtonUp(0)) {	
				player.positionZ += greenLightManager.GreenLight ? 1 : -2;
			}
		}
	}
}
