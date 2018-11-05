using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameStateEmitter : MonoBehaviour {
	public MinigamePlayerManager MinigamePlayerManager;

	void Update() {
		if(AuthenticationManager.Instance.CurrentUser != null && MinigamePlayerManager.Players.ContainsKey(AuthenticationManager.Instance.CurrentUser.UserId)) {
			SocketManager.Instance.Socket.Emit("blockChase/playerState", AuthenticationManager.Instance.CurrentUser.UserId, MinigamePlayerManager.Players[AuthenticationManager.Instance.CurrentUser.UserId].GetComponent<BlockChasePlayerState>().ToJSON());
		}
	}
}
