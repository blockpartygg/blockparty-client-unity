using UnityEngine;

public class FastestFingerPlayerController : MonoBehaviour {
    public bool MoveReleased;

    void Update() {
        MoveReleased = Input.GetMouseButtonUp(0);
    }
}