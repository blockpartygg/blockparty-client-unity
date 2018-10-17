using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlayerRenderer : MonoBehaviour {
	RLGLPlayer player;
	Vector3 position;

	void Awake() {
		position = new Vector3();
	}

	public void SetPlayer(RLGLPlayer player) {
		this.player = player;
	}

	void Update() {
		position.x = player.positionX;
		position.z = player.positionZ;
		transform.position = position;
	}
}
