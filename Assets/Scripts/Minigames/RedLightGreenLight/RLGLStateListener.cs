using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;


public class RLGLStateListener : MonoBehaviour {
	RLGLGreenLightManager greenLightManager;
	RLGLEliminationManager eliminationManager;
	RLGLPlayerManager playerManager;

	void Awake() {
		greenLightManager = GetComponent<RLGLGreenLightManager>();
		eliminationManager = GetComponent<RLGLEliminationManager>();
		playerManager = GetComponent<RLGLPlayerManager>();
	}

	void Start() {
		if(!SocketIO.Instance.IsConnected) {
			SocketIO.Instance.Connect();
		}
		SocketIO.Instance.Socket.On("redLightGreenLight/state", OnStateReceived);
		SocketIO.Instance.Socket.On("redLightGreenLight/eliminatePlayer", OnEliminatePlayerReceived);
	}

	void OnStateReceived(Socket socket, Packet packet, params object[] args) {
		Dictionary<string, object> state = (Dictionary<string, object>)args[0];
		Dictionary<string, object> players = (Dictionary<string, object>)state["players"];
		
		foreach(object playerObject in players.Values) {
			Dictionary<string, object> playerDictionary = (Dictionary<string, object>)playerObject;
			string id = playerDictionary["id"].ToString();
			bool active = bool.Parse(playerDictionary["active"].ToString());
			int positionX = int.Parse(playerDictionary["positionX"].ToString());
			int positionZ = int.Parse(playerDictionary["positionZ"].ToString());
			bool moving = (bool)playerDictionary["moving"];
			playerManager.SetPlayer(id, active, positionX, positionZ, moving);
		}

		greenLightManager.SetGreenLight((bool)state["greenLight"]);

		eliminationManager.SetEliminationCountdown((double)state["playerEliminationCountdown"]);
		eliminationManager.PlayerToEliminate = (string)state["playerToEliminate"];
	}

	void OnEliminatePlayerReceived(Socket socket, Packet packet, params object[] args) {
		string playerId = (string)args[0];
		playerManager.Players[playerId].active = false;
	}
}
