using UnityEngine;

public class PlayerMover : MonoBehaviour {
	public PlayerController Controller;
	public float MovementSpeed;
	public float RotationSpeed;

	void FixedUpdate() {
		if(Controller.Direction != Vector2.zero) {
			Vector3 direction = new Vector3(Controller.Direction.x, 0f, Controller.Direction.y);
			transform.Translate(direction * MovementSpeed * Time.deltaTime, Space.World);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction * -1f), RotationSpeed * Time.deltaTime);
		}
	}
}
