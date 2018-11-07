using UnityEngine;
using DG.Tweening;

public class FastestFingerPlayerJumper : MonoBehaviour {
    public FastestFingerPlayer Player;
    public float JumpHeight = 1f;
    public float JumpDuration = 0.1f;
    bool isMoving;
    
    void Update() {
        if((transform.position.x != Player.Position.x || transform.position.z != Player.Position.z) && !isMoving) {
            isMoving = true;
            Vector3 target = new Vector3(Player.Position.x, 0, Player.Position.z);
            transform.DOJump(target, JumpHeight, 1, JumpDuration).OnComplete(() => isMoving = false);
        }
    }
}