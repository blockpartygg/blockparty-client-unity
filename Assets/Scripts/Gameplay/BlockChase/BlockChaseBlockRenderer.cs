using UnityEngine;

public class BlockChaseBlockRenderer : MonoBehaviour {
    public BlockChaseBlockState State;
    bool wasActive;

    void Start() {
        wasActive = true;
    }

    void FixedUpdate() {
        transform.position = new Vector3(State.Position.x, 0f, State.Position.z);
    }
}