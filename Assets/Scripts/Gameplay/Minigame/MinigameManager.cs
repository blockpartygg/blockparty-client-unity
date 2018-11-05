using UnityEngine;

public class MinigameManager : MonoBehaviour {
    void Start() {
        if(AuthenticationManager.Instance.CurrentUser != null && SocketManager.Instance.IsConnected) {
            SocketManager.Instance.Socket.Emit("blockChase/joinGame", AuthenticationManager.Instance.CurrentUser.UserId);
        }
    }
}