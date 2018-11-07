using UnityEngine;

public class FastestFingerCameraController : MonoBehaviour {
    public FastestFingerPlayer Target;
    public Vector3 Offset;
    public float MovementSpeed;
    public FastestFingerPlayerManager PlayerManager;

    public void SetTarget(FastestFingerPlayer target) {
        Target = target;
        transform.position = Target.transform.position + Offset;
    }

    void LateUpdate() {
        if(Target != null) {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position + Offset, MovementSpeed * Time.deltaTime);
        }
    }
}