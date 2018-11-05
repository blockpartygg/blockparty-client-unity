using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject Target;
	public Vector3 Offset;
	public float MovementSpeed;

	public void SetTarget(GameObject target) {
		Target = target;
		transform.position = Target.transform.position + Offset;
	}

	void LateUpdate() {
		if(Target != null) {
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position + Offset, MovementSpeed * Time.deltaTime);
		}
	}
}
