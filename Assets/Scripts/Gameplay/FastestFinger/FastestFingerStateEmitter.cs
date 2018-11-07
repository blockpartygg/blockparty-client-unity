using UnityEngine;

public class FastestFingerStateEmitter : MonoBehaviour {
    public FastestFingerPlayerManager PlayerManager;

    void Update() {
        if(AuthenticationManager.Instance.CurrentUser != null && PlayerManager.Players.ContainsKey(AuthenticationManager.Instance.CurrentUser.UserId)) {
            SocketManager.Instance.Socket.Emit("fastestFinger/playerState", AuthenticationManager.Instance.CurrentUser.UserId, PlayerManager.Players[AuthenticationManager.Instance.CurrentUser.UserId].ToJSON());
        }
    }
}