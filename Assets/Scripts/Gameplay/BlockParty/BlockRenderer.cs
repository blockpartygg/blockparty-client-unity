using UnityEngine;

public class BlockRenderer: MonoBehaviour {
    public Block Block;

    void Update() {
        transform.position = transform.parent.position + new Vector3(Block.Position.x, Block.Position.y, 0f);
    }
}