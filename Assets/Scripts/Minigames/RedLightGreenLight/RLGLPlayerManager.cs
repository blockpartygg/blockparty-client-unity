using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerManager : MonoBehaviour {
	public Dictionary<string, RLGLPlayer> Players;
	public Dictionary<string, RLGLPlayerRenderer> PlayerRenderers;
	public RLGLPlayerRenderer PlayerRendererPrefab;

	void Awake() {
		Players = new Dictionary<string, RLGLPlayer>();
		PlayerRenderers = new Dictionary<string, RLGLPlayerRenderer>();
	}

	public void SetPlayer(string playerId, int positionX, int positionZ, bool moving) {
		if(!Players.ContainsKey(playerId)) {
			Players.Add(playerId, new RLGLPlayer(positionX, positionZ, moving));
		}
		else {
			if(playerId != Authentication.Instance.CurrentUser.UserId) {
				Players[playerId].positionX = positionX;
				Players[playerId].positionZ = positionZ;
				Players[playerId].moving = moving;
			}
		}

		if(!PlayerRenderers.ContainsKey(playerId)) {
			PlayerRenderers.Add(playerId, Instantiate(PlayerRendererPrefab, new Vector3(positionX, 0, positionZ), Quaternion.identity));
			PlayerRenderers[playerId].SetPlayer(playerId, Players[playerId]);
		}
	}
}
