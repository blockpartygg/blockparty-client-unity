using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerFollower : MonoBehaviour {
	GameObject target;
	Vector3 positionOffset = new Vector3(6, 10, -10);
	Vector3 position = new Vector3();

	void Awake() {
		target = GameObject.Find("Cube");
	}

	void Update() {
		position.x = target.transform.position.x + positionOffset.x;
		position.y = positionOffset.y;
		position.z = target.transform.position.z + positionOffset.z;
		transform.position = position;
	}
}
