using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RLGLPlayerRenderer : MonoBehaviour {
	RLGLPlayer player;
	Vector3 position;
	bool isMoving;

	void Awake() {
		position = new Vector3();
	}

	public void SetPlayer(RLGLPlayer player) {
		this.player = player;
	}

	void Update() {
		if(position.z != player.positionZ && !isMoving) {
			isMoving = true;
			position.x = player.positionX;
			position.z = player.positionZ;
			transform.DOJump(position, 1f, 1, 0.1f).OnComplete(() => isMoving = false);
		}
	}
}
