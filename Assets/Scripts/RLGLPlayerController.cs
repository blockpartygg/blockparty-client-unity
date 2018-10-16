using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerController : MonoBehaviour {
	Vector3 jumpBeginScale = new Vector3(1.2f, 0.75f, 1f);
	bool isMoving;

	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			transform.localScale = jumpBeginScale; // TODO: tween this
		}
		if(Input.GetMouseButtonUp(0)) {
			if(!isMoving) {
				isMoving = true;
				transform.Translate(Vector3.forward);
				transform.localScale = Vector3.one;
				isMoving = false;
			}
		}
	}
}
