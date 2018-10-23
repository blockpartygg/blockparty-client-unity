using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLStateEmitter : MonoBehaviour {
	RLGLPlayerManager playerManager;

	void Awake() {
		playerManager = GetComponent<RLGLPlayerManager>();
	}

	void Start() {
		if(!SocketIO.Instance.IsConnected) {
			SocketIO.Instance.Connect();
		}
		if(Authentication.Instance.CurrentUser != null) {
			SocketIO.Instance.Socket.Emit("redLightGreenLight/joinGame", Authentication.Instance.CurrentUser.UserId);
		}
	}

	void Update() {
		if(Authentication.Instance.CurrentUser != null && playerManager.Players.ContainsKey(Authentication.Instance.CurrentUser.UserId)) {
			SocketIO.Instance.Socket.Emit("redLightGreenLight/playerState", Authentication.Instance.CurrentUser.UserId, playerManager.Players[Authentication.Instance.CurrentUser.UserId].ToJSON());
		}
	}
}
