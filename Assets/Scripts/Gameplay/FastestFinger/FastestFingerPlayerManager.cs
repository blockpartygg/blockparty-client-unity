using UnityEngine;
using System.Collections.Generic;

public class FastestFingerPlayerManager : MonoBehaviour {
	public Dictionary<string, FastestFingerPlayer> Players;
	public FastestFingerPlayer PlayerPrefab;
	public GameObject PlayersObject;
	public string LocalPlayerId;
	public FastestFingerCameraController CameraController;

	void Awake() {
		Players = new Dictionary<string, FastestFingerPlayer>();
	}

	public void SpawnPlayer(string playerId, bool isLocalPlayer) {
		FastestFingerPlayer player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
		player.name = "Player " + playerId;
		player.transform.SetParent(PlayersObject.transform);
		player.GetComponent<PlayerAvatarRenderer>().SetPlayer(playerId);
		Players.Add(playerId, player);
		if(isLocalPlayer) {
			LocalPlayerId = playerId;
			player.GetComponent<FastestFingerPlayerController>().enabled = true;
			CameraController.SetTarget(player);
		}
	}

	public void UpdatePlayer(string playerId, bool active, Vector3 position, bool moving) {
		Players[playerId].Active = active;
		Players[playerId].Position = position;
		Players[playerId].Moving = moving;
	}
}