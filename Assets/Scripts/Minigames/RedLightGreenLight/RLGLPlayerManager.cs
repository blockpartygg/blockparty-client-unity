using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerManager : MonoBehaviour {
	public Dictionary<string, RLGLPlayer> Players;
	public RLGLPlayer RLGLPlayerPrefab;

	void Awake() {
		Players = new Dictionary<string, RLGLPlayer>();
	}

	public void SetPlayer(string playerId, bool active, int positionX, int positionZ, bool moving) {
		if(!Players.ContainsKey(playerId)) {
			RLGLPlayer player = Instantiate(RLGLPlayerPrefab, Vector3.zero, Quaternion.identity);
			player.transform.SetParent(transform);

			Players.Add(playerId, player);
			Players[playerId].Initialize(playerId, active, positionX, positionZ, moving);
		}
		else {
			if(Authentication.Instance.CurrentUser == null || playerId != Authentication.Instance.CurrentUser.UserId) {
				Players[playerId].active = active;
				Players[playerId].positionX = positionX;
				Players[playerId].positionZ = positionZ;
				Players[playerId].moving = moving;
			}
		}
	}
}
