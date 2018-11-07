using UnityEngine;

public class RLGLPlayerMover : MonoBehaviour {
    public RLGLPlayer Player;
    public RLGLPlayerController Controller;
    RLGLGreenLightManager greenLightManager;
    public int ForwardDistance = 1;
    public int BackwardDistance = -2;
    
    void Awake() {
        greenLightManager = GameObject.Find("Minigame Manager").GetComponent<RLGLGreenLightManager>();
    }

    void Update() {
        if(Controller.enabled) {
            if(Controller.MoveReleased) {
                Player.Position.z += greenLightManager.GreenLight ? ForwardDistance : BackwardDistance;
            }
        }
    }
}