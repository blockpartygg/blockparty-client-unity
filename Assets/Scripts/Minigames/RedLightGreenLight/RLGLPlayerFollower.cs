using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerFollower : MonoBehaviour {
	RLGLPlayerManager playerManager;
	RLGLEliminationManager eliminationManager;
	GameObject player;
	Vector3 positionOffset = new Vector3(6, 10, -10);
	Vector3 position = new Vector3();

	void Awake() {
		playerManager = GameObject.Find("Minigame Manager").GetComponent<RLGLPlayerManager>();
		eliminationManager = GameObject.Find("Minigame Manager").GetComponent<RLGLEliminationManager>();
	}

	void Update() {
		if(player == null) {
			if(Authentication.Instance.CurrentUser != null && playerManager.PlayerRenderers.ContainsKey(Authentication.Instance.CurrentUser.UserId)) {
				player = playerManager.PlayerRenderers[Authentication.Instance.CurrentUser.UserId].gameObject;
			}
			else {
				if(playerManager.PlayerRenderers.ContainsKey(eliminationManager.PlayerToEliminate)) {
					player = playerManager.PlayerRenderers[eliminationManager.PlayerToEliminate].gameObject;
				}
			}
		}
		else {
			if((Authentication.Instance.CurrentUser == null || player != playerManager.PlayerRenderers[Authentication.Instance.CurrentUser.UserId].gameObject) && (playerManager.PlayerRenderers.ContainsKey(eliminationManager.PlayerToEliminate) && player != playerManager.PlayerRenderers[eliminationManager.PlayerToEliminate].gameObject)) {
				player = playerManager.PlayerRenderers[eliminationManager.PlayerToEliminate].gameObject;
			}

			Vector3 target = new Vector3(player.transform.position.x + positionOffset.x, positionOffset.y, player.transform.position.z + positionOffset.z);
			transform.position = Vector3.MoveTowards(transform.position, target, 0.2f);
		}
	}
}
