using UnityEngine;

public class FastestFingerPlayerMover : MonoBehaviour {
    public FastestFingerPlayer Player;
    public FastestFingerPlayerController Controller;
    
    void Update() {
        if(Controller.enabled) {
            if(Controller.MoveReleased) {
                Player.Position.z++;
            }
        }
    }
}