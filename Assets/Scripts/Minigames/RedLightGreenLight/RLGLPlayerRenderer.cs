using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class RLGLPlayerRenderer : MonoBehaviour {
	RLGLPlayer player;
	bool isMoving;
	bool isEliminated;
	Tweener positionTweener;
	Tweener scaleTweener;

	void Awake() {
		player = GetComponent<RLGLPlayer>();
	}

	void Update() {
		if((transform.position.x != player.positionX || transform.position.z != player.positionZ) && !isMoving) {
			isMoving = true;
			Vector3 position = new Vector3(player.positionX, 0, player.positionZ);
			transform.DOJump(position, 1f, 1, 0.1f).OnComplete(() => isMoving = false);
		}

		if(!isEliminated && !player.active) {
			isEliminated = true;
			transform.DOScale(Vector3.zero, 3.0f);
			transform.DORotate(new Vector3(0.0f, 1080.0f, 0.0f), 3.0f, RotateMode.FastBeyond360);
		}
	}
}
