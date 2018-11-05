using System.Collections.Generic;
using UnityEngine;

public class MinigamePlayerManager : MonoBehaviour {
	public Dictionary<string, GameObject> Players;
	public GameObject PlayerPrefab;
	public string LocalPlayerId;
	public CameraController CameraController;

	void Awake() {
		Players = new Dictionary<string, GameObject>();
	}

	public void SpawnPlayer(string playerId, bool isLocalPlayer) {
		Vector3 position = new Vector3(Random.Range(-32f, 32f), 0f, Random.Range(-32f, 32f));
		Quaternion rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
		GameObject player = Instantiate(PlayerPrefab, position, rotation);
		Players.Add(playerId, player);
		player.name = "Player " + playerId;
		player.transform.SetParent(transform);
		if(isLocalPlayer) {
			LocalPlayerId = playerId;
			player.GetComponent<PlayerController>().enabled = true;
			CameraController.SetTarget(player);
		}
	}
}
