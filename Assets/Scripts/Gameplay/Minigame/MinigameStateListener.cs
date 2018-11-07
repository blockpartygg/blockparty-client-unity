using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;

public class MinigameStateListener : MonoBehaviour {
	public MinigamePlayerManager PlayerManager;
	public BlockChaseBlockManager BlockManager;

	void Start() {
		SocketManager.Instance.Socket.On("blockChase/state", OnStateReceived);
	}

	void OnDestroy() {
		SocketManager.Instance.Socket.Off();
	}

	void OnStateReceived(Socket socket, Packet packet, params object[] args) {
		Dictionary<string, object> state = (Dictionary<string, object>)args[0];
		Dictionary<string, object> players = (Dictionary<string, object>)state["players"];

		foreach(Dictionary<string, object> player in players.Values) {
			string id = player["id"].ToString();
			bool active = bool.Parse(player["active"].ToString());
			Dictionary<string, object> position = (Dictionary<string, object>)player["position"];
			float x = float.Parse(position["x"].ToString());
			float z = float.Parse(position["z"].ToString());

			if(!PlayerManager.Players.ContainsKey(id)) {
				bool isLocalPlayer = false;
				if(AuthenticationManager.Instance.CurrentUser != null && id == AuthenticationManager.Instance.CurrentUser.UserId) {
					isLocalPlayer = true;
				}
				PlayerManager.SpawnPlayer(id, isLocalPlayer);
				BlockChasePlayerState playerState = PlayerManager.Players[id].GetComponent<BlockChasePlayerState>();
				playerState.Active = active;
				playerState.Position.x = x;
				playerState.Position.z = z;
			}
			else {
				if(id != PlayerManager.LocalPlayerId) {
					BlockChasePlayerState playerState = PlayerManager.Players[id].GetComponent<BlockChasePlayerState>();
					playerState.Active = active;
					playerState.Position.x = x;
					playerState.Position.z = z;
				}
			}
		}

		Dictionary<string, object> blocks = (Dictionary<string, object>)state["blocks"];
		foreach(KeyValuePair<string, object> blockObject in blocks) {
			string id = blockObject.Key;
			Dictionary<string, object> block = (Dictionary<string, object>)blockObject.Value;
			bool active = bool.Parse(block["active"].ToString());
			Dictionary<string, object> position = (Dictionary<string, object>)block["position"];
			float x = float.Parse(position["x"].ToString());
			float z = float.Parse(position["z"].ToString());

			if(!BlockManager.Blocks.ContainsKey(id)) {
				BlockManager.SpawnBlock(id);
			}

			BlockChaseBlockState blockState = BlockManager.Blocks[id].GetComponent<BlockChaseBlockState>();
			blockState.Active = active;
			if(!blockState.Active) {
				BlockManager.DestroyBlock(id);
			}
			blockState.Position.x = x;
			blockState.Position.z = z;
		}
	}
}
