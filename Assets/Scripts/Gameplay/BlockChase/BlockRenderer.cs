using UnityEngine;

public class BlockRenderer : MonoBehaviour {
    public BlockState State;
    bool wasActive;

    void Start() {
        wasActive = true;
    }

    void FixedUpdate() {
        transform.position = new Vector3(State.Position.x, 0f, State.Position.z);
    }
}