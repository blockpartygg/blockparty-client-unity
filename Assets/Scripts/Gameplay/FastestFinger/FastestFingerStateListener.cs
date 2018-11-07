using UnityEngine;
using System.Collections.Generic;
using BestHTTP.SocketIO;

public class FastestFingerStateListener : MonoBehaviour {
    public FastestFingerPlayerManager PlayerManager;

    void Start() {
        if(!SocketManager.Instance.IsConnected) {
            SocketManager.Instance.Connect();
        }
        SocketManager.Instance.Socket.On("fastestFinger/state", OnStateReceived);
    }

    void OnDestroy() {
        SocketManager.Instance.Socket.Off();
    }

    void OnStateReceived(Socket socket, Packet packet, params object[] args) {
        Dictionary<string, object> state = (Dictionary<string, object>)args[0];
        Dictionary<string, object> players = (Dictionary<string, object>)state["players"];

        foreach(object playerObject in players.Values) {
            Dictionary<string, object> playerDictionary = (Dictionary<string, object>)playerObject;
            string playerId = playerDictionary["id"].ToString();
            bool active = bool.Parse(playerDictionary["active"].ToString());
            Dictionary<string, object> positionDictionary = (Dictionary<string, object>)playerDictionary["position"];
            float x = float.Parse(positionDictionary["x"].ToString());
            float z = float.Parse(positionDictionary["z"].ToString());
            bool moving = (bool)playerDictionary["moving"];

            if(!PlayerManager.Players.ContainsKey(playerId)) {
                bool isLocalPlayer = false;
                if(AuthenticationManager.Instance.CurrentUser != null && playerId == AuthenticationManager.Instance.CurrentUser.UserId) {
                    isLocalPlayer = true;
                }
                PlayerManager.SpawnPlayer(playerId, isLocalPlayer);
                PlayerManager.UpdatePlayer(playerId, active, new Vector3(x, 0, z), moving);
            }
            else {
                if(playerId != PlayerManager.LocalPlayerId) {
                    PlayerManager.UpdatePlayer(playerId, active, new Vector3(x, 0, z), moving);
                }
            }
        }
    }
}