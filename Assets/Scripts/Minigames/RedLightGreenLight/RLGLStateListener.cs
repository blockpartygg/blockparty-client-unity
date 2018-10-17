using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;


public class RLGLStateListener : MonoBehaviour {
	RLGLGreenLightManager greenLightManager;
	RLGLPlayerManager playerManager;

	void Awake() {
		greenLightManager = GetComponent<RLGLGreenLightManager>();
		playerManager = GetComponent<RLGLPlayerManager>();
	}

	void Start() {
		SocketIO.Instance.Socket.On("redLightGreenLight/state", OnStateReceived);
	}

	void OnStateReceived(Socket socket, Packet packet, params object[] args) {
		Dictionary<string, object> state = (Dictionary<string, object>)args[0];

		Dictionary<string, object> players = (Dictionary<string, object>)state["players"];
		foreach(object playerObject in players.Values) {
			Dictionary<string, object> playerDictionary = (Dictionary<string, object>)playerObject;
			string id = playerDictionary["id"].ToString();
			int positionX = int.Parse(playerDictionary["positionX"].ToString());
			int positionZ = int.Parse(playerDictionary["positionZ"].ToString());
			bool moving = (bool)playerDictionary["moving"];
			playerManager.SetPlayer(id, positionX, positionZ, moving);
		}

		greenLightManager.SetGreenLight((bool)state["greenLight"]);
	}
}
