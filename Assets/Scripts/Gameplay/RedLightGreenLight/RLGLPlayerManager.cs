using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerManager : MonoBehaviour {
	public Dictionary<string, RLGLPlayer> Players;
	public RLGLPlayer RLGLPlayerPrefab;
	public string LocalPlayerId;
	public RLGLCameraController CameraController;

	void Awake() {
		Players = new Dictionary<string, RLGLPlayer>();
	}

	public void SpawnPlayer(string playerId, bool isLocalPlayer) {
		RLGLPlayer player = Instantiate(RLGLPlayerPrefab, Vector3.zero, Quaternion.identity);
		player.name = "Player " + playerId;
		player.transform.SetParent(transform);
		player.GetComponent<PlayerAvatarRenderer>().SetPlayer(playerId);
		Players.Add(playerId, player);
		if(isLocalPlayer) {
			LocalPlayerId = playerId;
			player.GetComponent<RLGLPlayerController>().enabled = true;
			CameraController.SetTarget(player);
		}
	}

	public void UpdatePlayer(string playerId, bool active, int positionX, int positionZ, bool moving) {
		Players[playerId].Active = active;
		Players[playerId].Position.x = positionX;
		Players[playerId].Position.z = positionZ;
		Players[playerId].Moving = moving;
	}
}
