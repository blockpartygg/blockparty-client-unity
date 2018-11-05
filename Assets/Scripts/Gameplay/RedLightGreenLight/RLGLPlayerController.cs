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
			if(AuthenticationManager.Instance.CurrentUser != null && playerManager.Players.ContainsKey(AuthenticationManager.Instance.CurrentUser.UserId)) {
				player = playerManager.Players[AuthenticationManager.Instance.CurrentUser.UserId];
			}
		}
		else {
			if(GameManager.Instance.State == GameManager.GameState.MinigamePlay && player.active && Input.GetMouseButtonUp(0)) {	
				player.positionZ += greenLightManager.GreenLight ? 1 : -2;
			}
		}
	}
}
