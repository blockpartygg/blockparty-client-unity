using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLStateEmitter : MonoBehaviour {
	RLGLPlayerManager playerManager;

	void Awake() {
		playerManager = GetComponent<RLGLPlayerManager>();
	}

	void Start() {
		SocketIO.Instance.Socket.Emit("redLightGreenLight/joinGame", Authentication.Instance.CurrentUser.UserId);
	}

	void Update() {
		if(playerManager.Players.ContainsKey(Authentication.Instance.CurrentUser.UserId)) {
			SocketIO.Instance.Socket.Emit("redLightGreenLight/playerState", Authentication.Instance.CurrentUser.UserId, playerManager.Players[Authentication.Instance.CurrentUser.UserId].ToJSON());
		}
	}
}
