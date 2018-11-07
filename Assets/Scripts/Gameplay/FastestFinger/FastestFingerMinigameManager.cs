using UnityEngine; 

public class FastestFingerMinigameManager : MonoBehaviour {
    void Start() {
        if(!SocketManager.Instance.IsConnected) {
            SocketManager.Instance.Connect();
        }

        if(AuthenticationManager.Instance.CurrentUser != null) {
            SocketManager.Instance.Socket.Emit("fastestFinger/joinGame", AuthenticationManager.Instance.CurrentUser.UserId);
        }
    }
}