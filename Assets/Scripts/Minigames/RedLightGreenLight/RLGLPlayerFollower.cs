using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerFollower : MonoBehaviour {
	RLGLPlayerManager playerManager;
	GameObject player;
	Vector3 positionOffset = new Vector3(6, 10, -10);
	Vector3 position = new Vector3();

	void Awake() {
		playerManager = GameObject.Find("Minigame Manager").GetComponent<RLGLPlayerManager>();
	}

	void Update() {
		if(player == null) {
			if(playerManager.PlayerRenderers.ContainsKey(Authentication.Instance.CurrentUser.UserId)) {
				player = playerManager.PlayerRenderers[Authentication.Instance.CurrentUser.UserId].gameObject;
			}
		}
		else {
			position.x = player.transform.position.x + positionOffset.x;
			position.y = positionOffset.y;
			position.z = player.transform.position.z + positionOffset.z;
			transform.position = position;
		}
	}
}
