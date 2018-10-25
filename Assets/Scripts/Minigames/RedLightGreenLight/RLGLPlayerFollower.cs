using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerFollower : MonoBehaviour {
	RLGLPlayerManager playerManager;
	RLGLEliminationManager eliminationManager;
	GameObject target;
	Vector3 positionOffset = new Vector3(6, 10, -10);

	void Awake() {
		playerManager = GameObject.Find("Minigame Manager").GetComponent<RLGLPlayerManager>();
		eliminationManager = GameObject.Find("Minigame Manager").GetComponent<RLGLEliminationManager>();
	}

	void Update() {
		target = null;
		if(Authentication.Instance.CurrentUser != null) {
			if(playerManager.Players.ContainsKey(Authentication.Instance.CurrentUser.UserId)) {
				if(playerManager.Players[Authentication.Instance.CurrentUser.UserId].active) {
					target = playerManager.Players[Authentication.Instance.CurrentUser.UserId].gameObject;
				}
			}
		}
		if(target == null) {
			if(eliminationManager.PlayerToEliminate != null) {
				if(playerManager.Players.ContainsKey(eliminationManager.PlayerToEliminate)) {
					target = playerManager.Players[eliminationManager.PlayerToEliminate].gameObject;
				}
			}
		}

		if(target != null) {
			Vector3 targetPosition = new Vector3(target.transform.position.x + positionOffset.x, positionOffset.y, target.transform.position.z + positionOffset.z);
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.2f);
		}
	}
}
