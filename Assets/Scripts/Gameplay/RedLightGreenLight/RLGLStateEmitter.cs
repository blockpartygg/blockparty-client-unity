using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLStateEmitter : MonoBehaviour {
	RLGLPlayerManager playerManager;

	void Awake() {
		playerManager = GetComponent<RLGLPlayerManager>();
	}

	void Start() {
		if(!SocketManager.Instance.IsConnected) {
			SocketManager.Instance.Connect();
		}
		if(AuthenticationManager.Instance.CurrentUser != null) {
			SocketManager.Instance.Socket.Emit("redLightGreenLight/joinGame", AuthenticationManager.Instance.CurrentUser.UserId);
		}
	}

	void Update() {
		if(AuthenticationManager.Instance.CurrentUser != null && playerManager.Players.ContainsKey(AuthenticationManager.Instance.CurrentUser.UserId)) {
			SocketManager.Instance.Socket.Emit("redLightGreenLight/playerState", AuthenticationManager.Instance.CurrentUser.UserId, playerManager.Players[AuthenticationManager.Instance.CurrentUser.UserId].ToJSON());
		}
	}
}
