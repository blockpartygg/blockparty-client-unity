using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoystickController : MonoBehaviour {
	bool isTouching;
	Vector2 originPosition;
	Vector2 currentPosition;
	public Vector2 Direction;
	
	void Awake() {
		originPosition = new Vector2(Screen.width / 2, Screen.height / 2);
	}

	void Update () {
		// if(Input.GetMouseButtonDown(0)) {
		// 	// originPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
		// 	originPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		// }
		if(Input.GetMouseButton(0)) {
			isTouching = true;
			// currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
			// currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
			currentPosition = Input.mousePosition;
		}
		else {
			isTouching = false;
		}
	}

	void FixedUpdate() {
		if(isTouching) {
			Vector2 offsetPosition = currentPosition - originPosition;
			Direction = Vector2.ClampMagnitude(offsetPosition, 1f); // Screen space coordinate axis is reverse of world space
		}
		else {
			Direction = Vector2.zero;
		}
	}
}
