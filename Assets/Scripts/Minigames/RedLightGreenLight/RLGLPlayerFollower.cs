using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerFollower : MonoBehaviour {
	RLGLPlayerManager playerManager;
	RLGLEliminationManager eliminationManager;
	GameObject target;
	Vector3 positionOffset = new Vector3(6, 10, -10);
	public float MoveSpeed;

	void Awake() {
		playerManager = GameObject.Find("Minigame Manager").GetComponent<RLGLPlayerManager>();
		eliminationManager = GameObject.Find("Minigame Manager").GetComponent<RLGLEliminationManager>();
	}

	void Update() {
		target = null;
		if(AuthenticationManager.Instance.CurrentUser != null) {
			if(playerManager.Players.ContainsKey(AuthenticationManager.Instance.CurrentUser.UserId)) {
				if(playerManager.Players[AuthenticationManager.Instance.CurrentUser.UserId].active) {
					target = playerManager.Players[AuthenticationManager.Instance.CurrentUser.UserId].gameObject;
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
	}

	void FixedUpdate() {
		if(target != null) {
			Vector3 targetPosition = new Vector3(target.transform.position.x + positionOffset.x, positionOffset.y, target.transform.position.z + positionOffset.z);
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);
		}
	}
}
